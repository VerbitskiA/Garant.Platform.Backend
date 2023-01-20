using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Mapper;
using Garant.Platform.Core.Utils;
using Garant.Platform.Messaging.Core;
using Garant.Platform.Models.Entities.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Garant.Platform.Core.Filters;

namespace Garant.Platform
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public ContainerBuilder ContainerBuilder { get; }
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ContainerBuilder = new ContainerBuilder();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddControllersAsServices();

            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder
                    .WithOrigins(Configuration.GetSection("CorsUrls:Urls").Get<string[]>())
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }));

            services.AddSignalR();

            #region Для прода.

            // services.AddDbContext<IdentityDbContext>(options =>
            //     options.UseNpgsql(Configuration.GetConnectionString("NpgConfigurationConnectionRu")));

            #endregion

            #region Для теста.

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("NpgTestSqlConnectionRu")));

            #endregion

            services.AddIdentity<UserEntity, IdentityRole>(opts =>
                {
                    opts.Password.RequiredLength = 5;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Garant.Platform", Version = "v1" });                

                c.OperationFilter<AuthResponsesOperationFilter>();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            ApplicationContainer = AutoFac.Init(cb => { cb.Populate(services); });

            // Добавит доступ к контексту из любого компонента.
            services.AddHttpContextAccessor();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("ApiCorsPolicy");
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<NotifyHub>("/notify");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Garant.Platform v1"));

            // Наполнит словарь для динамической смены датаконтекстов при работе с разными БД.
            var connStrs = new Dictionary<string, string>();
            connStrs.TryAdd("NpgTestSqlConnectionRu", Configuration.GetConnectionString("NpgTestSqlConnectionRu"));
            connStrs.TryAdd("NpgConfigurationConnectionRu", Configuration.GetConnectionString("NpgConfigurationConnectionRu"));
            connStrs.TryAdd("NpgTestSqlConnectionEn", Configuration.GetConnectionString("NpgTestSqlConnectionEn"));
            connStrs.TryAdd("NpgConfigurationConnectionEn", Configuration.GetConnectionString("NpgConfigurationConnectionEn"));
            DbContextFactory.SetConnectionString(connStrs);
        }
    }
}