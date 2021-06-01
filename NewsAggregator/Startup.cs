using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Servises.Interfaces;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Implementation.Repositories;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Serviñes.Implementation;
using NewsAggregator.DAL.Serviñes.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace NewsAggregator
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
            services.AddDbContext<NewsAggregatorContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefoultConnection")));

            services.AddTransient<IRepository<News>, NewsRepository>();
            services.AddTransient<IRepository<Comment>, CommentRepository>();
            services.AddTransient<IRepository<NewsWithTags>, NewsWithTagsRepository>();
            services.AddTransient<IRepository<Role>, RoleRepository>();
            services.AddTransient<IRepository<RssSource>, RssSourseRepository>();
            services.AddTransient<IRepository<Tag>, TagRepository>();
            services.AddTransient<IRepository<User>, UserRepository>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IRssSourceService, RssSourceServiñe>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMailService, MailService>();

            services.AddTransient<OnlinerParser>();
            services.AddTransient<ShazooParser>();
            services.AddTransient<FourPdaParser>();
            services.AddTransient<WylsaParser>();
            services.AddTransient<IgromanijaParser>();
            services.AddTransient<WebParserResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "Onliner":
                        return serviceProvider.GetService<OnlinerParser>();
                    case "Shazoo":
                        return serviceProvider.GetService<ShazooParser>();
                    case "4pda":
                        return serviceProvider.GetService<FourPdaParser>();
                    case "Wylsa":
                        return serviceProvider.GetService<WylsaParser>();
                    case "Igromanija":
                        return serviceProvider.GetService<IgromanijaParser>();
                    default:
                        throw new KeyNotFoundException(); // or maybe return null, up to you
                }
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.LoginPath = new PathString("/Account/Login");
                    opt.AccessDeniedPath = new PathString("/Account/Login");
                });

            services.AddControllersWithViews();  //Add filters
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=News}/{action=Index}/{id?}");
            });
        }
    }
}
