namespace SqlReleaseManager.Core.Models;

public record DacpacDto
{
    public string Name { get; set; }
    public string Filepath { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
}

public record CreateOrUpdateDacpac
{
    public CreateOrUpdateDacpac(string name, Stream stream)
    {
        Name = name;
        Stream = stream;
    }

    public CreateOrUpdateDacpac(string name, string filePath)
    {
        Name = name;
        Stream = File.OpenRead(filePath);
    }

    public string Name { get; set; }
    public Stream Stream { get; set; }
}