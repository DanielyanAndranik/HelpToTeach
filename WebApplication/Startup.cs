﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Couchbase.Extensions.DependencyInjection;
using System;
using HelpToTeach.Core.Repository;
using HelpToTeach.Core.AI;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCouchbase(Configuration.GetSection("Couchbase"))
                .AddCouchbaseBucket<INamedBucketProvider>("HelpToTeachBucket");

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IGroupCourseRepository, GroupCourseRepository>();
            services.AddScoped<IMarkRepository, MarkRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IMLDataRepository, MLDataRepository>();
            

            services.AddAutoMapper();
        
            services.AddMvc(options => 
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.AccessDeniedPath = "/Home/ErrorForbidden";
                    options.LoginPath = "/Home/ErrorNotLoggedIn";
                    options.Cookie.Expiration = TimeSpan.FromHours(12);
                    options.Cookie.MaxAge = TimeSpan.FromHours(12);
                    options.Cookie.HttpOnly = true;
                });

            PythonRunner.Configuration = Configuration;

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorOnly", policy => policy.RequireRole("admin"));
            });
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

