using Serilog;

namespace Recordings.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
		.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
                .ConfigureWebHostDefaults(wb => wb.UseStartup<Startup>())
                .Build()
                .Run();
        }
    }
}
