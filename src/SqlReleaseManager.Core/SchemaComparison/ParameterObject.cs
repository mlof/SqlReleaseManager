using Microsoft.SqlServer.Dac.Model;

namespace SqlReleaseManager.Core.SchemaComparison;

public record ParameterObject : SchemaObject
{
    public string DataType { get; set; }
    public bool IsNullable { get; set; }
    public bool IsOutput { get; set; }

    public static ParameterObject FromTSqlObject(TSqlObject tSqlObject)
    {
        return FromTSqlObject<ParameterObject>(tSqlObject) with
        {
            IsNullable = tSqlObject.GetProperty<bool>(Parameter.IsNullable),
            IsOutput = tSqlObject.GetProperty<bool>(Parameter.IsOutput)
        };
    }
}