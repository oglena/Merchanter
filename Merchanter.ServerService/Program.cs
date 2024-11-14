using AspNetCore.Swagger.Themes;
using Merchanter.CustomerService.Repositories;
using Merchanter.ServerService;
using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Repositories;
using Merchanter.ServerService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder( args );

builder.Host.UseWindowsService();
builder.Services.AddWindowsService();

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen( c => {
    c.AddSecurityDefinition( "bearerAuth", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    } );
    c.AddSecurityRequirement( new OpenApiSecurityRequirement{
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
    } );
    c.EnableAnnotations();
    c.OperationFilter<IgnorePropertyFilter>();
} );

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ITokenService, TokenService>();

var secret = builder.Configuration[ "AppSettings:Secret" ];
if( string.IsNullOrEmpty( secret ) ) {
    throw new ArgumentNullException( nameof( secret ), "AppSettings:Secret configuration is missing or empty." );
}

var safelist = builder.Configuration[ "AdminSafeList" ];
if( string.IsNullOrEmpty( safelist ) ) {
    throw new ArgumentNullException( nameof( safelist ), "AdminSafeList configuration is missing or empty." );
}

builder.Services.AddScoped<ClientIpCheckActionFilter>( container => {
    var loggerFactory = container.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();

    return new ClientIpCheckActionFilter( safelist, logger );
} );

builder.Services.AddScoped( x => new MerchanterService() );
builder.Services.AddScoped<IServerService, ServerService>();
builder.Services.AddScoped<IServerRepository, ServerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication( options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
} ).AddJwtBearer( o => {
    o.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = builder.Configuration[ "AppSettings:ValidIssuer" ],
        ValidAudience = builder.Configuration[ "AppSettings:ValidAudience" ],
        IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( secret ) ),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
    o.SaveToken = true;
} );
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews().AddJsonOptions( options => {
    options.JsonSerializerOptions.Converters.Add( new JsonStringEnumConverter() );
} );

builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();
var app = builder.Build();

app.UseMiddleware<AdminSafeListMiddleware>( builder.Configuration[ "AdminSafeList" ] );
app.MapHealthChecks( "/health" );

if( app.Environment.IsDevelopment() ) {
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI( ModernStyle.Dark, options => {
    options.SwaggerEndpoint( "v1/swagger.json", "Merchanter.ServerAPI V1" );
} );

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapDefaultEndpoints();

app.Run();