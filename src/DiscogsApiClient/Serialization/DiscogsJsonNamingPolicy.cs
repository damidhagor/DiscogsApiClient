using System.Text.Json;
using System.Text;

namespace DiscogsApiClient.Serialization;

internal class DiscogsJsonNamingPolicy : JsonNamingPolicy
{
    private static readonly Dictionary<string, string> _cachedNames = new Dictionary<string, string>();

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
                stringBuilder.Append("_");
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
