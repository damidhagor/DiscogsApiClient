using System.Text;
using System.Text.Json;

namespace DiscogsApiClient.Serialization;

/// <summary>
/// A <see cref="JsonNamingPolicy"/> to convert the C# property names from PascalCase to snake_case which the Discogs Api uses.
/// The converted names are cached.
/// Example: ResourceUrl => resource_url
/// </summary>
internal sealed class DiscogsJsonNamingPolicy : JsonNamingPolicy
{
    private static readonly Dictionary<string, string> _cachedNames = new();

    public override string ConvertName(string name)
    {
        if (name.Length == 0)
            return "";

        if (_cachedNames.TryGetValue(name, out var cachedName))
            return cachedName;

        StringBuilder stringBuilder = new();

        stringBuilder.Append(Char.ToLower(name[0]));

        foreach (var c in name.Skip(1))
        {
            if (Char.IsUpper(c))
            {
                stringBuilder.Append('_');
                stringBuilder.Append(Char.ToLower(c));
            }
            else
            {
                stringBuilder.Append(c);
            }
        }

        string convertedName = stringBuilder.ToString();
        _cachedNames[name] = convertedName;
        return convertedName;
    }
}
