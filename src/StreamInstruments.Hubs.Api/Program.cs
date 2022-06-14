using Microsoft.EntityFrameworkCore;
using StreamInstruments.DataAccess;
using StreamInstruments.Helpers;
using StreamInstruments.Hubs.Api.Data;
using StreamInstruments.Hubs.Api.Domain.Infrastructure;
using StreamInstruments.Hubs.Api.Infrastructure.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<StreamInstrumentsContext>(opts =>
{
    opts.UseSqlite(ConfigurationHelper.GetDbConnectionString());
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
DomainInstaller.Install(builder.Services);
InfrastructureInstaller.Install(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();