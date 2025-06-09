using ApiService.Classes;
using ApiService.Startup;
using MerchanterApp.ServiceDefaults;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => {
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(60);
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(60);
});

builder.Host.UseWindowsService(); // Configure the host to run as a Windows Service
builder.Services.AddWindowsService(); // Add Windows Service support

builder.AddServiceDefaults(); // Add service defaults for the Merchanter application
builder.Services.AddProblemDetails(); // Add support for problem details in API responses
builder.Services.AddHealthChecks(); // Add health checks for monitoring application health

builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); // Add JSON options to handle enum serialization
builder.Services.AddEndpointsApiExplorer(); // Add support for API endpoint exploration

builder.Services.ConfigureMerchanterServices(builder.Configuration); // Configure Merchanter services and dependencies
builder.Services.ConfigureSwaggerAndAuthentication(builder.Configuration); // Configure Swagger and authentication services

var app = builder.Build(); // Build the application pipeline

app.UseMiddleware<AdminSafeListMiddleware>(builder.Configuration["AdminSafeList"]); // Use middleware for admin safelist based on configuration
app.MapHealthChecks("/health"); // Map health checks endpoint
app.UseCors(); // Use CORS policy defined in ServiceDefaults

app.MapDefaultEndpoints(); // Map default endpoints for the application
app.UseSwagger(); // Enable Swagger middleware for API documentation
app.UseStaticFiles(); // Serve static files from wwwroot
app.UseRouting(); // Enable routing for the application
app.UseAuthentication(); // Use authentication middleware to handle JWT tokens
app.UseAuthorization(); // Use authorization middleware to enforce access control
app.MapControllers(); // Map controller endpoints for the application
app.MapSwagger("/swagger/{documentName}.json"); // Map Swagger JSON endpoint for API documentation
app.UseExceptionHandler(); // Use exception handler middleware for global error handling

if (app.Environment.IsDevelopment()) {
    //TODO: Enable Swagger UI in development environment
    app.UseDeveloperExceptionPage(); // Use developer exception page for detailed error information in development
}

app.UseRouter(routes => {
    routes.MapGet("/", async context => {
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync("<h1>Merchanter API</h1>");
        await context.Response.WriteAsync("Welcome to the Merchanter API!<br>");
        await context.Response.WriteAsync("Use the Scalar UI to explore the API: <a href=\"/scalar\">Scalar UI</a><br>");
        await context.Response.WriteAsync("Or access the Swagger documentation: <a href=\"/swagger/merchanter.json\">Swagger JSON</a><br>");
    });
}); // Map a default route for the application
app.MapScalarApiReference(options => {
    options.WithClientButton(false);
    options.WithLayout(ScalarLayout.Modern);
    options.WithTitle("Merchanter API Reference");
    options.WithTheme(ScalarTheme.DeepSpace);
    options.WithDefaultHttpClient(ScalarTarget.Php, ScalarClient.Guzzle);
    options.AddDocument("merchanter");
    options.WithOpenApiRoutePattern("/swagger/merchanter.json");
}); // Configure the Scalar API reference with custom options


app.Run(); // Run the application