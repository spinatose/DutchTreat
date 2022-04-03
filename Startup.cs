using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace DutchTreat
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
            services.AddTransient<DutchSeeder>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly()); 
            services.AddScoped<IDutchAdapter, DutchAdapter>();
            services.AddTransient<IMailService, NullMailService>(); // dependency injection baby!!
            services.AddMvc();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation() // "razor compilation" search terms in nuget pkg manager
                .AddNewtonsoftJson(cfg => cfg.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddRazorPages();
            services.AddDbContext<DutchContext>();

            services.AddIdentity<StoreUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
               .AddEntityFrameworkStores<DutchContext>()
               .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                    {
                        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                        {
                            ValidIssuer = Configuration["Token:Issuer"],
                            ValidAudience = Configuration["Token:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"]))
                        };
                    }
                );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/error"); // use a razor page so we don't have to create a whole new controller
            
            //app.Run(async context =>
            //    {
            //        await context.Response.WriteAsync("Hola mundo!");
            //    });

            // order here matters for middleware
            // app.UseDefaultFiles(); // this tells it to look for default index.html or other default pages first- don't want this if we are going to use MVC
            app.UseStaticFiles();
            
            // this tells MVC how to route traffic into site
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(cfg =>
            {
                cfg.MapRazorPages();
                cfg.MapControllerRoute("Default", "/{controller}/{action}/{id?}", new { controller = "App", action = "Index" }); //this is the default route and it tells it which controller is our default if someone hits root of site
            });
        }
    }
}
