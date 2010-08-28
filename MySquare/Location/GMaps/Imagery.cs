//based on GMaps.NET code
using System.Drawing;
using System;
using System.Diagnostics;
using System.Threading;
using System.Net;
namespace RisingMobility.Mobile.Location
{
    enum MapType
    {
        GoogleMap,
        GoogleMapChina,
        GoogleMapKorea,
        GoogleSatellite,
        GoogleSatelliteChina,
        GoogleSatelliteKorea,
        GoogleLabels,
        GoogleLabelsChina,
        GoogleLabelsKorea,
        GoogleTerrain,
        GoogleTerrainChina,
        GoogleTerrainKorea
    }
    static class Imagery
    {
        // Google version strings
        const string VersionGoogleMap = "m@130";
        const string VersionGoogleSatellite = "66";
        const string VersionGoogleLabels = "h@130";
        const string VersionGoogleTerrain = "t@125,r@130";
        const string SecGoogleWord = "Galileo";

        // Google (China) version strings
        const string VersionGoogleMapChina = "m@130";
        const string VersionGoogleSatelliteChina = "s@66";
        const string VersionGoogleLabelsChina = "h@130";
        const string VersionGoogleTerrainChina = "t@125,r@130";

        // Google (Korea) version strings
        const string VersionGoogleMapKorea = "kr1.12";
        const string VersionGoogleSatelliteKorea = "66";
        const string VersionGoogleLabelsKorea = "kr1t.12";


        /// <summary>
        /// gets secure google words based on position
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="sec1"></param>
        /// <param name="sec2"></param>
        static void GetSecGoogleWords(Point pos, out string sec1, out string sec2)
        {
            sec1 = ""; // after &x=...
            sec2 = ""; // after &zoom=...
            int seclen = ((pos.X * 3) + pos.Y) % 8;
            sec2 = SecGoogleWord.Substring(0, seclen);
            if (pos.Y >= 10000 && pos.Y < 100000)
            {
                sec1 = "&s=";
            }
        }

        /// <summary>
        /// gets server num based on position
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        static int GetServerNum(Point pos, int max)
        {
            return (pos.X + 2 * pos.Y) % max;
        }


