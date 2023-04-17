using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlReleaseManager.Core.Abstractions;
using SqlReleaseManager.Core.Models;
using SqlReleaseManager.Core.SchemaComparison;
using ForeignKeyConstraint = System.Data.ForeignKeyConstraint;

namespace SqlReleaseManager.Core.Domain;

public class SqlServer : ISqlServer
{
    private readonly SqlConnection _dbConnection;
    private readonly SqlConnectionStringBuilder _builder;
    private readonly string _connectionString;

    public string Name => _builder.DataSource;
    public string Version { get; set; }
    public string Edition { get; set; }


    public SqlServer(string connectionString)
    {
        this._builder =
            new SqlConnectionStringBuilder(connectionString);

        if (_builder.InitialCatalog != "master")
        {
            throw new ArgumentException("Connection string must be to the master database");
        }

        this._connectionString = _builder.ToString();


        this._dbConnection = new SqlConnection(_connectionString);
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

    public Schema GetSchema(string databaseName)
    {
        var connectionString = GetConnectionStringForDatabase(databaseName);
        var builder = new SchemaBuilder()
            .ForTSqlModel(TSqlModel.LoadFromDatabase(connectionString));


        return builder.Build();
    }

    private string GetConnectionStringForDatabase(string databaseName)
    {
        var builder = new SqlConnectionStringBuilder(_connectionString)
        {
            InitialCatalog = databaseName
        };
        return builder.ToString();
    }


    public void Dispose()
    {
        _dbConnection.Dispose();
    }
}