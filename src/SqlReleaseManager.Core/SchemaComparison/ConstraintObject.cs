namespace SqlReleaseManager.Core.SchemaComparison;

public record ConstraintObject : SchemaObject
{
    public ConstraintType Type { get; set; }
}