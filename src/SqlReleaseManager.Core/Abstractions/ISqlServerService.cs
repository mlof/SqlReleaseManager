using SqlReleaseManager.Core.Commands;

namespace SqlReleaseManager.Core.Abstractions;

public interface ISqlServerService
{
    Task Create(CreateOrUpdateSqlServerInstance instance);
    Task UpdateDatabases(string name);
    Task<bool> CanConnectToInstance(string name);
}