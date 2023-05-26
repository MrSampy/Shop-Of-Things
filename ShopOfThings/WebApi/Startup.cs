using Business.Interfaces;
using Business.Services;
using Business.Validation;
using Data.Data;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
