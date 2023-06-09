﻿using Business.Interfaces;
using Business.Services;
using Business.Validation;
using Data.Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private string[] controllers = new string[] { "order", "user", "product", "receipt", "recommendation", "statistic" };
        public IConfiguration Configuration { get; }

        public async void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("Test_Database");

            services.AddControllers();

            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
            .EnableSensitiveDataLogging()
            .UseInMemoryDatabase(databaseName: "Test_Database").Options);

            await TestData.SeedData(context);

            var unitOfWork = new UnitOfWork(context);

            services.AddDbContext<ShopOfThingsDBContext>(t => t.UseSqlite(connection));

            services.AddAutoMapper(typeof(AutomapperProfile).Assembly);

            services.AddSingleton<IUnitOfWork>(x => unitOfWork);

            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IProductService, ProductService>();

            services.AddTransient<IReceiptService, ReceiptService>();

            services.AddTransient<IOrderService, OrderService>();

            services.AddTransient<IStatisticService, StatisticService>();

            services.AddTransient<IRecommendationService, RecommendationService>();

            #region Swagger Configuration
            services.AddSwaggerGen(swagger =>
            {
                foreach (var controllerName in controllers) 
                {
                    swagger.SwaggerDoc(controllerName, new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "JWT Token Authentication API",
                        Description = "ASP.NET Core 5.0 Web API",
                        Contact = new OpenApiContact
                        {
                            Name = "itsfinniii"
                        }
                    });
                }
                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });
            #endregion

            #region Authentication
            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme);

                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])) //Configuration["JwtToken:SecretKey"]
                };
            });
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    foreach (var controllerName in controllers) 
                    {
                        c.SwaggerEndpoint($"/swagger/{controllerName}/swagger.json", controllerName);
                    }
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
