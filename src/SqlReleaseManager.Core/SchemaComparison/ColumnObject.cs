using Microsoft.SqlServer.Dac.Model;

namespace SqlReleaseManager.Core.SchemaComparison;

public record ColumnObject : SchemaObject
{
    public string DataType { get; set; }
    public bool IsNullable { get; set; }
    public bool IsIdentity { get; set; }
    public bool IsHidden { get; set; }

    public static ColumnObject FromTSqlObject(TSqlObject o)
    {
        var obj = FromTSqlObject<ColumnObject>(o) with
        {
            IsNullable = o.GetProperty<bool>(Column.Nullable),
            IsIdentity = o.GetProperty<bool>(Column.IsIdentity),
            IsHidden = o.GetProperty<bool>(Column.IsHidden),
        };
        var typeSpecifier = o.GetReferenced(Column.DataType).FirstOrDefault();
        if (typeSpecifier != null)
        {
            obj.DataType = typeSpecifier.Name.ToString();
        }


        return obj;
    }
}