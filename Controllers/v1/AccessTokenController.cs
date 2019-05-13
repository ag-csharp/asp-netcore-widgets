using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WidgetExample.Data;
using WidgetExample.Dto;
using WidgetExample.Entities;

namespace WidgetExample.Controllers.V1 {
  [Authorize(Roles = "Admin")]
  [Route("v1/[controller]")]
  [ApiController]
  public class AccessTokenController : ControllerBase {
    private readonly IWidgetRepo _repo;

    public AccessTokenController(IWidgetRepo repo) {
      _repo = repo;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(AccessTokenDto accessTokenDto) {
      var result = await _repo.AddToken(accessTokenDto);
      return result ? StatusCode(201) : BadRequest();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccessToken>>> ListAll() {
      var list = await _repo.ListTokens();
      return list != null ? (ActionResult<IEnumerable<AccessToken>>) Ok(list) : BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id) {
      var result = await _repo.DeleteToken(id);
      return result ? (IActionResult) Ok() : BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(AccessToken accessToken) {
      var result = await _repo.UpdateToken(accessToken);
      return result != null ? (IActionResult) Ok() : BadRequest();
    }
  }
}