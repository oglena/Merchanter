using Blazored.Toast;
using Merchanter;
using MerchanterApp.CMS;
using MerchanterApp.CMS.Components;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//builder.Services.AddSingleton<MerchanterService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddServerSideBlazor().AddCircuitOptions( o => {
    o.DetailedErrors = true;
} );

builder.Services.AddAuthentication( Variables.auth_cookie ).AddCookie( Variables.auth_cookie, options => {
    options.Cookie.Name = Variables.auth_cookie;
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
} );

builder.Services.AddAuthorization( options => {
    options.AddPolicy( Variables.admin_role, policy => policy.RequireRole( Variables.admin_role ) );
    options.AddPolicy( Variables.admin_id, policy => policy.RequireClaim( Variables.admin_id ) );
} );
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredToast();

var app = builder.Build();

// Configure the HTTP request pipeline.
if( !app.Environment.IsDevelopment() ) {
    app.UseExceptionHandler( "/Error", createScopeForErrors: true );
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthorization();

app.Run();
