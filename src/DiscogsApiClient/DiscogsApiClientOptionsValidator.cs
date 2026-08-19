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

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}
