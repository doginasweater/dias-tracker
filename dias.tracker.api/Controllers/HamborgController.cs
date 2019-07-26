using dias.tracker.api.Data;
using dias.tracker.api.Data.Tables;
using dias.tracker.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace dias.tracker.api.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class HamborgController : ControllerBase {
    private readonly TrackerContext _ctx;

    public HamborgController(TrackerContext ctx) {
      _ctx = ctx;
    }

    // GET api/values
    [HttpGet]
    public async Task<IEnumerable<HamborgDto>> Get() =>
      await _ctx.HamborgText
        .Select(x => new HamborgDto {
          pool = x.pool,
          text = x.text
        })
        .ToListAsync();

    // GET api/values/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Hamborg>> Get(int id) => Ok(await _ctx.HamborgText.SingleOrDefaultAsync(x => x.id == id));

    // POST api/values
    [HttpPost]
    public async Task Post([FromBody] HamborgDto dto) {
      var (pool, text) = dto;

      _ctx.HamborgText.Add(new Hamborg {
        pool = pool,
        text = text
      });

      await _ctx.SaveChangesAsync();
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value) {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id) {
    }
  }
}

