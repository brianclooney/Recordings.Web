using Client.Web.Components;
using Recordings.Client.Web.Models;
using Recordings.Client.Web.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add serilog services to the container and read config from appsettings
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
    
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var baseAddress = builder.Configuration["ApiSettings:BaseAddress"] ?? "localhost";

Console.WriteLine($"baseAddress={baseAddress}");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress)} );
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<RecordingsState>();

// builder.Services.AddLogging(options =>
// {
//     options.AddSimpleConsole(c =>
//     {
//         c.TimestampFormat = "[yyyy-MM-ddTHH:mm:ss] ";
// 	c.UseUtcTimestamp = false;
// 	c.SingleLine = true;
//     });
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
