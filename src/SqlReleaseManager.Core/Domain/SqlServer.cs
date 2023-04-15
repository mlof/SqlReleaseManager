using Dapper;
using Microsoft.Data.SqlClient;
using SqlReleaseManager.Core.Abstractions;
using SqlReleaseManager.Core.Models;

namespace SqlReleaseManager.Core.Domain;

public class SqlServer : ISqlServer
{
    private readonly SqlConnection _dbConnection;
    private readonly SqlConnectionStringBuilder _builder;

    public string Name => _builder.DataSource;

    public SqlServer(string connectionString)
    {
        this._builder =
            new SqlConnectionStringBuilder(connectionString);


        this._dbConnection = new SqlConnection(connectionString);
    }

    public async Task<bool> CanConnect()
    {
        try
        {
            await _dbConnection.OpenAsync();
            await _dbConnection.CloseAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<IEnumerable<DatabaseDto>> GetDatabases()
    {
        var sql = @"SELECT 
                        name AS Name,
                        compatibility_level AS CompatibilityLevel,
                        collation_name AS Collation,
                        recovery_model_desc AS RecoveryModel,
                        state_desc AS Status,
                        create_date AS CreateDate
                    FROM sys.databases
                    ORDER BY name";

        return _dbConnection.QueryAsync<DatabaseDto>(sql);
    }


    public void Dispose()
    {
        _dbConnection.Dispose();
    }
}