using dias.tracker.api.Data;
using dias.tracker.api.Data.Tables;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Claims;

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
        .AddApiAuthorization<ApplicationUser, TrackerContext>(options => {
          options.Clients.Add(new Client {
            ClientId = "dias.tracker.discord",
            ClientName = "Tiro Finale Tracker",
            ClientSecrets = { new Secret(Configuration["DiasTrackerDiscord:ClientSecret"].Sha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = { "dias.tracker.apiAPI" }
          });
        });

      services.AddAuthentication()
        .AddIdentityServerJwt()
        .AddDiscord(options => {
          options.ClientId = Configuration["Discord:ClientId"];
          options.ClientSecret = Configuration["Discord:ClientSecret"];
          options.Scope.Add("email");
          options.Scope.Add("guilds");
          options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
          options.SaveTokens = true;
        });

      // automatically migrate db on startup
      services
        .BuildServiceProvider()
        .GetService<TrackerContext>()
        .Database
        .Migrate();

      services.AddMvc(options => options.EnableEndpointRouting = false);
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
      app.UseStaticFiles();
      app.UseIdentityServer();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseMvcWithDefaultRoute();
    }
  }
}
