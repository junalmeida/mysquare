using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("Rising Mobility")]
[assembly: AssemblyProduct("MySquare")]
[assembly: AssemblyCopyright("Rising Mobility")]
[assembly: AssemblyTrademark("Rising Mobility ™ 2010")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("1.3.*")]
#if ALPHA
[assembly: AssemblyConfiguration("alpha3")]
#elif DEBUG
[assembly: AssemblyConfiguration("debug")]
#else
[assembly: AssemblyConfiguration("beta3")]
#endif
