using UI.Components;
using Recordings.UI.Models;
using Recordings.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var baseAddress = builder.Configuration["ApiSettings:BaseAddress"] ?? "localhost";

Console.WriteLine($"baseAddress={baseAddress}");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress)} );
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<RecordingsState>();

builder.Services.AddLogging(options =>
{
    options.AddSimpleConsole(c =>
    {
        c.TimestampFormat = "[yyyy-MM-ddTHH:mm:ss] ";
	c.UseUtcTimestamp = false;
	c.SingleLine = true;
    });
});

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
