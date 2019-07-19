using dias.tracker.api.Data;
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
      if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() == "production") {
        services.AddEntityFrameworkNpgsql()
          .AddDbContext<TrackerContext>(options =>
            options
              .UseLazyLoadingProxies()
              .UseNpgsql(Configuration.GetConnectionString("tracker")));
      } else {
        services.AddDbContext<TrackerContext>(options =>
          options
            .UseLazyLoadingProxies()
            .UseSqlite(Configuration.GetConnectionString("tracker")));
      }

      services.AddWebEncoders(); // bug in app insights requires this
      services.AddApplicationInsightsTelemetry();

      services.AddCors();

      services.AddControllers();

      // services.AddAuthorization();

      // services
      //   .AddAuthentication("Bearer")
      //   .AddJwtBearer("Bearer", options => {
      //     options.Authority = "http://localhost:5000";
      //     options.RequireHttpsMetadata = false;
      //     options.Audience = "api1";
      //   })
      //   .AddDiscord(options => {
      //   });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      } else {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseCors(builder =>
        builder.WithOrigins(

        ));

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
