using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Prometheus;
using Recordings.API.Configuration;
using Recordings.API.Data;
using Recordings.API.Middleware;
using Recordings.API.Services;

namespace Recordings.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FilePathOptions>(Configuration.GetSection("FilePaths"));

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<RecordingDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("RecordingDatabase");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddScoped<IRecordingExtractionService, RecordingExtractionService>();
            services.AddHostedService<UptimeMetricHostedService>();

            services.AddLogging(options =>
            {
                options.AddSimpleConsole(c =>
                {
                    c.TimestampFormat = "[yyyy-MM-ddTHH:mm:ss] ";
                    c.UseUtcTimestamp = false;
                    c.SingleLine = true;
                });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<FilePathOptions> filePathOptionsAccessor)
        {
            FilePathOptions filePathOptions = filePathOptionsAccessor.Value;

            if (env.IsDevelopment())
            {
                app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<StaticFileMetricsMiddleware>();

            app.UseHttpMetrics();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(filePathOptions.StaticFileRootPath),
                RequestPath = filePathOptions.StaticFileRequestPath
            });

            app.UseRouting();
            app.UseEndpoints(e =>
            {
                e.MapMetrics();
                e.MapControllers();
            });
        }
    }
}
