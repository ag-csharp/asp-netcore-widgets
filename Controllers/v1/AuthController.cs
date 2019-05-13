using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WidgetExample.Data;
using WidgetExample.Dto;
using WidgetExample.Entities;

namespace WidgetExample.Controllers.V1 {
  [Route("v1/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase {
    private readonly IAuthRepo _repo;
    private readonly IConfiguration  _config;
    
    public AuthController(IAuthRepo repo, IConfiguration config)
    {
      _config = config;
      _repo   = repo;
    }

    [Authorize(Roles = "Administrator")]
    [HttpDelete("delete/{uuid}")]
    public async Task<IActionResult> DeleteUser(string uuid) {
      var userDeleteSuccess = await _repo.DeleteUser(uuid);

      return !userDeleteSuccess ? (IActionResult) BadRequest() : Ok(true);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto) {
      var userFromRepo = await _repo.Login(userLoginDto);
      if (userFromRepo == null) {
        return Unauthorized("Invalid Email or Password");
      }
      
      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, userFromRepo.UserId),
        new Claim(ClaimTypes.Email, userFromRepo.Email),
        new Claim(ClaimTypes.Role, userFromRepo.Role) 
      };
      
      var key = new SymmetricSecurityKey(Encoding.UTF8
                                                 .GetBytes(_config.GetSection("AppSettings:Token").Value));

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject            = new ClaimsIdentity(claims),
        Expires            = DateTime.Now.AddDays(1),
        SigningCredentials = creds
      };

      var tokenHandler = new JwtSecurityTokenHandler();

      var token = tokenHandler.CreateToken(tokenDescriptor);

      return Ok(new {
        token = tokenHandler.WriteToken(token),
        uuid = userFromRepo.UserId
      });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto userReg) {
      if (await _repo.UserExists(userReg.Email)) {
        return BadRequest("Disallowed Email Address");
      }

      var userToCreate = new User {Email = userReg.Email};
      return await _repo.Register(userToCreate, userReg.Password) == null
        ? (IActionResult) BadRequest("Register Error")
        : Ok();
    }
  }
}