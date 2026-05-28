using DiscogsApiClient.SourceGenerator.Diagnostics;

namespace DiscogsApiClient.SourceGenerator.Shared;

internal sealed record GeneratorResult<T>(T? Model, EquatableArray<DiagnosticInfo> Diagnostics)
    where T : IEquatable<T>
{
    public bool TryGetModel(out T model)
    {
        if (Model is not null)
        {
            model = Model;
            return true;
        }

        model = default!;
        return false;
    }

    public static GeneratorResult<T> Success(T model) => new(model, []);

    public static GeneratorResult<T> Success(T model, EquatableArray<DiagnosticInfo> diagnostics) => new(model, diagnostics);

    public static GeneratorResult<T> Failure(EquatableArray<DiagnosticInfo> diagnostics) => new(default, diagnostics);
}
