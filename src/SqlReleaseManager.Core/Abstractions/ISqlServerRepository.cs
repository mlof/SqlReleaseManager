using SqlReleaseManager.Core.Commands;
using SqlReleaseManager.Core.Models;
using SqlReleaseManager.Core.Persistence;

namespace SqlReleaseManager.Core.Abstractions;

public interface ISqlServerRepository
{
    Task<int> Add(CreateOrUpdateSqlServerInstance instance);
    Task UpdateDatabases(int instanceId, IEnumerable<DatabaseDto> databases);
    Task<SqlServerInstance> GetById(int id);
    Task Delete(int id);
    Task Update(int id, CreateOrUpdateSqlServerInstance instance);
}