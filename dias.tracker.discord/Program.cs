using System;
using System.Threading.Tasks;
using dias.tracker.discord.Commands;
using dias.tracker.discord.Events;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;

namespace dias.tracker.discord {
  public class Program {
    private static CommandsNextModule commands;
    private static DiscordClient discord;
    private static InteractivityModule interactivity;

    public static async Task Main(string[] args) {
      var token = Environment.GetEnvironmentVariable("BOT_TOKEN");

      if (string.IsNullOrEmpty(token)) {
        throw new ArgumentException("The bot needs a token to run!");
      }

      discord = new DiscordClient(new DiscordConfiguration {
        Token = token,
        TokenType = TokenType.Bot,
        UseInternalLogHandler = true,
        LogLevel = LogLevel.Debug,
        AutoReconnect = true,

      });

      commands = discord.UseCommandsNext(new CommandsNextConfiguration {
        StringPrefix = "!!",
        EnableDefaultHelp = true,
        EnableDms = true,
        EnableMentionPrefix = true,
      });

      commands.RegisterCommands<Basic>();
      commands.RegisterCommands<Poll>();

      interactivity = discord.UseInteractivity(new InteractivityConfiguration());

      // events
      discord.Ready += LogEvents.ClientReady;
      discord.Ready += SetGame;
      discord.GuildAvailable += LogEvents.GuildAvailable;
      discord.ClientErrored += LogEvents.ClientError;

      commands.CommandExecuted += LogEvents.CommandExecuted;
      commands.CommandErrored += LogEvents.CommandErrored;

      await discord.ConnectAsync();

      await Task.Delay(-1);
    }

    private static async Task SetGame(ReadyEventArgs e) {
      await discord.UpdateStatusAsync(new DiscordGame($"Cat Herder {DateTime.Now.Year}"));
    }
  }
}
