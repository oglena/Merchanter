using Merchanter;
using MerchanterApp.ApiService.Classes;
using MerchanterApp.ApiService.Repositories;
using MerchanterApp.ApiService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder( args );

builder.Host.UseWindowsService();
builder.Services.AddWindowsService();

builder.Services.AddCors( options => {
    options.AddDefaultPolicy(
        builder => {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        } );
} );

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();
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
} );

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddScoped<ClientIpCheckActionFilter>( container => {
    var loggerFactory = container.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();

    return new ClientIpCheckActionFilter(
        builder.Configuration[ "AdminSafeList" ], logger );
} );

builder.Services.AddScoped( x => new MerchanterService() );
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

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
        IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( builder.Configuration[ "AppSettings:Secret" ] ) ),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
    };
    o.SaveToken = true;
} );
builder.Services.AddAuthorization();

builder.Services.AddHealthChecks();
var app = builder.Build();

app.UseMiddleware<AdminSafeListMiddleware>( builder.Configuration[ "AdminSafeList" ] );
app.MapHealthChecks( "/health" );
app.UseCors();

if( app.Environment.IsDevelopment() ) {
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI( c => {
    c.SwaggerEndpoint( "v1/swagger.json", "Merchanter.API V1" );
} );

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapDefaultEndpoints();

app.Run();