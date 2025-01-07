using Autofac;
using Autofac.Extensions.DependencyInjection;
using FinTrack.RestApi.ActionFilters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Serilog;
using System.Text;

namespace FinTrack.RestApi
{
    public class Startup
    {
        private readonly string _corsPolicyName = "FinTrackCorsPolicy";
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private ILifetimeScope _autofacContainer;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var settings = _configuration.Get<ApiSettings>();

            services.AddControllers(options =>
            {
                options.Filters.Add<UserContextActionFilter>();
                options.Filters.Add<LocaleActionFilter>();
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "FinTrack API", Version = "v1" });

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddStackExchangeRedisCache(options => {
                options.Configuration = settings.ConnectionStrings.RedisDatabase;
                options.InstanceName = settings.ConnectionStrings.RedisInstanceName;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(_corsPolicyName, builder =>
                {
                    builder.WithOrigins(settings.AllowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:5001",
                    ValidAudience = "https://localhost:5001",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Auth.Secret))
                };
            });

            services.AddSerilog();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRouting();
            app.UseCors(_corsPolicyName);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinTrack.Rest.Api v1"));


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (app is not null)
            {
                _autofacContainer = app.ApplicationServices.GetAutofacRoot();
            }

            loggerFactory.AddSerilog();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var options = _configuration.Get<ApiSettings>();
            ContainerConfiguration.ResisterTypes(builder, options);

            // register filters
            builder.RegisterType<UserContextActionFilter>().AsSelf();
        }
    }
}
