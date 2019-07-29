using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using dias.tracker.discord.Commands;
using dias.tracker.dto;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.VoiceNext;
using IdentityModel.Client;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Newtonsoft.Json;

namespace dias.tracker.discord {
  public class Program {
    private static CommandsNextModule commands;
    private static DiscordClient discord;
    private static InteractivityModule interactivity;
    private static VoiceNextClient voice;
    private static TelemetryClient appInsightsClient;
    public static readonly string botname = "dias-tracker-bot";
    private static HttpClient httpClient = new HttpClient(new HttpClientHandler {
      ServerCertificateCustomValidationCallback = (a, b, c, d) => true
    });

    public static DependencyTrackingTelemetryModule InitializeAppInsights(TelemetryConfiguration config) {
      var dependencyModule = new DependencyTrackingTelemetryModule();
      dependencyModule.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
      dependencyModule.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");

      dependencyModule.Initialize(config);

      return dependencyModule;
    }

    public async static Task LoginToAPI() {
      appInsightsClient.TrackTrace("Attempting to connect to dias-tracker api");

      var secret = Environment.GetEnvironmentVariable("DIAS_TRACKER_SECRET");

      var discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");

      if (discovery.IsError) {
        Console.WriteLine($"discovery error: {discovery.Error}");
        appInsightsClient.TrackException(discovery.Exception);
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
        appInsightsClient.TrackException(authToken.Exception);
        return;
      }

      httpClient.SetBearerToken(authToken.AccessToken);

      appInsightsClient.TrackTrace("Connected to dias-tracker api");
    }

    public static async Task Main(string[] args) {
      var aiConfig = TelemetryConfiguration.CreateDefault();
      aiConfig.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());

      QuickPulseTelemetryProcessor? processor = null;

      aiConfig.TelemetryProcessorChainBuilder
        .Use(next => {
          processor = new QuickPulseTelemetryProcessor(next);
          return processor;
        })
        .Build();

      var quickPulse = new QuickPulseTelemetryModule();
      quickPulse.Initialize(aiConfig);
      quickPulse.RegisterTelemetryProcessor(processor);

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
          StringPrefix = "$ ",
          EnableDefaultHelp = true,
          EnableDms = true,
          EnableMentionPrefix = true,
        });

        commands.RegisterCommands<Basic>();
        commands.RegisterCommands<Poll>();
        commands.RegisterCommands<Hamborg>();
        commands.RegisterCommands<Voice>();

        interactivity = discord.UseInteractivity(new InteractivityConfiguration());

        voice = discord.UseVoiceNext();

        // events
        discord.Ready += ClientReady;
        discord.Ready += SetGame;
        discord.GuildAvailable += GuildAvailable;
        discord.ClientErrored += ClientError;
        discord.MessageCreated += SlackbotResponse;

        commands.CommandExecuted += CommandExecuted;
        commands.CommandErrored += CommandErrored;

        await discord.ConnectAsync();

        appInsightsClient.TrackTrace("Connected to Discord");

        // await LoginToAPI();

        await Task.Delay(-1);
      }
    }

    private static async Task SlackbotResponse(MessageCreateEventArgs e) {
      // if (e.Message.Content != "hamburger test") {
      //   return;
      // }

      // using var msgResponse = await httpClient.GetAsync("https://localhost:5001/api/Hamborg");

      // if (!msgResponse.IsSuccessStatusCode) {
      //   appInsightsClient.TrackTrace($"API Error {msgResponse.StatusCode} {msgResponse.ReasonPhrase}");
      //   return;
      // }

      // var msgString = await msgResponse.Content.ReadAsStringAsync();

      // var messages = JsonConvert.DeserializeObject<List<HamborgDto>>(msgString);

      // await e.Message.RespondAsync(messages[0].text);

      if (e.Message.Content.ToLower() == "i am your dad" || e.Message.Content.ToLower() == "i'm your dad") {
        await e.Message.RespondAsync("i'm your dad! (boogie woogie woogie)");
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
