using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using Serilog;

namespace formica;

public class Program
{
    public static void Main(string[] args)
    {
        using var consoleLogger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        Log.Logger = consoleLogger; // Set to global logger

        try
        {
            InitializeDirectories();
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(C.Data));

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Something went wrong");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
    static void InitializeDirectories()
    {
        Directory.CreateDirectory(C.Data);
        var settingsJson = C.DataFor("settings.json");
        var settingsJsonExample = C.DataFor("settings.example.json");
        if (!File.Exists(settingsJson) && !File.Exists(settingsJsonExample))
            File.WriteAllText(settingsJsonExample, JsonSerializer.Serialize(C.Settings, C.JsonOpt));

        if (!File.Exists(settingsJson))
            throw new FileNotFoundException("Must configure settings.json");

        var settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(settingsJson), C.JsonOpt);
        if (settings == null)
            throw new JsonException("Could not parse settings.json");

        C.Settings = settings;
    }
}