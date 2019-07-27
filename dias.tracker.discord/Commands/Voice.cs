using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.VoiceNext;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace dias.tracker.discord.Commands {
  public class Voice {
    [Command("join")]
    public async Task Join(CommandContext ctx) {
      var vnext = ctx.Client.GetVoiceNextClient();
      var vnc = vnext.GetConnection(ctx.Guild);

      if (vnc != null) {
        throw new InvalidOperationException("Already connected in this guild");
      }

      var channel = ctx.Member?.VoiceState?.Channel;

      if (channel is null) {
        throw new InvalidOperationException("You need to be in a voice channel");
      }

      vnc = await vnext.ConnectAsync(channel);
      await ctx.RespondAsync("👌");
    }

    [Command("leave")]
    public async Task Leave(CommandContext ctx) {
      var vnext = ctx.Client.GetVoiceNextClient();

      var vnc = vnext.GetConnection(ctx.Guild);
      if (vnc is null) {
        throw new InvalidOperationException("Not connected in this guild.");
      }

      vnc.Disconnect();
      await ctx.RespondAsync("👌");
    }

    [Command("screech")]
    public async Task Screech(CommandContext ctx) {
      var file = "./aki.mp3";

      var vnext = ctx.Client.GetVoiceNextClient();

      var vnc = vnext.GetConnection(ctx.Guild);

      if (vnc is null) {
        throw new InvalidOperationException("Not connected in this guild.");
      }

      if (!File.Exists(file)) {
        throw new FileNotFoundException("File was not found.");
      }

      await ctx.RespondAsync("👌");
      await vnc.SendSpeakingAsync(true); // send a speaking indicator

      var psi = new ProcessStartInfo {
        FileName = "ffmpeg",
        Arguments = $@"-i ""{file}"" -ac 2 -f s16le -ar 48000 pipe:1",
        RedirectStandardOutput = true,
        UseShellExecute = false
      };

      var ffmpeg = Process.Start(psi);
      var ffout = ffmpeg.StandardOutput.BaseStream;

      var buff = new byte[3840];
      var br = 0;
      while ((br = ffout.Read(buff, 0, buff.Length)) > 0) {
        if (br < buff.Length) {// not a full sample, mute the rest
          for (var i = br; i < buff.Length; i++) {
            buff[i] = 0;
          }
        }

        await vnc.SendAsync(buff, 20);
      }

      await vnc.SendSpeakingAsync(false); // we're not speaking anymore
    }
  }
}
