using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using StreamInstruments.Helpers;

namespace StreamInstruments.DataAccess.Services;

public class StreamInstrumentsContextFactory : IDesignTimeDbContextFactory<StreamInstrumentsContext>
{
    public StreamInstrumentsContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StreamInstrumentsContext>();
        var connectionString = GetConnectionString();
        optionsBuilder.UseSqlite(connectionString);

        return new StreamInstrumentsContext(optionsBuilder.Options);
    }

    private static string GetConnectionString()
    {
        var config = ConfigurationHelper.GetConfig();

        return config.GetConnectionString("SqliteConnection");
    }
}