using DiscogsApiClient.SourceGenerator.Diagnostics;
using DiscogsApiClient.SourceGenerator.Shared;

namespace DiscogsApiClient.SourceGenerator.Tests.Shared;

public sealed class GeneratorResultTests
{
    private sealed record TestModel(string Name);

    [Test]
    public async Task ShouldReturnTrueAndModel_WhenSuccessResult()
    {
        var model = new TestModel("SuccessModel");
        var result = GeneratorResult<TestModel>.Success(model);

        var hasModel = result.TryGetModel(out var extracted);

        await Assert.That(hasModel).IsTrue();
        await Assert.That(extracted).IsEqualTo(model);
    }

    [Test]
    public async Task ShouldReturnFalseAndDefault_WhenFailureResult()
    {
        var result = GeneratorResult<TestModel>.Failure([]);

        var hasModel = result.TryGetModel(out var extracted);

        await Assert.That(hasModel).IsFalse();
        await Assert.That(extracted).IsNull();
    }

    [Test]
    public async Task ShouldCreateSuccessResult_WithNoDiagnostics()
    {
        var model = new TestModel("SuccessModel");
        var result = GeneratorResult<TestModel>.Success(model);

        await Assert.That(result.Model).IsEqualTo(model);
        await Assert.That(result.Diagnostics).IsEmpty();
    }

    [Test]
    public async Task ShouldCreateSuccessResult_WithDiagnostics()
    {
        var model = new TestModel("SuccessModel");
        var descriptor = new DiagnosticDescriptor("ID", "Title", "Message", "Category", DiagnosticSeverity.Warning, true);
        var diag = new DiagnosticInfo(descriptor, DiagnosticLocation.From(Location.None));
        var result = GeneratorResult<TestModel>.Success(model, [diag]);

        await Assert.That(result.Model).IsEqualTo(model);
        await Assert.That(result.Diagnostics.Length).IsEqualTo(1);
        await Assert.That(result.Diagnostics[0]).IsEqualTo(diag);
    }

    [Test]
    public async Task ShouldCreateFailureResult_WithDiagnostics()
    {
        var descriptor = new DiagnosticDescriptor("ID", "Title", "Message", "Category", DiagnosticSeverity.Error, true);
        var diag = new DiagnosticInfo(descriptor, DiagnosticLocation.From(Location.None));
        var result = GeneratorResult<TestModel>.Failure([diag]);

        await Assert.That(result.Model).IsNull();
        await Assert.That(result.Diagnostics.Length).IsEqualTo(1);
        await Assert.That(result.Diagnostics[0]).IsEqualTo(diag);
    }
}
