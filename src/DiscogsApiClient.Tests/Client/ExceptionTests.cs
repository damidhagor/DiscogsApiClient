namespace DiscogsApiClient.Tests.Client;

public sealed class ExceptionTests
{
    [Test]
    public async Task DiscogsException_ShouldInitializeCorrectly()
    {
        var message = "Error";
        var inner = new Exception("Inner");
        
        var ex = new DiscogsException(message, inner);
        await Assert.That(ex.Message).IsEqualTo(message);
        await Assert.That(ex.InnerException).IsEqualTo(inner);

        var exDefault = new DiscogsException();
        await Assert.That(exDefault.Message).IsNotNullOrEmpty();
        await Assert.That(exDefault.InnerException).IsNull();
    }

    [Test]
    public async Task AuthenticationFailedDiscogsException_ShouldInitializeCorrectly()
    {
        var message = "Auth failed";
        var inner = new Exception("Inner");
        
        var ex = new AuthenticationFailedDiscogsException(message, inner);
        await Assert.That(ex.Message).IsEqualTo(message);
        await Assert.That(ex.InnerException).IsEqualTo(inner);

        var exDefault = new AuthenticationFailedDiscogsException();
        await Assert.That(exDefault.Message).IsNotNullOrEmpty();
        await Assert.That(exDefault.InnerException).IsNull();
    }

    [Test]
    public async Task RateLimitExceededDiscogsException_ShouldInitializeCorrectly()
    {
        var message = "Rate limit hit";
        var inner = new Exception("Inner");
        
        var ex = new RateLimitExceededDiscogsException(message, inner);
        await Assert.That(ex.Message).IsEqualTo(message);
        await Assert.That(ex.InnerException).IsEqualTo(inner);

        var exDefault = new RateLimitExceededDiscogsException();
        await Assert.That(exDefault.Message).IsNotNullOrEmpty();
        await Assert.That(exDefault.InnerException).IsNull();
    }

    [Test]
    public async Task ResourceNotFoundDiscogsException_ShouldInitializeCorrectly()
    {
        var message = "Not found";
        var inner = new Exception("Inner");
        
        var ex = new ResourceNotFoundDiscogsException(message, inner);
        await Assert.That(ex.Message).IsEqualTo(message);
        await Assert.That(ex.InnerException).IsEqualTo(inner);

        var exDefault = new ResourceNotFoundDiscogsException();
        await Assert.That(exDefault.Message).IsNotNullOrEmpty();
        await Assert.That(exDefault.InnerException).IsNull();
    }

    [Test]
    public async Task SerializationDiscogsException_ShouldInitializeCorrectly()
    {
        var message = "Serialization failed";
        var inner = new Exception("Inner");
        
        var ex = new SerializationDiscogsException(message, inner);
        await Assert.That(ex.Message).IsEqualTo(message);
        await Assert.That(ex.InnerException).IsEqualTo(inner);

        var exDefault = new SerializationDiscogsException();
        await Assert.That(exDefault.Message).IsNotNullOrEmpty();
        await Assert.That(exDefault.InnerException).IsNull();
    }

    [Test]
    public async Task UnauthenticatedDiscogsException_ShouldInitializeCorrectly()
    {
        var message = "Unauthenticated";
        var inner = new Exception("Inner");
        
        var ex = new UnauthenticatedDiscogsException(message, inner);
        await Assert.That(ex.Message).IsEqualTo(message);
        await Assert.That(ex.InnerException).IsEqualTo(inner);

        var exDefault = new UnauthenticatedDiscogsException();
        await Assert.That(exDefault.Message).IsNotNullOrEmpty();
        await Assert.That(exDefault.InnerException).IsNull();
    }
}
