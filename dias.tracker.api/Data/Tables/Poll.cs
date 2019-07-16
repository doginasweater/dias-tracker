namespace dias.tracker.api.Data.Tables {
  public class Poll : Common {
    public string server { get; set; } = "";
    public string channel { get; set; } = "";
    public string authorDiscordId { get; set; } = "";
    public string name { get; set; } = "";
    public bool active { get; set; } = false;
  }

  public enum PollType {
    YesNo,
    MultipleChoice,

  }
}
