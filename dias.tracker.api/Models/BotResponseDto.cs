using System;

namespace dias.tracker.api.Models {
  public class BotResponseDto {
    public string trigger { get; set; } = "";
    public string response { get; set; } = "";
    public CooldownDto cooldown { get; set; } = new CooldownDto();

    public void Deconstruct(out string trigger, out string response, out TimeSpan cooldown) {
      trigger = this.trigger;
      response = this.response;
      cooldown = new TimeSpan(hours: this.cooldown.hours, minutes: this.cooldown.minutes, seconds: this.cooldown.seconds);
    }
  }

  public class CooldownDto {
    public int hours { get; set; } = 0;
    public int minutes { get; set; } = 1;
    public int seconds { get; set; } = 0;
  }
}
