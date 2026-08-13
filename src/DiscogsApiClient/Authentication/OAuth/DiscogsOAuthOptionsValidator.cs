using Microsoft.Extensions.Options;

namespace DiscogsApiClient.Authentication.OAuth;

internal sealed class DiscogsOAuthOptionsValidator : IValidateOptions<DiscogsOAuthOptions>
{
    public ValidateOptionsResult Validate(string? name, DiscogsOAuthOptions options)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.ConsumerKey))
        {
            errors.Add($"{nameof(DiscogsOAuthOptions.ConsumerKey)} is required.");
        }

        if (string.IsNullOrWhiteSpace(options.ConsumerSecret))
        {
            errors.Add($"{nameof(DiscogsOAuthOptions.ConsumerSecret)} is required.");
        }

        if (options.VerifierCallbackUrl is not null
            && !Uri.TryCreate(options.VerifierCallbackUrl, UriKind.Absolute, out _))
        {
            errors.Add($"{nameof(DiscogsOAuthOptions.VerifierCallbackUrl)} must be a valid URL.");
        }

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}
