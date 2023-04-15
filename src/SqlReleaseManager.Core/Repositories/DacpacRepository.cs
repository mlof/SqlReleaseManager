using Microsoft.SqlServer.Dac;
using SqlReleaseManager.Core.Abstractions;
using SqlReleaseManager.Core.Models;

namespace SqlReleaseManager.Core.Repositories;

public class DacpacRepository : IDacpacRepository
{
    private readonly DirectoryInfo _storageDirectory;

    public DacpacRepository() 
    {
        _storageDirectory = new DirectoryInfo(ApplicationPaths.DacpacPath);
        if (!_storageDirectory.Exists)
        {
            _storageDirectory.Create();
        }
        
    }

    public DacpacRepository(string path) 
    {
        _storageDirectory = new DirectoryInfo(path);
        if (!_storageDirectory.Exists)
        {
            _storageDirectory.Create();
        }
    }


    public async Task Create(CreateOrUpdateDacpac dacpac)
    {
        var directoryPath = Path.Join(_storageDirectory.FullName, dacpac.Name);

        var filePath = Path.Join(directoryPath, $"{dacpac.Name}.dacpac");

        if (File.Exists(filePath))
        {
            throw new InvalidOperationException($"Dacpac with name {dacpac.Name} already exists.");
        }

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        await using var fileStream = File.Create(filePath);

        await dacpac.Stream.CopyToAsync(fileStream);
    }

    public async Task<IEnumerable<DacpacDto>> List()
    {
        var directories = _storageDirectory.GetDirectories();
        var dacpacs = new List<DacpacDto>();
        foreach (var directory in directories)
        {
            if (!File.Exists(Path.Join(directory.FullName, $"{directory.Name}.dacpac")))
            {
                continue;
            }

            var file = new FileInfo(Path.Join(directory.FullName, $"{directory.Name}.dacpac"));

            var dacpac = MapFileToDto(file);

            dacpacs.Add(dacpac);
        }

        return dacpacs;
    }

    public async Task Delete(string name)
    {
        var directoryPath = Path.Join(_storageDirectory.FullName, name);
        if (!Directory.Exists(directoryPath))
        {
            throw new InvalidOperationException($"Dacpac with name {name} does not exist.");
        }

        Archive(name, directoryPath);
    }

    public DacPackage Retrieve(string name)
    {
        var directoryPath = Path.Join(_storageDirectory.FullName, name);

        if (!Directory.Exists(directoryPath))
        {
            throw new InvalidOperationException($"Dacpac with name {name} does not exist.");
        }


        var package = DacPackage.Load(Path.Join(_storageDirectory.FullName, name, $"{name}.dacpac"));


        return package;
    }

    public Task Update(string name, CreateOrUpdateDacpac dacpac)
    {
        // check if dacpac exists

        var directoryPath = Path.Join(_storageDirectory.FullName, name);

        if (!Directory.Exists(directoryPath))
        {
            throw new InvalidOperationException($"Dacpac with name {name} does not exist.");
        }

        Archive(name, directoryPath);

        // create new file

        return Create(dacpac);
    }

    private static void Archive(string name, string directoryPath)
    {
        var filePath = Path.Join(directoryPath, $"{name}.dacpac");

        // if file exists, rename it with the date it was created

        if (File.Exists(filePath))
        {
            var fileInfo = new FileInfo(filePath);
            var newFileName = $"{name}_{fileInfo.CreationTimeUtc:yyyyMMddHHmmss}.dacpac";
            var newFilePath = Path.Join(directoryPath, newFileName);
            File.Move(filePath, newFilePath);
        }
    }

    private DacpacDto MapFileToDto(FileInfo file)
    {
        var dacpac = new DacpacDto
        {
            Name = Path.GetFileNameWithoutExtension(file.Name),
            Filepath = file.FullName,
            Created = file.CreationTimeUtc,
            LastModified = file.LastWriteTimeUtc

        };


        return dacpac;
    }

    public void Dispose()
    {
    }
}