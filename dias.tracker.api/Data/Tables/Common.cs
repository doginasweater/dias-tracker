using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dias.tracker.api.Data.Tables {
  public abstract class Common {
    [Key]
    public int id { get; set; }

    public DateTime created { get; set; }
    public string createdBy { get; set; } = "";

    public DateTime modified { get; set; }
    public string modifiedBy { get; set; } = "";


    public bool isSuppressed { get; set; } = false;
  }
}
