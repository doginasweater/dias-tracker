using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace dias.tracker.discord.Commands {
  public class Hamborg {
    private static Random _random = new Random();
    private List<List<string>> pools = new List<List<string>> {
      new List<string> {
        "The hamburger is wet, the hamburger is in fact the social construct of wetness, which is to say that it is soaked simultaneously in every liquid imaginable, and yet none at all.",
        "The hamburger has never read the works of Joan Didion, but the hamburger has a great many opinions on social fragmentation.",
        "The hamburger shrieks silence from the cavernous arteries in the blood-pump of creation; the hamburger takes your order for another hamburger, and disregards it.",
        "The hamburger has legs. So many legs. You try to count the legs, but they are uncountable. They are unknowable. They are infinite and unceasing.",
        "The hamburger harmonizes exquisitely with the universal chorus.",
        "The hamburger is you. You remember the feeling of being yourself, but the hamburger does not remind you of this feeling. The hamburger reminds you instead of how that feeling might have felt, had you known yourself at all.",
        "Beneath the bun of the hamburger, which is not a bun but the inverted roof of your childhood home, there is a woman dancing in a circle.",
        "There are condiments on the hamburger. The condiments are loneliness, sadness, empathy, and the singular sensation that you will never truly know the complex interior world of the woman behind the counter at the corner store.",
        "You hold the hamburger. Inside your belly blooms the unshakeable certainty that you have held this hamburger before, but you have not. You have never held anything.",
        "The hamburger is a rare medium. Through it you can feel the pull of the departed, whispering on the edges of your hearing. They are beseeching you, pleading for your understanding.",
        "The hamburger does not have enough legs.",
        "The hamburger speaks to you; it recites romantic poetry to you, in a voice so much like your first love that you find yourself crying. The tears only make the hamburger stronger.",
        "Within the hamburger you can feel the resonance of the deepest secrets of existence. You are not ready to know them.",
        "How did we arrive here? You did not order a hamburger. You are screaming. You did not order a hamburger.",
        "You consider the shape and texture of the hamburger. You consider that you, too, are mostly made of soft and yielding meat. There are children playing in the distance. The children are also made of meat.",
        "This is the hamburger to end all other hamburgers. This is the hamburger to begin all other hamburgers.",
        "There are fifteen sides to the hamburger. You count them. You do not know how a hamburger can have this many sides, or why the sides are so flat and precise.",
        "It is merely a drawing of a hamburger. The paper is greasy. The paper melts in your hands.",
        "You give the hamburger back. You give the hamburger back. You give the hamburger back.",
        "The hamburger has a mind of its own, but it does not know this yet. You can sense the hamburger's thoughts. They terrify you.",
      },
      new List<string> {
        "From beyond the walls of the room you hear your mother calling you. She calls a name that is not yours, but might have been yours, if you had been born as someone else.",
        "You feel the earth beneath your feet begin to pulse in time with your own heartbeat. Your heartbeat is frantic. The earth is frantic. You are frantic.",
        "You smell the hamburger. The smell, you decide, is adequate.",
        "\"Is this what disappointment feels like?\" You wonder out loud. No, it is not. This is what a hamburger feels like. A hamburger is not the same as disappointment.",
        "You take one step out of your front door, holding the hamburger. You take one more step. You take another. You continue to take steps out the door, but the door does not move, and neither do you.",
        "This is the time. This is the place. This is where you take a stand against the universe. You can feel the universe standing nearby, wondering what your next move might be.",
        "As you lift the hamburger to your mouth you feel your skin unravelling in uncountable marvellous satin ribbons, furling into the darkness in a rare billowing bloom of flesh and blood.",
        "You are surprised to find that you are no longer hungry. You are thoughtful. You are alone.",
        "You clench together the disparate pieces of your mouth parts and prepare yourself for the feast of an eternity. You have been asleep for so long, but now you are awake. And oh, what terrors await.",
        "The chitinous legs of the hamburger claw desperately at the edges of your mouth as you attempt to consume the meat. You are unfazed by these legs. You are unfazed by the screaming.",
        "As the darkness closes in you realize that this is not your house. You realize that this is not your hamburger. Whose child is this?",
        "The doctor has ordered that you eat the hamburger. Which doctor? How did you arrive in this hospital? There are no lights in the hospital. There are no lights anywhere.",
        "Perhaps the hamburger is not the answer you were looking for. You wonder why that old woman is staring at you.",
        "The sprouts beneath the bun of the hamburger have already taken root in your palms. You begin to see the thoughts of plants. Your veins become sunlight. Your hair becomes leaves.",
        "If you bite the hamburger it will all be over. If you bite the hamburger you can go back. If you bite the hamburger it will all be alright.",
        "Time stretches out in a long taffy pull. You feel the seconds tick by like centuries. You have forgotten to turn off the stove.",
        "Every locked door in your mind flies open. You are faced with the horror of yourself. You cannot hide.",
        "The hamburger is filled with birdsong. The hamburger is filled with the laughter of children. The hamburger has no place inside you; you must relinquish the hamburger, but you cannot.",
        "The hamburger speaks to you without making any sound. You have no ears. Who is speaking?",
        "Nobody has seen a hamburger in over four hundred years. You are certain that this is a hamburger, but you have no evidence of this.",
      },
    };

    [Command("give")]
    [Description("I give you a hamburger.")]
    public async Task Hamburger(CommandContext ctx,
      [Description("You must ask for a hamburger.")]
      [RemainingText]
      string whatIsRequested
    ) {
      if (string.IsNullOrEmpty(whatIsRequested) || !whatIsRequested.Contains("me a hamburger")) {
        await ctx.RespondAsync($"What would you like me to give, {ctx.User.Mention}?");
        return;
      }

      var numPools = _random.Next(0, pools.Count);

      await ctx.RespondAsync("I give you a hamburger.");

      for (var i = 0; i <= numPools; i++) {
        var delay = _random.Next(2, 7) * 1000;

        if (i > 0 && i % 2 == 0) {
          await ctx.TriggerTypingAsync();
          await Task.Delay(delay);

          await ctx.RespondAsync("I give you a hamburger.");

          delay = _random.Next(2, 7) * 1000;
        }

        await ctx.TriggerTypingAsync();
        await Task.Delay(delay);

        await ctx.RespondAsync(GetString(i));
      }
    }

    public string GetString(int pool) => pools[pool][_random.Next(0, pools[pool].Count)];
  }
}
