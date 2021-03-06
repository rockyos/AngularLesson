﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PhotoAPI.Automapper;
using PhotoAPI.Models.Email;
using PhotoAPI.Models.Entity;
using PhotoAPI.Repository;
using PhotoAPI.Services;
using PhotoAPI.Services.Interfaces;

namespace PhotoAPI
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
            string conn = Configuration.GetConnectionString("ConnectionToDB");
            services.AddDbContext<PhotoContext>(options => options.UseSqlServer(conn));

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<PhotoContext>()
            .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
               cfg.RequireHttpsMetadata = false;
               cfg.SaveToken = true;
               cfg.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidIssuer = Configuration["JwtIssuer"],
                   ValidAudience = Configuration["JwtIssuer"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                   ClockSkew = TimeSpan.Zero 
               };
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) 
            .AddGoogle(options =>
            {
                options.ClientId = Configuration["GoogleID"]; 
                options.ClientSecret = Configuration["GoogleKey"]; 
            }).AddFacebook(options =>
            {
                options.AppId = Configuration["FacebookID"];
                options.AppSecret = Configuration["FacebookKey"];
            }).AddCookie();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddSingleton<IEmailSender, EmailSenderService>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy( builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            services.AddDistributedMemoryCache(); 

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });



            services.AddScoped<UnitOfWork>();
            services.AddScoped<IResizeService, ResizeService>();
            services.AddScoped<IGetPhotoService, GetPhotoService>();
            services.AddScoped<IAddPhotoService, AddPhotoService>();
            services.AddScoped<ISavePhotoService, SavePhotoService>();
            services.AddScoped<IDeleteService, DeleteService>();
            services.AddScoped<IGenerateJwtTokenService, GenerateJwtTokenService>();
            services.AddScoped<IGetExternalLoginService, GetExternalLoginService>();
            services.AddHttpClient<IGetExternalLoginService, GetExternalLoginService>();
            //services.AddScoped<UserManager<IdentityUser>>();
            //services.AddScoped<SignInManager<IdentityUser>>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddViewOptions(options =>
            {
                options.HtmlHelperOptions.ClientValidationEnabled = true;
            });


            var mapconfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            services.AddSingleton(mapconfig.CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