        static string GetImageryUrl(MapType type, Point pos, int zoom)
        {
            string language = System.Globalization.CultureInfo.CurrentCulture.ToString();
            language = "en-US";
            if (language.StartsWith("zh"))
            {
                if (type == MapType.GoogleMap)
                    type = MapType.GoogleMapChina;
                else if (type == MapType.GoogleSatellite)
                    type = MapType.GoogleSatelliteChina;
                else if (type == MapType.GoogleLabels)
                    type = MapType.GoogleLabelsChina;
                else if (type == MapType.GoogleTerrain)
                    type = MapType.GoogleTerrainChina;

            }
            else if (language == "ko")
            {

                if (type == MapType.GoogleMap)
                    type = MapType.GoogleMapKorea;
                else if (type == MapType.GoogleSatellite)
                    type = MapType.GoogleSatelliteKorea;
                else if (type == MapType.GoogleLabels)
                    type = MapType.GoogleLabelsKorea;
                else if (type == MapType.GoogleTerrain)
                    type = MapType.GoogleTerrainKorea;

            }

            switch (type)
            {
                #region -- Google --
                case MapType.GoogleMap:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(pos, out sec1, out sec2);

                        // http://mt1.google.com/vt/lyrs=m@130&hl=lt&x=18683&s=&y=10413&z=15&s=Galile

                        return string.Format("http://{0}{1}.google.com/{2}/lyrs={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(pos, 4), request, VersionGoogleMap, language, pos.X, sec1, pos.Y, zoom, sec2);
                    }

                case MapType.GoogleSatellite:
                    {
                        string server = "khm";
                        string request = "kh";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(pos, out sec1, out sec2);
                        return string.Format("http://{0}{1}.google.com/{2}/v={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(pos, 4), request, VersionGoogleSatellite, language, pos.X, sec1, pos.Y, zoom, sec2);
                    }

                case MapType.GoogleLabels:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(pos, out sec1, out sec2);

                        // http://mt1.google.com/vt/lyrs=h@107&hl=lt&x=583&y=325&z=10&s=Ga
                        // http://mt0.google.com/vt/lyrs=h@130&hl=lt&x=1166&y=652&z=11&s=Galile

                        return string.Format("http://{0}{1}.google.com/{2}/lyrs={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(pos, 4), request, VersionGoogleLabels, language, pos.X, sec1, pos.Y, zoom, sec2);
                    }

                case MapType.GoogleTerrain:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(pos, out sec1, out sec2);
                        return string.Format("http://{0}{1}.google.com/{2}/v={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(pos, 4), request, VersionGoogleTerrain, language, pos.X, sec1, pos.Y, zoom, sec2);
                    }
                #endregion

                #region -- Google (China) version --
                case MapType.GoogleMapChina:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(pos, out sec1, out sec2);

                        // http://mt3.google.cn/vt/lyrs=m@123&hl=zh-CN&gl=cn&x=3419&y=1720&z=12&s=G

                        return string.Format("http://{0}{1}.google.cn/{2}/lyrs={3}&hl={4}&gl=cn&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(pos, 4), request, VersionGoogleMapChina, "zh-CN", pos.X, sec1, pos.Y, zoom, sec2);
                    }

                case MapType.GoogleSatelliteChina:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(pos, out sec1, out sec2);

                        // http://mt1.google.cn/vt/lyrs=s@59&gl=cn&x=3417&y=1720&z=12&s=Gal

                        return string.Format("http://{0}{1}.google.cn/{2}/lyrs={3}&gl=cn&x={4}{5}&y={6}&z={7}&s={8}", server, GetServerNum(pos, 4), request, VersionGoogleSatelliteChina, pos.X, sec1, pos.Y, zoom, sec2);
                    }

                case MapType.GoogleLabelsChina:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(pos, out sec1, out sec2);

                        // http://mt1.google.cn/vt/imgtp=png32&lyrs=h@123&hl=zh-CN&gl=cn&x=3417&y=1720&z=12&s=Gal

                        return string.Format("http://{0}{1}.google.cn/{2}/imgtp=png32&lyrs={3}&hl={4}&gl=cn&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(pos, 4), request, VersionGoogleLabelsChina, "zh-CN", pos.X, sec1, pos.Y, zoom, sec2);
                    }

                case MapType.GoogleTerrainChina:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(pos, out sec1, out sec2);

                        // http://mt2.google.cn/vt/lyrs=t@108,r@123&hl=zh-CN&gl=cn&x=3418&y=1718&z=12&s=Gali

                        return string.Format("http://{0}{1}.google.com/{2}/lyrs={3}&hl={4}&gl=cn&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(pos, 4), request, VersionGoogleTerrainChina, "zh-CN", pos.X, sec1, pos.Y, zoom, sec2);
                    }
                #endregion

                #region -- Google (Korea) version --
                case MapType.GoogleMapKorea:
                    {
                        string server = "mt";
                        string request = "mt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(pos, out sec1, out sec2);

                        // http://mt0.gmaptiles.co.kr/mt/v=kr1.12&hl=lt&x=876&y=400&z=10&s=Gali

                        var ret = string.Format("http://{0}{1}.gmaptiles.co.kr/{2}/v={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(pos, 4), request, VersionGoogleMapKorea, language, pos.X, sec1, pos.Y, zoom, sec2);
                        return ret;
                    }

                case MapType.GoogleSatelliteKorea:
                    {
                        string server = "khm";
                        string request = "kh";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(pos, out sec1, out sec2);

                        // http://khm1.google.co.kr/kh/v=59&x=873&y=401&z=10&s=Gali

                        return string.Format("http://{0}{1}.google.co.kr/{2}/v={3}&x={4}{5}&y={6}&z={7}&s={8}", server, GetServerNum(pos, 4), request, VersionGoogleSatelliteKorea, pos.X, sec1, pos.Y, zoom, sec2);
                    }

                case MapType.GoogleLabelsKorea:
                    {
                        string server = "mt";
                        string request = "mt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(pos, out sec1, out sec2);

                        // http://mt3.gmaptiles.co.kr/mt/v=kr1t.12&hl=lt&x=873&y=401&z=10&s=Gali

                        return string.Format("http://{0}{1}.gmaptiles.co.kr/{2}/v={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(pos, 4), request, VersionGoogleLabelsKorea, language, pos.X, sec1, pos.Y, zoom, sec2);
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
                #endregion
            }
        }


        /// <summary>
        /// Gets the value of the User-agent HTTP header.
        /// </summary>
        const string UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.1.7) Gecko/20091221 Firefox/3.5.7";

        public static Bitmap Download(MapType mapType, Point p, int tileSize, int zoom)
        {
            string[] urls = null;

            if (mapType == MapType.GoogleLabels)
            {
                urls = new string[] {
                     GetImageryUrl(MapType.GoogleSatellite, p, zoom),
                     GetImageryUrl(mapType, p, zoom)
                };
            }
            else
            {
                urls = new string[] {
                     GetImageryUrl( mapType, p, zoom)
                };
            }

            HttpWebResponse response = null;
            HttpWebRequest request = null; 
            try
            {
                Bitmap bmp = new Bitmap(tileSize, tileSize);
                using (Graphics gx = Graphics.FromImage(bmp))
                {
                    foreach (string url in urls)
                    {
                        request = (HttpWebRequest)WebRequest.Create(url);
                        request.UserAgent = UserAgent;
                        if (response != null)
                            response.Close();

                        response = (HttpWebResponse)request.GetResponse();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            gx.DrawImage(new Bitmap(response.GetResponseStream()), 0, 0);
                        }
                    }
//#if DEBUG
//                    gx.DrawString(string.Format("{0},{1}", p.X, p.Y),
//                        new Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Red), 0, 0);
//#endif
                }
                return bmp;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString(), "error");
                return null;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }

    }
}