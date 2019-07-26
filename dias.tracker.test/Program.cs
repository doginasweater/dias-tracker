using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace dias.tracker.test {
  class Program {
    private static HttpClient httpClient = new HttpClient(new HttpClientHandler {
      ServerCertificateCustomValidationCallback = (a, b, c, d) => true
    });

    public static async Task Main(string[] args) {
      await LoginToAPI();

      var data = await httpClient.GetAsync("https://localhost:5001/api/Hamborg/1");

      if (!data.IsSuccessStatusCode) {
        Console.WriteLine($"error: {data.StatusCode}");
      }

      var dataString = await data.Content.ReadAsStringAsync();

      Console.WriteLine(dataString);

      Console.ReadKey();
    }

    public async static Task LoginToAPI() {
      var secret = Environment.GetEnvironmentVariable("DIAS_TRACKER_SECRET");

      Console.WriteLine($"the secret is: {secret}");

      var discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");

      if (discovery.IsError) {
        Console.WriteLine($"discovery error: {discovery.Error}");
        return;
      }

      var authToken = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest {
        Address = discovery.TokenEndpoint,
        ClientId = "dias.tracker.discord",
        Scope = "dias.tracker.apiAPI",
        ClientSecret = secret,
      });

      if (authToken.IsError) {
        Console.WriteLine($"Auth error: {authToken.Error}");
        Console.WriteLine($"Error description: {authToken.ErrorDescription}");
        return;
      }

      Console.WriteLine(authToken.Json);

      httpClient.SetBearerToken(authToken.AccessToken);
    }
  }
}
