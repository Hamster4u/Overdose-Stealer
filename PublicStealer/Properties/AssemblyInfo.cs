using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

// Descripción general del ensamblado. Haz que parezca una aplicación real o un componente.
[assembly: AssemblyTitle("Microsoft OneDrive Sync Engine")]
[assembly: AssemblyDescription("Provides core synchronization services for Microsoft OneDrive, ensuring your files are always up-to-date across all your devices and the cloud.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft Corporation")] // Empresa legítima o una que suene creíble
[assembly: AssemblyProduct("Microsoft OneDrive")] // Nombre de un producto real o verosímil
[assembly: AssemblyCopyright("© 2025 Microsoft Corporation. All rights reserved.")] // Año actual y nombre de la empresa
[assembly: AssemblyTrademark("Microsoft® and OneDrive® are registered trademarks of Microsoft Corporation.")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components. If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
// Puedes generar un nuevo GUID si lo deseas, o usar este.
[assembly: Guid("8f3a2c1d-6e7f-4b9a-8c0d-1a2b3c4d5e6f")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

// Versión del ensamblado. Usa un esquema de versionado realista (ej. 24.1.0.1234 para algo de 2024)
// Version de "Microsoft OneDrive": suelen ser números altos y con patrón específico.
[assembly: AssemblyVersion("25.1.0.1478")] // Año actual (25.x.x.x)
[assembly: AssemblyFileVersion("25.1.0.1478")] // Normalmente coincide con AssemblyVersion
[assembly: AssemblyInformationalVersion("25.1.0.1478-Release")] // Versión para mostrar al usuario, a menudo con un sufijo.

// Esto es útil para los ensamblados de seguridad, a menudo para indicar compatibilidad con CLS (Common Language Specification)
[assembly: CLSCompliant(true)]

// Para .NET Framework, si quieres firmar tu ensamblado con un nombre fuerte
// [assembly: AssemblyKeyFile("MyKeyPair.snk")]
// [assembly: AssemblyDelaySign(false)]
