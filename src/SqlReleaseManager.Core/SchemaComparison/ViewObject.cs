using Microsoft.SqlServer.Dac.Model;

namespace SqlReleaseManager.Core.SchemaComparison;

public record ViewObject : SchemaObject
{
    public IEnumerable<ColumnObject> Columns { get; set; } = new List<ColumnObject>();

    public static ViewObject FromTSqlObject(TSqlObject tSqlObject)
    {
        return FromTSqlObject<ViewObject>(tSqlObject);
    }
}