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
using AutoMapper;
using NewsAggregator.DAL.Core;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Implementation.Repositories;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Serviñes.Implementation;
using NewsAggregator.DAL.Serviñes.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Mapping;
using NewsAggregator.DAL.Serviñes.Implementation.Parsers;

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

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<ImageStorage>(Configuration.GetSection("ImageStorage"));
            //TODO mapping and check working with database, rework Dtos
            //TODO roles, letters and other pages about accounting 
            //TODO pagination
            //TODO logging 
            //TODO FluentValidation
            //TODO Checking authorization in comments 

            services.AddTransient<IRepository<News>, NewsRepository>();
            services.AddTransient<IRepository<Comment>, CommentRepository>();
            services.AddTransient<IRepository<Role>, RoleRepository>();
            services.AddTransient<IRepository<RssSource>, RssSourseRepository>();
            services.AddTransient<IRepository<User>, UserRepository>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IRssSourceService, RssSourceServiñe>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICleanService, CleanService>();

            services.AddTransient<OnlinerParser>();
            services.AddTransient<ShazooParser>();
            services.AddTransient<FourPdaParser>();
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
                    default:
                        throw new KeyNotFoundException(); // or maybe return null, up to you
                }
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping());
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

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
