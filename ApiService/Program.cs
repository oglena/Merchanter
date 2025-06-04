using ApiService.Classes;
using ApiService.Repositories;
using ApiService.Services;
using MerchanterApp.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => {
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(60);
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(60);
});

builder.Host.UseWindowsService();
builder.Services.AddWindowsService();

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("merchanter", new OpenApiInfo { Title = "Merchanter API", Version = "v1" });
    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            new string[] { }
        }
    });
    c.EnableAnnotations();
    c.OperationFilter<IgnorePropertyFilter>();
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MerchanterApi.xml"));
    c.AddScalarFilters();
});

#region Merchanter
var secret = builder.Configuration["AppSettings:Secret"];
if (string.IsNullOrEmpty(secret)) {
    throw new ArgumentNullException(nameof(secret), "AppSettings:Secret configuration is missing or empty.");
}

var safelist = builder.Configuration["AdminSafeList"];
if (string.IsNullOrEmpty(safelist)) {
    throw new ArgumentNullException(nameof(safelist), "AdminSafeList configuration is missing or empty.");
}

builder.Services.AddScoped<ClientIpCheckActionFilter>(container => {
    var loggerFactory = container.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();

    return new ClientIpCheckActionFilter(safelist, logger);
});

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped(x => new MerchanterService());
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
#endregion

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o => {
    o.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = builder.Configuration["AppSettings:ValidIssuer"],
        ValidAudience = builder.Configuration["AppSettings:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
    o.SaveToken = true;
});
builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseMiddleware<AdminSafeListMiddleware>(builder.Configuration["AdminSafeList"]);
app.MapHealthChecks("/health");
app.UseCors();

app.MapDefaultEndpoints();
app.UseSwagger();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();
app.MapSwagger("/swagger/{documentName}.json");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseRouter(routes => {
        routes.MapGet("/", async context => {
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("<h1>Merchanter API</h1>");
            await context.Response.WriteAsync("Welcome to the Merchanter API!<br>");
            await context.Response.WriteAsync("Use the Scalar UI to explore the API: <a href=\"/scalar\">Scalar UI</a><br>");
            await context.Response.WriteAsync("Or access the Swagger documentation: <a href=\"/swagger/merchanter.json\">Swagger JSON</a><br>");
        });
    });
    app.UseDeveloperExceptionPage();
    app.MapScalarApiReference(options => {
        options.WithLayout(ScalarLayout.Modern);
        options.WithTitle("Merchanter API Reference");
        options.WithTheme(ScalarTheme.DeepSpace);
        options.WithDefaultHttpClient(ScalarTarget.Php, ScalarClient.Guzzle);
        options.AddDocument("merchanter");
        options.WithOpenApiRoutePattern("/swagger/merchanter.json");
    });
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();



app.Run();