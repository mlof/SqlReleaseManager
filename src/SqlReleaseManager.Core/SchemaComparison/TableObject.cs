using Microsoft.SqlServer.Dac.Model;

namespace SqlReleaseManager.Core.SchemaComparison;

public record TableObject : SchemaObject
{
    public IEnumerable<ColumnObject> Columns { get; set; } = new List<ColumnObject>();

    public IEnumerable<ConstraintObject> Constraints { get; set; } = new List<ConstraintObject>();

    public static TableObject FromTSqlObject(TSqlObject tSqlObject)
    {
        return FromTSqlObject<TableObject>(tSqlObject);
    }
}