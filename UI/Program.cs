using UI.Components;
using Recordings.UI.Models;
using Recordings.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var baseAddress = builder.Configuration["ApiSettings:BaseAddress"] ?? "localhost";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress)} );
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<RecordingsState>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
