using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace dias.tracker.auth
{
  public class Startup {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
      services.AddMvc();

      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

      services
        .AddIdentityServer()
        .AddInMemoryIdentityResources(Config.GetIdentityResources())
        .AddInMemoryApiResources(Config.GetApis())
        .AddInMemoryClients(Config.GetClients())
        .AddDeveloperSigningCredential();

      services
        .AddAuthentication(options => {
          options.DefaultScheme = "Cookies";
          options.DefaultChallengeScheme = "oidc";
        })
        .AddCookie("Cookies")
        .AddOpenIdConnect("oidc", options => {
          options.Authority = "http://localhost:5000";
          options.RequireHttpsMetadata = false;
          options.ClientId = "mvc";
          options.SaveTokens = true;
        })
        .AddDiscord(options => {
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseStaticFiles();

      app.UseIdentityServer();

      app.UseAuthentication();

      app.UseRouting();

      app.UseEndpoints(endpoints => {
        endpoints.MapGet("/", async context => {
          await context.Response.WriteAsync("Hello World!");
        });
      });
    }
  }
}
