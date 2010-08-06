//#define TEST

using System;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using MySquare.FourSquare;
using Tenor.Mobile.Location;
using System.Threading;
using MySquare.Controller;
using System.Drawing;

[assembly: System.Reflection.Obfuscation(Feature = "Apply to MySquare.FourSquare.*: all", Exclude = true, ApplyToMembers = true)]
namespace MySquare
{
    public static class Program
    {
        internal static WorldPosition Location
        { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            using (UI.Main mainForm = new UI.Main())
            {
#if !DEBUG
                try
                {
#endif
                    Location = new WorldPosition(true, true, 15000);
                    Location.PollHit += new EventHandler(Location_PollHit);
                    Location.Poll();
                    Application.Run(mainForm);
#if !DEBUG
                }
                catch (ObjectDisposedException)
                {
                }
                catch (Exception ex)
                {
                    Service.Log.RegisterLog(ex);
                    MessageBox.Show("Unknown error.\r\n" + ex.Message, "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    Terminate();
                }
                finally
                {
                    Location.Dispose();
                }
                try
                {
                    mainForm.Close();
                    mainForm.Dispose();
                }
                catch (ObjectDisposedException) { }
#endif
            }
            Application.Exit();
        }

        static void Location_PollHit(object sender, EventArgs e)
        {
            if (Location != null)
            {
                if (Location.FixType == FixType.GsmNetwork)
                    Location.UseNetwork = false;
                if (!KeepGpsOpened && Location.FixType == FixType.Gps)
                    Location.UseGps = false;
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e != null && e.ExceptionObject is Exception)
                Service.Log.RegisterLog(e.ExceptionObject as Exception);
        }

        internal static bool KeepGpsOpened { get; set; }

        internal static void Terminate()
        {
            try
            {
                BaseController.Terminate();
            }
            catch { }
        }

        internal static void DrawSeparator(Graphics g, Rectangle bounds, System.Drawing.Color color)
        {
            Rectangle rect2 = new Rectangle(
               bounds.X, bounds.Y, bounds.Width / 3, 1);
            Tenor.Mobile.Drawing.GradientFill.Fill(g, rect2, color, Color.LightGray, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
            rect2.X += rect2.Width;
            Tenor.Mobile.Drawing.GradientFill.Fill(g, rect2, Color.LightGray, Color.LightGray, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
            rect2.X += rect2.Width;
            Tenor.Mobile.Drawing.GradientFill.Fill(g, rect2, Color.LightGray, color, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
        }
    }
}

#if TESTING
namespace Microsoft.WindowsCE.Forms
{
    public class InputPanel : System.ComponentModel.Component
    {
        public InputPanel()
            : base()
        { }

        public InputPanel(System.ComponentModel.IContainer container)
            : this()
        { }

        public bool Enabled { get; set; }
        public System.Drawing.Rectangle Bounds
        {
            get
            {
                return System.Drawing.Rectangle.Empty;
            }
        }
        public event EventHandler EnabledChanged;
    }
}
#endif


// Definition of custom attributes for declarative obfuscation.
// This file is only necessary for .NET Compact Framework and Silverlight projects.


namespace System.Reflection
{
    /// <summary>
    /// Instructs obfuscation tools to use their standard obfuscation rules for the appropriate assembly type.
    /// </summary>
    [ComVisible(true), AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    sealed class ObfuscateAssemblyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObfuscateAssemblyAttribute"/> class,
        /// specifying whether the assembly to be obfuscated is public or private.
        /// </summary>
        /// <param name="assemblyIsPrivate"><c>true</c> if the assembly is used within the scope of one application; otherwise, <c>false</c>.</param>
        public ObfuscateAssemblyAttribute(bool assemblyIsPrivate)
        {
            m_assemblyIsPrivate = assemblyIsPrivate;
            m_stripAfterObfuscation = true;
        }

        bool m_assemblyIsPrivate;

        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value indicating whether the assembly was marked private.
        /// </summary>
        /// <value>
        /// <c>true</c> if the assembly was marked private; otherwise, <c>false</c>.
        /// </value>
        public bool AssemblyIsPrivate
        {
            get { return m_assemblyIsPrivate; }
        }

        bool m_stripAfterObfuscation;

        /// <summary>
        /// Gets or sets a <see cref="System.Boolean"/> value indicating whether the obfuscation tool should remove the attribute after processing.
        /// </summary>
        /// <value>
        /// <c>true</c> if the obfuscation tool should remove the attribute after processing; otherwise, <c>false</c>.
        /// The default value for this property is <c>true</c>.
        /// </value>
        public bool StripAfterObfuscation
        {
            get { return m_stripAfterObfuscation; }
            set { m_stripAfterObfuscation = value; }
        }
    }

    /// <summary>
    /// Instructs obfuscation tools to take the specified actions for an assembly, type, or member.
    /// </summary>
    [ComVisible(true), AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Parameter | AttributeTargets.Interface | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    sealed class ObfuscationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObfuscationAttribute"/> class.
        /// </summary>
        public ObfuscationAttribute()
        {
            this.m_applyToMembers = true;
            this.m_exclude = true;
            this.m_feature = "all";
            this.m_stripAfterObfuscation = true;
        }

        bool m_applyToMembers;

        /// <summary>
        /// Gets or sets a <see cref="System.Boolean"/> value indicating whether the attribute of a type is to apply to the members of the type.
        /// </summary>
        /// <value>
        /// <c>true</c> if the attribute is to apply to the members of the type; otherwise, <c>false</c>. The default is <c>true</c>.
        /// </value>
        public bool ApplyToMembers
        {
            get { return m_applyToMembers; }
            set { m_applyToMembers = value; }
        }

        bool m_exclude;

        /// <summary>
        /// Gets or sets a <see cref="System.Boolean"/> value indicating whether the obfuscation tool should exclude the type or member from obfuscation.
        /// </summary>
        /// <value>
        /// <c>true</c> if the type or member to which this attribute is applied should be excluded from obfuscation; otherwise, <c>false</c>.
        /// The default is <c>true</c>.
        /// </value>
        public bool Exclude
        {
            get { return m_exclude; }
            set { m_exclude = value; }
        }

        string m_feature;

        /// <summary>
        /// Gets or sets a string value that is recognized by the obfuscation tool, and which specifies processing options.
        /// </summary>
        /// <value>
        /// A string value that is recognized by the obfuscation tool, and which specifies processing options. The default is "all".
        /// </value>
        public string Feature
        {
            get { return m_feature; }
            set { m_feature = value; }
        }

        bool m_stripAfterObfuscation;

        /// <summary>
        /// Gets or sets a <see cref="System.Boolean"/> value indicating whether the obfuscation tool should remove the attribute after processing.
        /// </summary>
        /// <value>
        /// <c>true</c> if the obfuscation tool should remove the attribute after processing; otherwise, <c>false</c>.
        /// The default value for this property is <c>true</c>.
        /// </value>
        public bool StripAfterObfuscation
        {
            get { return m_stripAfterObfuscation; }
            set { m_stripAfterObfuscation = value; }
        }
    }
}
