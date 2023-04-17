using Microsoft.Data.SqlClient;
using SqlReleaseManager.Core.Abstractions;
using SqlReleaseManager.Core.Commands;
using SqlReleaseManager.Core.Domain;

namespace SqlReleaseManager.Core.Services;

public class SqlServerInstanceService : ISqlServerService
{
    private readonly ISqlServerRepository _repository;

    public SqlServerInstanceService(ISqlServerRepository repository)
    {
        _repository = repository;
    }

    public async Task Create(CreateOrUpdateSqlServerInstance instance)
    {
        // make sure the connection string is not pointing to a database

        var connectionStringBuilder = new SqlConnectionStringBuilder(instance.ConnectionString)
        {
            InitialCatalog = null
        };

        instance.ConnectionString = connectionStringBuilder.ConnectionString;


        var id = await _repository.Add(instance);


        await UpdateDatabases(id);
    }

    public async Task UpdateDatabases(int id)
    {
        var instance = await _repository.GetById(id);

        var server = new SqlServer(instance.ConnectionString);
        var databases = await server.GetDatabases();
        await _repository.UpdateDatabases(instance.Id, databases);
    }


    public async Task<bool> CanConnectToInstance(int id )
    {
        var instance = await _repository.GetById(id);

        var server = new SqlServer(instance.ConnectionString);

        return await server.CanConnect();
    }
}