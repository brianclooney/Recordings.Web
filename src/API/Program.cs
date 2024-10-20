using Microsoft.EntityFrameworkCore;
using Recordings.API.Data;
using Serilog;

namespace Recordings.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
                .ConfigureWebHostDefaults(wb => wb.UseStartup<Startup>())
                .Build();

                using var scope = host.Services.CreateScope();
                await using var dbContext = scope.ServiceProvider.GetRequiredService<RecordingDbContext>();
                try {
                    await dbContext.Database.MigrateAsync();
                }
                catch (Exception e)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "An error occurred during migration.");
		    // return Task.Cpmleted;
		    return;
                }

                await host.RunAsync();
        }
    }
}
