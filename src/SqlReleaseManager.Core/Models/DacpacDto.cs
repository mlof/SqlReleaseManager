namespace SqlReleaseManager.Core.Models;

public record DacpacDto
{
    public string Name { get; set; }
    public string Filepath { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
}