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
using NewsAggregator.DAL.Servises.Implementation;
using NewsAggregator.DAL.Servises.Interfaces;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Implementation.Repositories;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Serviñes.Implementation;
using NewsAggregator.DAL.Serviñes.Interfaces;

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
            services.AddScoped<INewsServise, NewsServise>();
            services.AddScoped<IRssSourceService, RssSourceServiñe>();

            services.AddTransient<IWebParser, TutbyParser>();

            services.AddControllersWithViews();  
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
