using MerchanterApp.CMS;
using MerchanterApp.CMS.Classes;
using MerchanterApp.CMS.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder( args );
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem( new DirectoryInfo( builder.Environment.ContentRootPath + @"\keys" ) )
    .SetApplicationName( "MerchanterApp.CMS" )
    .SetDefaultKeyLifetime( TimeSpan.FromDays( 90 ) );

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.WebHost.UseWebRoot( "wwwroot" ).UseStaticWebAssets();
builder.Services.AddServerSideBlazor().AddCircuitOptions( x => {
    x.DetailedErrors = true;
    x.DisconnectedCircuitMaxRetained = 10;
} );

builder.Services.AddTransient<IPostHelper, PostHelper>();

builder.Services.AddAuthentication( Variables.auth_cookie ).AddCookie( Variables.auth_cookie, x => {
    x.Cookie.Name = Variables.auth_cookie;
    x.LoginPath = "/Account/Login";
    x.LogoutPath = "/Account/Logout";
    x.AccessDeniedPath = "/Account/AccessDenied";
} );

builder.Services.AddAuthorization( x => {
    x.AddPolicy( Variables.admin_role, policy => policy.RequireRole( Variables.admin_role ) );
    x.AddPolicy( Variables.admin_id, policy => policy.RequireClaim( Variables.admin_id ) );
} );
builder.Services.AddCascadingAuthenticationState();

var deploymentName = builder.Configuration[ "AzureOpenAIChatCompletion:DeploymentName" ];
var endpoint = builder.Configuration[ "AzureOpenAIChatCompletion:Endpoint" ];
var apiKey = builder.Configuration[ "AzureOpenAIChatCompletion:Key" ];

if( deploymentName is null || endpoint is null || apiKey is null ) {
    throw new InvalidOperationException( "Azure OpenAI Chat Completion configuration is missing." );
}

builder.Services.AddAzureOpenAIChatCompletion( deploymentName, endpoint, apiKey );

var app = builder.Build();
// Configure the HTTP request pipeline.
if( !app.Environment.IsDevelopment() ) {
    app.UseExceptionHandler( "/Error", createScopeForErrors: true );
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.Run();
