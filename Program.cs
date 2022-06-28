using System.Diagnostics;
using Honeycomb.OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient()
                .AddControllersWithViews();

// Open Telemetry Setup

builder.Services.AddHoneycomb(builder.Configuration);

// End Open Telemetry Setup

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

class ActivityHelper
{
    public static ActivitySource Fibonacci = new ActivitySource("Fibonacci");
}