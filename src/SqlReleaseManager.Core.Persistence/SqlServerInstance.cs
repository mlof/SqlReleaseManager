namespace SqlReleaseManager.Core.Persistence;

public class SqlServerInstance
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public string ConnectionString { get; set; }


    public List<DatabaseInstance> Databases { get; set; } = new List<DatabaseInstance>();
}