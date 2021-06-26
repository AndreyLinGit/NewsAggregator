using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Core.Mapping;
using NewsAggregator.DAL.CQRS.QueryHandlers.RssSourcesHandlers;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Implementation.Repositories;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Serviñes.Implementation;
using NewsAggregator.DAL.Serviñes.Implementation.Parsers;
using NewsAggregator.DAL.Serviñes.Interfaces;
using NewsAggregator.DAL.Serviñes.Implementation.Cqrs;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GoodNewsAggregator
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

            //services.AddTransient<IRepository<News>, NewsRepository>();
            //services.AddTransient<IRepository<Comment>, CommentRepository>();
            //services.AddTransient<IRepository<Role>, RoleRepository>();
            //services.AddTransient<IRepository<RssSource>, RssSourseRepository>();
            //services.AddTransient<IRepository<User>, UserRepository>();

            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INewsService, NewsCqrsServices>();
            services.AddScoped<IRssSourceService, RssSourceCqrsServiñe>();
            services.AddScoped<IUserService, UserCqrsService>();
            services.AddScoped<IRoleService, RoleCqrsService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ICommentService, CommentCqrsService>();
            services.AddScoped<ICleanService, CleanService>();

            services.AddScoped<IJwtAuthManager, JwtAuthManager>();
            services.AddScoped<IRefreshTokenService, RefreshTokenCqrsService>();

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

            services.AddMediatR(typeof(GetAllRssSourcesQueryHandler).GetTypeInfo().Assembly);

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;

                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddCors(options =>
            {
                options.AddPolicy("Default",
                    builder => 
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); 

                    });


            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GoodNewsAggregator", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoodNewsAggregator v1"));
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
