using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace dias.tracker.api.Controllers {
  [Route("identity")]
  [Authorize]
  public class IdentityController : ControllerBase {
    [HttpGet]
    public IActionResult Get() => Ok(User.Claims.Select(x => new { x.Type, x.Value }).ToList());
  }
}
