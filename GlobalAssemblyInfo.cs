using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("Rising Mobility")]
[assembly: AssemblyProduct("MySquare")]
[assembly: AssemblyCopyright("Rising Mobility")]
[assembly: AssemblyTrademark("Rising Mobility ™ 2010")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("2.0.0.*")]
#if ALPHA
[assembly: AssemblyConfiguration("alpha")]
#elif DEBUG
[assembly: AssemblyConfiguration("debug")]
#else
[assembly: AssemblyConfiguration("")]
#endif
