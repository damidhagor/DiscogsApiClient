// Polyfill for System.Runtime.CompilerServices.CollectionBuilderAttribute
// Required for collection expression support on custom types when targeting netstandard2.0.
// The compiler uses this attribute to enable syntax like: EquatableArray<string> args = ["a", "b"];

#pragma warning disable IDE0130 // Namespace does not match folder structure: this is a required polyfill namespace
namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false)]
internal sealed class CollectionBuilderAttribute(Type builderType, string methodName) : Attribute
{
    public Type BuilderType { get; } = builderType;

    public string MethodName { get; } = methodName;
}
