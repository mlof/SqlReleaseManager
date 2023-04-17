namespace SqlReleaseManager.Web.Models;

public record ServerViewModel
{
    public int Id { get; set; }

    public string ServerName { get; set; }
    public string Version { get; set; }
    public string Edition { get; set; }
    public string Release { get; set; }

    public int Jobs { get; set; }
    public string Uptime { get; set; }

    public List<DatabaseViewModel> Databases { get; set; } = new List<DatabaseViewModel>();
}