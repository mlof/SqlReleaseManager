namespace SqlReleaseManager.Web;

public class DatabaseOptions
{
    public const string Database = "Database";
    public bool RunMigrationsOnStartup { get; set; }
}