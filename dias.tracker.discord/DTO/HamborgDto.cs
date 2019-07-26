namespace dias.tracker.discord.DTO {
  public class HamborgDto {
    public int pool { get; set; }
    public string text { get; set; }

    public void Deconstruct(out int pool, out string text) {
      pool = this.pool;
      text = this.text;
    }
  }
}
