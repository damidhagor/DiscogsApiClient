using Microsoft.Extensions.Options;

namespace DiscogsApiClient;

internal sealed class DiscogsApiClientOptionsValidator : IValidateOptions<DiscogsApiClientOptions>
{
    public ValidateOptionsResult Validate(string? name, DiscogsApiClientOptions options)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.BaseUrl))
        {
            errors.Add($"{nameof(DiscogsApiClientOptions.BaseUrl)} is required.");
        }
        else if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _))
        {
            errors.Add($"{nameof(DiscogsApiClientOptions.BaseUrl)} must be a valid URL.");
        }

        if (string.IsNullOrWhiteSpace(options.UserAgent))
        {
            errors.Add($"{nameof(DiscogsApiClientOptions.UserAgent)} is required.");
        }

        var hasConsumerKey = !string.IsNullOrWhiteSpace(options.ConsumerKey);
        var hasConsumerSecret = !string.IsNullOrWhiteSpace(options.ConsumerSecret);
        var hasVerifierCallbackUrl = !string.IsNullOrWhiteSpace(options.VerifierCallbackUrl);

        if ((hasConsumerKey || hasConsumerSecret || hasVerifierCallbackUrl)
            && !(hasConsumerKey && hasConsumerSecret && hasVerifierCallbackUrl))
        {
            errors.Add($"{nameof(DiscogsApiClientOptions.ConsumerKey)}, {nameof(DiscogsApiClientOptions.ConsumerSecret)} and {nameof(DiscogsApiClientOptions.VerifierCallbackUrl)} must all be provided to use OAuth authentication.");
        }
        else if (hasVerifierCallbackUrl && !Uri.TryCreate(options.VerifierCallbackUrl, UriKind.Absolute, out _))
        {
            errors.Add($"{nameof(DiscogsApiClientOptions.VerifierCallbackUrl)} must be a valid URL.");
        }

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}
