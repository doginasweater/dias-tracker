using System;
using System.Threading.Tasks;
using DSharpPlus;

namespace dias.tracker.discord {
  public class Program {
    private static DiscordClient discord;

    public static async Task Main(string[] args) {
      discord = new DiscordClient(new DiscordConfiguration {
        Token = "",
        TokenType = TokenType.Bot
      });

      discord.MessageCreated += async e => {
        Console.WriteLine($"Received message: {e.Message.Content}");
        if (e.Message.Content.ToLower().StartsWith("!ping")) {
          await e.Message.RespondAsync("pong!");
        }
      };

      await discord.ConnectAsync();

      Console.WriteLine("Connected");

      await Task.Delay(-1);
    }
  }
}
