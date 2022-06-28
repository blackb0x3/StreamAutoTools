using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StreamInstruments.Helpers;

namespace StreamInstruments.DataAccess.Services;

public static class DataAccessInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddDbContext<StreamInstrumentsContext>(opts =>
        {
            opts.UseSqlite(ConfigurationHelper.GetDbConnectionString());
        });
    }
}