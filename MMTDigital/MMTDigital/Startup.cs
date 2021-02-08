using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MMTDigital.Model;
using MMTDigital.Services;
using System.Web;
using AutoMapper;
using MMTDigital.Helper;
using MMTDigital.ViewModel;
using MMTDigital.Context;

namespace MMTDigital
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers();
            services.AddHttpClient();

            //setting-up base url for connection to external Api using api-key authentication
            services.AddHttpClient<MMTDigitalClient>(client =>
            {
                var builder = new UriBuilder(Configuration["Api-Uri"]);
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["code"] = Configuration["Api-Key"];
                builder.Query = query.ToString();
                client.BaseAddress = new Uri(builder.ToString());
                client.DefaultRequestHeaders.Add("Accept", "application/json");              
            });

            //setting-up connection to Sql Server database
            services.AddDbContext<ConnectionDB>(options =>
                options.UseSqlServer(Configuration["MyConnection"]));

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            //Registering all required classes for Dependency Injection
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IUtilityService, UtilityService>();
            services.AddTransient<Customer, Customer>();
            services.AddTransient<CustomerRecentOrder, CustomerRecentOrder>();
            services.AddTransient<OrderView, OrderView>();
            services.AddTransient<ProductView, ProductView>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
