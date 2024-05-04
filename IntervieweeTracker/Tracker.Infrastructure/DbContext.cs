using System.Data.SQLite;
using Microsoft.Extensions.Configuration;

namespace Tracker.Infrastructure;

public class DbContext : IDisposable
{
    private readonly Lazy<SQLiteConnection> _connection;

    public SQLiteConnection Connection => _connection.Value;

    public DbContext(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        
        var dbConnection = configuration.GetSection("Sqlite").GetSection("DbConnection").Value ?? throw new Exception();

        _connection = new Lazy<SQLiteConnection>(
            () =>
            {
                var connection = new SQLiteConnection(dbConnection);
                connection.BeginTransaction();
                return connection;
            });
    }

    public void Dispose()
    {
        _connection.Value.Dispose();
    }
}