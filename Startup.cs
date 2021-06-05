using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestForGraduates.Data;
using TestForGraduates.Data.Repository;
using TestForGraduates.Interfaces;
using TestForGraduates.Models;
using TestForGraduates.Services;

namespace TestForGraduates
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectStr = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectStr));
            services.AddScoped<UserRepository>();
            services.AddTransient<IMessageEmailService, EmailService>();

            services.AddIdentity<User, IdentityRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
            })
                .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
