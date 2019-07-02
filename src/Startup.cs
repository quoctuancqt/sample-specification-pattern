namespace src
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using src.Domain;
    using System.IO;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<TestDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("TestDB")));

            services.AddScoped<UnitOfWork>();

            services.AddTransient<SeedData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            SeedData _seedData)
        {
            if (env.IsDevelopment())
            {
                _seedData.SeedUser();

                app.UseDeveloperExceptionPage();
            }
            else
            {
                //TODO Production
            }

            app.UseMvc();
        }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
        {
            public TestDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                var builder = new DbContextOptionsBuilder<TestDbContext>();

                var connectionString = configuration.GetConnectionString("TestDB");

                builder.UseSqlServer(connectionString);

                return new TestDbContext(builder.Options);
            }
        }
    }
}
