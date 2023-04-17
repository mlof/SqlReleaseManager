namespace SqlReleaseManager.Core.SchemaComparison;

public record Schema
{
    public string DatabaseName { get; set; } = string.Empty;

    public List<TableObject> Tables { get; set; } = new List<TableObject>();
    public List<ViewObject> Views { get; set; } = new List<ViewObject>();
    public List<ProcedureObject> Procedures { get; set; } = new List<ProcedureObject>();
}