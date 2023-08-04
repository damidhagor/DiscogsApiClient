namespace DiscogsApiClient.ApiClientGenerator.Models;

internal sealed class ApiMethodReturnType
{
    public string FullName { get; private set; }

    public bool IsVoid { get; private set; }

    public bool HasResult { get; private set; }

    public bool IsTask { get; private set; }

    public string TaskResultTypeFullName { get; private set; }

    private ApiMethodReturnType(string fullName, bool isVoid, bool hasResult, bool isTask, string taskResultTypeFullName)
    {
        FullName = fullName;
        IsVoid = isVoid;
        HasResult = hasResult;
        IsTask = isTask;
        TaskResultTypeFullName = taskResultTypeFullName;
    }

    public static ApiMethodReturnType CreateVoid()
        => new("void", true, false, false, "");

    public static ApiMethodReturnType CreateNoTask(string fullName)
        => new(fullName, false, true, false, "");

    public static ApiMethodReturnType CreateTask(string fullName)
        => new(fullName, false, false, true, "");

    public static ApiMethodReturnType CreateTaskWithResult(string fullName, string resultFullName)
        => new(fullName, false, true, true, resultFullName);
}
