using Dottor.DotNetConf2026.Web.Components;
using Dottor.DotNetConf2026.Web.Endpoints;
using Dottor.DotNetConf2026.Web.Services;
using Dottor.DotNetConf2026.Web.Shutdown;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.AddCircuitShutdown();
builder.Services.AddSingleton<DemoInstance>();
builder.Services.AddSingleton<UserDirectory>();
builder.Services.AddSingleton<TicketCatalog>();
builder.Services.AddSingleton<SubmitCounter>();
builder.Services.AddSingleton<CategoryReportService>();
builder.Services.AddOptions<AutoPauseDemoOptions>()
    .Bind(builder.Configuration.GetSection("AutoPause"))
    .Validate(options => options.HiddenDelay > TimeSpan.Zero, "AutoPause:HiddenDelay deve essere maggiore di zero.")
    .ValidateOnStart();
builder.Services.AddValidation();
builder.Services.AddOpenApi(options => options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_2);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api"), branch =>
    branch.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true));
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCircuitShutdown();

app.MapDemoEndpoints();
app.MapOpenApi();
app.MapStaticAssets();
var autoPause = app.Services.GetRequiredService<IOptions<AutoPauseDemoOptions>>().Value;
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    // Opt-in package Microsoft.AspNetCore.Components.Server.AutoPause: applies to every interactive page.
    .WithBrowserOptions(options => options.AddAutoPause(pause =>
    {
        pause.Enabled = autoPause.Enabled;
        pause.HiddenDelay = autoPause.HiddenDelay;
    }));

app.Run();
