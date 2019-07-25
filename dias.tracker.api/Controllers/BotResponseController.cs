using dias.tracker.api.Data;
using dias.tracker.api.Data.Tables;
using dias.tracker.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dias.tracker.api.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class BotResponseController : ControllerBase {
    private readonly TrackerContext _ctx;

    public BotResponseController(TrackerContext ctx) {
      _ctx = ctx;
    }

    [HttpGet]
    public async Task<IEnumerable<BotResponse>> Get() =>
      await _ctx.BotResponses.ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BotResponseDto r) {
      var (trigger, response, cooldown) = r;

      var resp = new BotResponse {
        triggerText = trigger,
        responses = response,
        cooldown = cooldown
      };

      _ctx.BotResponses.Add(resp);

      await _ctx.SaveChangesAsync();

      return Ok();
    }
  }
}
