using System;

namespace dias.tracker.api.Data.Tables {
  public class XivJob : Common {
    public string name { get; set; } = "";
    public XivRole role { get; set; }
  }

  public enum XivRole {
    Tank,
    Healer,
    Melee,
    Ranged,
    Magic
  }
}
