namespace SqlReleaseManager.Core;

public static class ApplicationPaths
{
    public static string DacpacPath => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SqlReleaseManager", "Dacpacs");
}