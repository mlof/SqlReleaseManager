using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Dac;
using SqlReleaseManager.Core.Abstractions;
using SqlReleaseManager.Core.Commands;
using SqlReleaseManager.Core.Domain;
using SqlReleaseManager.Core.Models;
using SqlReleaseManager.Core.Persistence;

namespace SqlReleaseManager.Core.Repositories;

public class SqlServerRepository : ISqlServerRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SqlServerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

  

    public async Task<int> Add(CreateOrUpdateSqlServerInstance instance)
    {
        var sqlServerInstance = new SqlServerInstance
        {
            Name = instance.Name,
            Description = instance.Description,
            ConnectionString = instance.ConnectionString
        };

        _dbContext.SqlServerInstances.Add(sqlServerInstance);

        await _dbContext.SaveChangesAsync();

        return sqlServerInstance.Id;


    }

    public async Task UpdateDatabases(int instanceId, IEnumerable<DatabaseDto> databases)
    {
        var existingDatabases = _dbContext.DatabaseInstances.Where(x => x.SqlServerInstanceId == instanceId).ToList();

        var databasesToAdd = databases.Where(x => existingDatabases.All(y => y.DatabaseName != x.Name)).ToList();
        var databasesToRemove = existingDatabases.Where(x => !databases.Any(y => y.Name == x.DatabaseName)).ToList();

        foreach (var database in databasesToAdd)
        {
            _dbContext.DatabaseInstances.Add(new DatabaseInstance
            {
                Name = database.Name,
                Description = database.Name,
                DatabaseName = database.Name,
                SqlServerInstanceId = instanceId,
                DeploymentConfigurationId = 1
            });
        }

        foreach (var database in databasesToRemove)
        {
            _dbContext.DatabaseInstances.Remove(database);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<SqlServerInstance> GetById(int id )
    {
        var sqlServerInstance = await _dbContext.SqlServerInstances.SingleAsync(x => x.Id == id);

        return sqlServerInstance;
    }

    public async Task Delete(int id)
    {
        _dbContext.SqlServerInstances.Remove(new SqlServerInstance { Id = id });
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(int id, CreateOrUpdateSqlServerInstance instance)
    {
        var sqlServerInstance = await _dbContext.SqlServerInstances.SingleAsync(x => x.Id == id);
        sqlServerInstance.Name = instance.Name;
        sqlServerInstance.Description = instance.Description;
        sqlServerInstance.ConnectionString = instance.ConnectionString;
        await _dbContext.SaveChangesAsync();
    }
}