namespace dias.tracker.api.Data.Tables {
  public class PlayerData : Common {
    public int playerId { get; set; }
    public virtual Player? player { get; set; }
  }
}
