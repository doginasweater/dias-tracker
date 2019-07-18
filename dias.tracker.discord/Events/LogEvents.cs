using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace dias.tracker.discord.Events {
  public static class LogEvents {
    public static string botname { get; set; } = "dias-tracker-bot";

    public static Task ClientReady(ReadyEventArgs e) {
      e.Client.DebugLogger.LogMessage(
        LogLevel.Info,
        botname,
        "Client is ready to process events",
        DateTime.Now
      );

      return Task.CompletedTask;
    }

    public static Task GuildAvailable(GuildCreateEventArgs e) {
      e.Client.DebugLogger.LogMessage(
        LogLevel.Info,
        botname,
        $"Guild available: {e.Guild.Name}",
        DateTime.Now
      );

      return Task.CompletedTask;
    }

    public static Task ClientError(ClientErrorEventArgs e) {
      e.Client.DebugLogger.LogMessage(
        LogLevel.Error,
        botname,
        $"Exception occurred: {e.Exception.GetType()}: {e.Exception.Message}",
        DateTime.Now
      );

      return Task.CompletedTask;
    }

    public static Task CommandExecuted(CommandExecutionEventArgs e) {
      // let's log the name of the command and user
      e.Context.Client.DebugLogger.LogMessage(
        LogLevel.Info,
        botname,
        $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'",
        DateTime.Now
      );

      return Task.CompletedTask;
    }

    public static async Task CommandErrored(CommandErrorEventArgs e) {
      // let's log the error details
      e.Context.Client.DebugLogger.LogMessage(
        LogLevel.Error,
        botname,
        $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}",
        DateTime.Now
      );

      // let's check if the error is a result of lack
      // of required permissions
      if (e.Exception is ChecksFailedException) {
        // yes, the user lacks required permissions,
        // let them know

        var emoji = DiscordEmoji.FromName(e.Context.Client, ":no_entry:");

        // let's wrap the response into an embed
        var embed = new DiscordEmbedBuilder {
          Title = "Access denied",
          Description = $"{emoji} You do not have the permissions required to execute this command.",
          Color = new DiscordColor(0xFF0000) // red
        };

        await e.Context.RespondAsync("", embed: embed);
      }
    }
  }
}
