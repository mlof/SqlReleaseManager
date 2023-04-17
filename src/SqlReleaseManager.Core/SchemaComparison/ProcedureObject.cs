using Microsoft.SqlServer.Dac.Model;

namespace SqlReleaseManager.Core.SchemaComparison;

public record ProcedureObject : SchemaObject
{
    public IEnumerable<ParameterObject> Parameters { get; set; } = new List<ParameterObject>();

    public static ProcedureObject FromTSqlObject(TSqlObject tSqlObject)
    {
        return FromTSqlObject<ProcedureObject>(tSqlObject);
    }
}