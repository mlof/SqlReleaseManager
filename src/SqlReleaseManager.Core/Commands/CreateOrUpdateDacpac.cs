namespace SqlReleaseManager.Core.Commands;

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