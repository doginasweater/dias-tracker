using System;
using dias.tracker.api.Models;

namespace dias.tracker.api.Data.Tables {
  public class BotResponse : Common {
    public string triggerText { get; set; } = "";
    public TimeSpan cooldown { get; set; } = new TimeSpan(hours: 0, minutes: 1, seconds: 0);
    public string responses { get; set; } = "";
  }
}
