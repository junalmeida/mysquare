﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("MySquare")]
[assembly: AssemblyDescription("A FourSquare client.")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("133b1aab-6031-4c1f-8961-c9252a6a6773")]

#if DEBUG
#if !TESTING_2010
[assembly: InternalsVisibleTo("MySquare.Test")]
#else
[assembly: InternalsVisibleTo("MySquare.2010.Test")]
#endif
#endif
// Below attribute is to suppress FxCop warning "CA2232 : Microsoft.Usage : Add STAThreadAttribute to assembly"
// as Device app does not support STA thread.
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2232:MarkWindowsFormsEntryPointsWithStaThread")]
