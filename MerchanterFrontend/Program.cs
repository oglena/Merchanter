using MerchanterFrontend;
using MerchanterFrontend.Classes;
using MerchanterFrontend.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(builder.Environment.ContentRootPath + @"\keys"))
    .SetApplicationName("Merchanter.Frontend")
    .SetDefaultKeyLifetime(TimeSpan.FromDays(90));

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddScoped<DialogService>();
builder.Services.AddFluentUIComponents();
//builder.Services.AddDataGridEntityFrameworkAdapter();
//builder.Services.AddDataGridODataAdapter();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.WebHost.UseWebRoot("wwwroot").UseStaticWebAssets();
builder.Services.AddServerSideBlazor().AddCircuitOptions(x => {
    x.DetailedErrors = true;
    x.DisconnectedCircuitMaxRetained = 10;
});

builder.Services.AddTransient<IPostHelper, PostHelper>();

builder.Services.AddAuthentication(Variables.auth_cookie).AddCookie(Variables.auth_cookie, x => {
    x.Cookie.Name = Variables.auth_cookie;
    x.LoginPath = "/Account/Login";
    x.LogoutPath = "/Account/Logout";
    x.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthorization(x => {
    x.AddPolicy(Variables.customer_role, policy => policy.RequireRole(Variables.customer_role));
    x.AddPolicy(Variables.customer_id, policy => policy.RequireClaim(Variables.customer_id));
});
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
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