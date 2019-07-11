using IdentityServer4.Models;
using System.Collections.Generic;

namespace dias.tracker.auth
{
  public static class Config
  {
    public static IEnumerable<IdentityResource> GetIdentityResources() =>
      new IdentityResource[] {
        new IdentityResources.OpenId()
      };

    public static IEnumerable<ApiResource> GetApis() =>
      new List<ApiResource> {
        new ApiResource("api1", "My API")
      };

    public static IEnumerable<Client> GetClients() =>
      new List<Client> {
        new Client {
          ClientId = "client",

          // no interactive user, use the clientid/secret for authentication
          AllowedGrantTypes = GrantTypes.ClientCredentials,

          // secret for authentication
          ClientSecrets =
          {
              new Secret("secret".Sha256())
          },

          // scopes that client has access to
          AllowedScopes = { "api1" }
        }
    };
  }
}
