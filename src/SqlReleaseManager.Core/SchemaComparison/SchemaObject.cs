using Microsoft.SqlServer.Dac.Model;

namespace SqlReleaseManager.Core.SchemaComparison;

public record SchemaObject
{
    public string Name { get; set; }
    public string? Definition { get; set; }

    public static T FromTSqlObject<T>(TSqlObject tSqlObject) where T : SchemaObject, new()
    {
        var obj = new T
        {
            Name = tSqlObject.Name.ToString()
        };


        if (tSqlObject.TryGetScript(out var script))
        {
            obj.Definition = script;
        }

        return obj;
    }
}