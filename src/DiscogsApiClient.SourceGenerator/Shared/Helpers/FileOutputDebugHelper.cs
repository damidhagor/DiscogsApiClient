namespace DiscogsApiClient.SourceGenerator.Shared.Helpers;

internal static class FileOutputDebugHelper
{
#pragma warning disable RS1035 // Do not use APIs banned for analyzers
    public static StreamWriter GetOutputStreamWriter(string filename, bool append)
    {
        var fullFilename = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                filename);

        return new StreamWriter(fullFilename, append);
    }
#pragma warning restore RS1035 // Do not use APIs banned for analyzers
}
