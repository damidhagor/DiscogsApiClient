using System.Net.Http.Headers;
using System.Text.Json;

namespace DiscogsApiClient.Tests.Fixtures.Recording;

public static class RecordingHelper
{
    public static string RecordingPath => Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Fixtures", "Recording", "Recordings"));

    public static JsonSerializerOptions JsonOptions { get; }
        = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };

    private static HashSet<string> IgnoredHeaders { get; } = ["traceparent"];
    private static HashSet<string> SensitiveHeaders { get; } = ["Authorization"];

    public static string GetRecordingKey()
    {
        ArgumentNullException.ThrowIfNull(TestContext.Current);

        var testDetails = TestContext.Current.Metadata.TestDetails;

        var testClassName = testDetails.ClassType.Name;
        var testName = testDetails.TestName;
        var testNamespace = testDetails.ClassType.Namespace?.Replace("DiscogsApiClient.Tests.", "");

        return string.IsNullOrEmpty(testNamespace)
            ? $"{testClassName}.{testName}"
            : $"{testNamespace}.{testClassName}.{testName}";
    }

    public static Dictionary<string, string[]> GetCleanedUpHeaders(this HttpHeaders headers)
        => headers.Where(h => !IgnoredHeaders.Contains(h.Key))
            .ToDictionary(
                h => h.Key,
                h => SensitiveHeaders.Contains(h.Key)
                    ? ["****"]
                    : h.Value.ToArray());
}
