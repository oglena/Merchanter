using ApiService.Classes;
using ApiService.Repositories;
using ApiService.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ApiService.Startup {
    /// <summary>
    /// Provides extension methods for configuring services and dependencies in the Merchanter application.
    /// </summary>
    /// <remarks>This class contains methods to register application-specific services, repositories, and
    /// middleware,  as well as to configure Swagger and authentication settings. These methods are intended to be used 
    /// during application startup to ensure proper initialization of required components.</remarks>
    public static class StartupExtensions {
        /// <summary>
        /// Configures the Merchanter application services and dependencies.
        /// </summary>
        /// <remarks>This method registers various services and repositories required by the Merchanter
        /// application. It also validates the presence of required configuration values, such as
        /// <c>AppSettings:Secret</c> and <c>AdminSafeList</c>. If these configuration values are missing or empty, an
        /// <see cref="ArgumentNullException"/> is thrown.</remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the services will be added.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance used to retrieve configuration values.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <c>AppSettings:Secret</c> or <c>AdminSafeList</c> configuration values are missing or empty.</exception>
        public static void ConfigureMerchanterServices(this IServiceCollection services, IConfiguration configuration) {
            var secret = configuration["AppSettings:Secret"];
            if (string.IsNullOrEmpty(secret)) {
                ArgumentNullException argumentNullException = new(nameof(configuration), "AppSettings:Secret configuration is missing or empty.");
                throw argumentNullException;
            }

            var safelist = configuration["AdminSafeList"];
            if (string.IsNullOrEmpty(safelist)) {
                throw new ArgumentNullException(nameof(configuration), "AdminSafeList configuration is missing or empty.");
            }

            services.AddScoped(container => {
                var loggerFactory = container.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();

                return new ClientIpCheckActionFilter(safelist, logger);
            });

            services.AddScoped(x => new MerchanterService());
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<ICatalogRepository, CatalogRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<ISettingsRepository, SettingsRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }

        /// <summary>
        /// Configures Swagger and authentication services for the Merchanter application.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the services will be added.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance used to retrieve configuration values.</param>
        public static void ConfigureSwaggerAndAuthentication(this IServiceCollection services, IConfiguration configuration) {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("merchanter", new OpenApiInfo { Title = "Merchanter API", Version = "v1" });
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
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
            });

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => {
                o.TokenValidationParameters = new TokenValidationParameters {
                    ValidIssuer = configuration["AppSettings:ValidIssuer"],
                    ValidAudience = configuration["AppSettings:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["AppSettings:Secret"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
                o.SaveToken = true;
            });

            services.AddAuthorization();
        }
    }
}
