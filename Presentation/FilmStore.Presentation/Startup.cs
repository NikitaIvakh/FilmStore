using FilmStore.Contractors.YandexKassa;
using FilmStore.Contractors;
using FilmStore.Data.EF;
using FilmStore.Messages;
using FilmStore.Presentation.Application;
using FilmStore.Presentation.Contractors;
using Microsoft.AspNetCore.Identity;

namespace FilmStore.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
            });

            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddEfRepositories(Configuration.GetConnectionString("FilmStore"));

            services.AddScoped<INotificationService, DebugNotificationService>();
            services.AddScoped<IDeliveryService, PostamateDeliveryService>();
            services.AddScoped<IPaymentService, CachPaymentService>();
            services.AddScoped<IPaymentService, YandexKassaPaymentService>();
            services.AddScoped<IWebContractorService, YandexKassaPaymentService>();
            services.AddScoped<FilmService>();
            services.AddScoped<OrderService>();

            services.AddScoped<IFilmRepository, FilmRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<Dictionary<Type, FilmStoreDbContext>>();
            services.AddScoped<DbContextFactory>();

            //services.AddDefaultIdentity<IdentityUser>(options =>
            //{
            //    options.Password.RequiredDigit = true;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            if (false)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}