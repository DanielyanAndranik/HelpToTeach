using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Couchbase.Extensions.DependencyInjection;
using HelpToTeach.Core.Repository;
using HelpToTeach.Data.Models;

namespace WebApi
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
            services.AddScoped<IRepository<Course>, CouchbaseRepository<Course>>();
            services.AddScoped<IRepository<Student>, CouchbaseRepository<Student>>();
            services.AddScoped<IRepository<Group>, CouchbaseRepository<Group>>();
            services.AddScoped<IMarkRepository, MarkRepository>();
            services.AddScoped<IMLDataRepository, MLDataRepository>();

            services.AddMvc();
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

            app.UseMvc();
        }

    }
}
