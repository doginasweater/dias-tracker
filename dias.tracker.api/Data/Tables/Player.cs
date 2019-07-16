using System;

namespace dias.tracker.api.Data.Tables {
  public class Player : Common {
    public string discordHandle { get; set; } = "";
    public string characterName { get; set; } = "";
    public string xivApiId { get; set; } = "";
  }
}
