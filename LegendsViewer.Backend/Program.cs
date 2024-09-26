
using LegendsViewer.Backend.Legends;
using LegendsViewer.Backend.Legends.Maps;
using LegendsViewer.Backend.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Text;
using System.Text.Json.Serialization;

namespace LegendsViewer.Backend;

public class Program
{
    public static void Main(string[] args)
    {
        // Register the encoding provider
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var builder = WebApplication.CreateBuilder(args);

        // Increase Kestrel's request timeout
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(5);
            serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(5);
        });

        // Add services to the container.
        builder.Services.AddSingleton<IWorld, World>();
        builder.Services.AddSingleton<IWorldMapImageGenerator, WorldMapImageGenerator>();

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Serialize Enums as strings globally
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add logging
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole(options => options.FormatterName = nameof(SimpleLogFormatter));
        builder.Logging.AddConsoleFormatter<SimpleLogFormatter, ConsoleFormatterOptions>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.MapControllers();

        // Create a logger instance
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        // Log at application start
        logger.LogInformation("LegendsViewer is starting...");

        //_ = WebAppStaticServer.RunAsync();
        //_ = WebAppStaticServer.OpenPageInBrowserAsync();

        app.Run();
    }
}
