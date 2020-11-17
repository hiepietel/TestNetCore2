using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TestNetCore2.Jobs;
using TestNetCore2.Services;
using TestNetCore2.Services.IService;

namespace TestNetCore2
{
    public class Startup
    {
        public string ConnectionString { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
#if (DEBUG || RELEASE)
            ConnectionString = Configuration.GetValue<string>("db");
            services.AddDbContext<ApplicationContext>(c => c.UseSqlServer(ConnectionString, opt => opt.EnableRetryOnFailure(3)));

#elif (DEBUGMYSQL || RELEASEMYSQL)
            ConnectionString = Configuration.GetValue<string>("mysqldb");
            services.AddDbContext<ApplicationContext>(c => c.UseMySQL(ConnectionString));
#endif
            services.AddHangfire(configuration => configuration
             .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UseMemoryStorage());
            services.AddHangfireServer();

            

            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IDeviceService, DeviceService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<ITemperatureService, TemperatureService>();
            services.AddTransient<ICovidService, CovidService>();
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHangfireDashboard("/hangfire");
            RecurringJob.AddOrUpdate<DeviceAliveJob>(x => x.Execute(), "0 * * * *");
            RecurringJob.AddOrUpdate<TemperatureJob>(x => x.Execute(), "0 */10 * ? * *");
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
