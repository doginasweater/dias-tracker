using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dias.tracker.discord.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;

namespace dias.tracker.discord {
  public class Program {
    private static CommandsNextModule commands;
    private static DiscordClient discord;
    private static InteractivityModule interactivity;
    private static TelemetryClient appInsightsClient;
    public static readonly string botname = "dias-tracker-bot";

    public static DependencyTrackingTelemetryModule InitializeAppInsights(TelemetryConfiguration config) {
      var dependencyModule = new DependencyTrackingTelemetryModule();
      dependencyModule.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
      dependencyModule.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");

      dependencyModule.Initialize(config);

      // QuickPulseTelemetryProcessor? processor = null;

      // appInsightsConfig.TelemetryProcessorChainBuilder
      //   .Use(next => {
      //     processor = new QuickPulseTelemetryProcessor(next);
      //     return processor;
      //   })
      //   .Build();

      // var quickPulse = new QuickPulseTelemetryModule();
      // quickPulse.Initialize(appInsightsConfig);
      // quickPulse.RegisterTelemetryProcessor(processor);

      return dependencyModule;
    }

    public static async Task Main(string[] args) {
      var aiConfig = TelemetryConfiguration.CreateDefault();
      aiConfig.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());

      appInsightsClient = new TelemetryClient(aiConfig);

      using (InitializeAppInsights(aiConfig)) {
        var token = Environment.GetEnvironmentVariable("BOT_TOKEN");

        if (string.IsNullOrEmpty(token)) {
          throw new ArgumentException("The bot needs a token to run!");
        }

        appInsightsClient.TrackTrace("Token found. Beginning boot.");

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
        commands.RegisterCommands<Hamborg>();

        interactivity = discord.UseInteractivity(new InteractivityConfiguration());

        // events
        discord.Ready += ClientReady;
        discord.Ready += SetGame;
        discord.GuildAvailable += GuildAvailable;
        discord.ClientErrored += ClientError;

        commands.CommandExecuted += CommandExecuted;
        commands.CommandErrored += CommandErrored;

        await discord.ConnectAsync();

        appInsightsClient.TrackTrace("Connected");

        await Task.Delay(-1);
      }
    }

    private static async Task SetGame(ReadyEventArgs e) {
      await discord.UpdateStatusAsync(new DiscordGame($"Cat Herder {DateTime.Now.Year}"));
    }

    public static Task ClientReady(ReadyEventArgs e) {
      e.Client.DebugLogger.LogMessage(
        LogLevel.Info,
        botname,
        "Client is ready to process events",
        DateTime.Now
      );

      appInsightsClient.TrackEvent("Client ready");

      return Task.CompletedTask;
    }

    public static Task GuildAvailable(GuildCreateEventArgs e) {
      e.Client.DebugLogger.LogMessage(
        LogLevel.Info,
        botname,
        $"Guild available: {e.Guild.Name}",
        DateTime.Now
      );

      appInsightsClient.TrackEvent($"Guild available: {e.Guild.Name}");

      return Task.CompletedTask;
    }

    public static Task ClientError(ClientErrorEventArgs e) {
      e.Client.DebugLogger.LogMessage(
        LogLevel.Error,
        botname,
        $"Exception occurred: {e.Exception.GetType()}: {e.Exception.Message}",
        DateTime.Now
      );

      appInsightsClient.TrackException(e.Exception);

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

      appInsightsClient.TrackEvent("Command executed", new Dictionary<string, string> {
        { "User", e.Context.User.Username },
        { "Command", e.Command.QualifiedName }
      });

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

      appInsightsClient.TrackException(e.Exception, new Dictionary<string, string> {
        { "User", e.Context.User.Username },
        { "Command", e.Command?.QualifiedName ?? "<unknown command>" }
      });

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
