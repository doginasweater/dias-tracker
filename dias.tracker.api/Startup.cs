using dias.tracker.api.Data;
using dias.tracker.api.Data.Tables;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace dias.tracker.api {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddWebEncoders(); // bug in app insights requires this
      services.AddApplicationInsightsTelemetry();

      if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() == "development") {
        services.AddDbContext<TrackerContext>(options =>
          options
            .UseLazyLoadingProxies()
            .UseSqlite(Configuration.GetConnectionString("tracker")));
      } else {
        services.AddEntityFrameworkNpgsql()
          .AddDbContext<TrackerContext>(options =>
            options
              .UseLazyLoadingProxies()
              .UseNpgsql(Configuration.GetConnectionString("tracker")));
      }

      services.AddDefaultIdentity<ApplicationUser>()
        .AddEntityFrameworkStores<TrackerContext>();

      services.AddIdentityServer()
        .AddApiAuthorization<ApplicationUser, TrackerContext>();

      services.AddAuthentication()
        .AddIdentityServerJwt();
        // .AddDiscord(options => {
        //   options.
        // });

      // automatically migrate db on startup
      services
        .BuildServiceProvider()
        .GetService<TrackerContext>()
        .Database
        .Migrate();

      services.AddCors();

      services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      } else {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}
