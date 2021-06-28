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
using Hangfire;
using Hangfire.SqlServer;
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
            services.AddDbContext<NewsAggregatorContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<ImageStorage>(Configuration.GetSection("ImageStorage"));
            
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
                        throw new KeyNotFoundException();
                }
            });

            services.AddHangfire(conf => conf
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(30),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(30),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer();

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
                options.AddPolicy("Default", builder => 
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoodNewsAggregator v1"));

            app.UseHangfireDashboard();
            var rssSourceService = serviceProvider.GetService(typeof(IRssSourceService)) as IRssSourceService;
            var newService = serviceProvider.GetService(typeof(INewsService)) as INewsService;
            RecurringJob.AddOrUpdate(() => newService.RateNews() , "0-45/2 * 1/1 * ?");
            RecurringJob.AddOrUpdate(() => rssSourceService.GetNewsFromSources(), "50 * 1/1 * ?");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
