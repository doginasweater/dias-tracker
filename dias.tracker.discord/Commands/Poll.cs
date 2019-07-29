using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;

namespace dias.tracker.discord.Commands {
  public class Poll {
    [Command("poll"), Description("Create a poll using reactions")]
    public async Task RunPoll(
      CommandContext ctx,
      [Description("How long should the poll last?")] TimeSpan duration,
      [Description("What options should people have?")] params DiscordEmoji[] options
    ) {
      await ctx.RespondAsync($"duration days: {duration.Days} hours: {duration.Hours} minutes: {duration.Minutes} seconds: {duration.Seconds}");

      var interactivity = ctx.Client.GetInteractivityModule();

      var poll_options = options.Select(xe => xe.ToString());

      // then let's present the poll
      var embed = new DiscordEmbedBuilder {
        Title = "POLL TIME",
        Description = string.Join(" ", poll_options)
      };

      var msg = await ctx.RespondAsync(embed: embed);

      // add the options as reactions
      foreach (var option in options) {
        await msg.CreateReactionAsync(option);
      }

      // collect and filter responses
      var poll_result = await interactivity.CollectReactionsAsync(msg, duration);

      var results = poll_result.Reactions
        .Where(xkvp => options.Contains(xkvp.Key))
        .Select(xkvp => $"{xkvp.Key}: {xkvp.Value}");

      // and finally post the results
      await ctx.RespondAsync(string.Join("\n", results));
    }
  }
}
