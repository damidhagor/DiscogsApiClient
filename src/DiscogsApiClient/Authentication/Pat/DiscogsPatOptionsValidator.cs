using Microsoft.Extensions.Options;

namespace DiscogsApiClient.Authentication.Pat;

internal sealed class DiscogsPatOptionsValidator : IValidateOptions<DiscogsPatOptions>
{
    public ValidateOptionsResult Validate(string? name, DiscogsPatOptions options)
    {
        if (options.Token is not null && string.IsNullOrWhiteSpace(options.Token))
        {
            return ValidateOptionsResult.Fail($"{nameof(DiscogsPatOptions.Token)} must not be empty or whitespace when provided.");
        }

        return ValidateOptionsResult.Success;
    }
}
