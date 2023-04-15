namespace SqlReleaseManager.Core.Commands;

public record CreateOrUpdateSqlServerInstance
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public string ConnectionString { get; set; }
}