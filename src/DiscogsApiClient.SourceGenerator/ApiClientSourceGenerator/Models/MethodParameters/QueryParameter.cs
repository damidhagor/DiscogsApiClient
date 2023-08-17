namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

internal sealed class QueryParameter
{
    public string ParameterName { get; private set; }

    public string PropertyName { get; private set; }

    public string PropertyType { get; private set; }

    public QueryParameterType ParameterType { get; private set; }

    public bool IsNullable { get; private set; }

    public List<(string MemberName, string DisplayName)>? EnumValues { get; private set; }

    public QueryParameter(
        string parameterName,
        string propertyName,
        string propertyType,
        QueryParameterType parameterType,
        bool isNullable,
        List<(string MemberName, string DisplayName)>? enumValues = null)
    {
        ParameterName = parameterName;
        PropertyName = propertyName;
        PropertyType = propertyType;
        ParameterType = parameterType;
        IsNullable = isNullable;
        EnumValues = enumValues;
    }
}
