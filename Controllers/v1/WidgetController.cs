using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WidgetExample.Data;

namespace WidgetExample.Controllers.V1 {
  [Authorize(Roles = "Admin")]
  [Route("v1/[controller]")]
  [ApiController]
  public class WidgetController : ControllerBase {
    private readonly IWidgetRepo      _repo;
    
    public WidgetController(IWidgetRepo repo)
    {
      _repo   = repo;
    }
    
    [HttpGet("claims")]
    public async Task<IActionResult> GetClaims() {
      List<Object> list = new List<Object>();
      var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      Console.WriteLine($"User = {user}\nType = {user.GetType()}");

      foreach (var claim in HttpContext.User.Claims) {
        var res = new { type = claim.Type, value = claim.Value };
        list.Add(res);
      }

      return Ok(list);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWidgets() {
      var repo = await _repo.GetAll();
      return Ok(repo);
    }
  }
}