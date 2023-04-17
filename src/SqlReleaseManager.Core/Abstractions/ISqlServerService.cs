using SqlReleaseManager.Core.Commands;

namespace SqlReleaseManager.Core.Abstractions;

public interface ISqlServerService
{
    Task Create(CreateOrUpdateSqlServerInstance instance);
    Task UpdateDatabases(int name);
    Task<bool> CanConnectToInstance(int id);
}