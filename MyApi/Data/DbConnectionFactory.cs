using System.Data;
using System.Data.SqlClient;

namespace MyApi.Data;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    
    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }


    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
        return services;
    }
}