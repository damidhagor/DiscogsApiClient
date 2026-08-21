// Polyfill for System.Runtime.CompilerServices.IsExternalInit
// Required for records and init-only properties when targeting netstandard2.0.
// The compiler emits references to this type for init-only setters, but it only
// exists in the runtime starting from .NET 5. This polyfill makes the compiler happy.

#pragma warning disable IDE0130 // Namespace does not match folder structure: this is a required polyfill namespace
namespace System.Runtime.CompilerServices;

internal static class IsExternalInit;
