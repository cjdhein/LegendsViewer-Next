using System.Diagnostics;
using System.Reflection;

namespace LegendsViewer.Frontend;

public static class WebAppStaticServer
{
    public const uint WebAppPort = 8081;

    public static async Task RunAsync()
    {
        var assembly = Assembly.GetAssembly(typeof(WebAppStaticServer));
        var options = new WebApplicationOptions
        {
            ContentRootPath = Path.GetDirectoryName(assembly?.Location),
            // Look for static files in "legends-viewer-frontend/dist"
            WebRootPath = "legends-viewer-frontend/dist"
        };
        var builder = WebApplication.CreateBuilder(options);
        var app = builder.Build();
        app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = ["index.html"] });
        app.UseStaticFiles();
        app.MapFallbackToFile("index.html");
        await app.RunAsync($"http://*:{WebAppPort}");
    }

    public static async Task OpenPageInBrowserAsync()
    {
        await Task.Delay(200);
        var url = $"http://localhost:{WebAppPort}";
        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch
        {
            Console.WriteLine($"Failed to open URL: {url}");
        }
    }
}
