using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;

namespace dias.tracker.discord.Commands {
  public class Basic {
    [Command("hi")]
    [Description("Say hi!")]
    public async Task Hi(CommandContext ctx) {
      await ctx.RespondAsync($"sup, {ctx.User.Mention}?");

      var interactivity = ctx.Client.GetInteractivityModule();
      var msg = await interactivity.WaitForMessageAsync(
        x => x.Author.Id == ctx.User.Id && x.Content.ToLower().Contains("how are you"),
        TimeSpan.FromMinutes(1)
      );

      if (msg != null) {
        await ctx.RespondAsync("i'm fine, thanks!");
      }
    }

    [Command("ping")]
    [Description("ping? pong!")]
    public async Task Ping(CommandContext ctx) {
      await ctx.TriggerTypingAsync();

      var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");

      await ctx.RespondAsync($"{emoji} pong! Ping: {ctx.Client.Ping}ms");
    }

    [Command("navyseal")]
    [Aliases("gorillawarfare")]
    [Description("what the fuck did you just say to me?")]
    public async Task NavySeal(CommandContext ctx) {
      await ctx.TriggerTypingAsync();
      await ctx.RespondAsync(
        "What the fuck did you just fucking say about me, you little bitch? I’ll have you know I graduated top " +
        "of my class in the Navy Seals, and I’ve been involved in numerous secret raids on Al-Quaeda, and I have over " +
        "300 confirmed kills. I am trained in gorilla warfare and I’m the top sniper in the entire US armed forces. You " +
        "are nothing to me but just another target. I will wipe you the fuck out with precision the likes of which has " +
        "never been seen before on this Earth, mark my fucking words. You think you can get away with saying that shit " +
        "to me over the Internet? Think again, fucker. As we speak I am contacting my secret network of spies across the " +
        "USA and your IP is being traced right now so you better prepare for the storm, maggot. The storm that wipes out " +
        "the pathetic little thing you call your life. You’re fucking dead, kid. I can be anywhere, anytime, and I can " +
        "kill you in over seven hundred ways, and that’s just with my bare hands. Not only am I extensively trained in " +
        "unarmed combat, but I have access to the entire arsenal of the United States Marine Corps and I will use it to " +
        "its full extent to wipe your miserable ass off the face of the continent, you little shit. If only you could " +
        "have known what unholy retribution your little “clever” comment was about to bring down upon you, maybe you would " +
        "have held your fucking tongue. But you couldn’t, you didn’t, and now you’re paying the price, you goddamn idiot. " +
        "I will shit fury all over you and you will drown in it. You’re fucking dead, kiddo."
      );
    }

    [Command("youcant")]
    [Hidden]
    public async Task YouCant(CommandContext ctx) {
      await ctx.TriggerTypingAsync();
      await ctx.RespondAsync(
        "You can't. \n\n" +
        "Seriously, new moderators are only chosen a few times a year, and the minimum requirements for applying are very " +
        "steep. Even so, hundreds apply, but only a few are selected. If you seriously want to be a moderator, you'll likely " +
        "(but not always) need to have a history on GameFAQs that makes you recognizable to the majority of the user-base for " +
        "several years to even be considered. Pestering the current moderators or admins to become a moderator essentially " +
        "guarantees that you won't be selected."
      );
    }
  }
}
