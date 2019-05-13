using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WidgetExample.Data;
using WidgetExample.Entities;

namespace WidgetExample.Helpers {
  internal class TokenDetails {
    public string Id { get; }
    public string Email { get; }
    public string Role { get; }

    public TokenDetails() {
      Id    = "";
      Email = "";
      Role  = "";
    }

    public TokenDetails(string id, string email, string role) {
      Id = id;
      Email = email;
      Role = role;
    }

    public bool isValidUser() {
      return Id != "" && Email != "" && Role != "";
    }
  }

// ReSharper disable once ClassNeverInstantiated.Global
  public class RoleMiddleware {
    private readonly ILogger _logger;
    private readonly IConfiguration _conf;
    
    private readonly RequestDelegate _next;

    // ReSharper disable once SuggestBaseTypeForParameter
    public RoleMiddleware(RequestDelegate n, IConfiguration c, ILogger<RoleMiddleware> l) {
      _conf = c;
      _logger = l;
      _next = n;
    }

    // ReSharper disable once UnusedMember.Global
    public Task Invoke(HttpContext httpContext, DataContext d) {
      string authHeader = httpContext.Request.Headers["Authorization"];
      if (string.IsNullOrEmpty(authHeader)) {
        return _next(httpContext);
      }
      
      var authToken = authHeader.Replace("Bearer ", "");
      var isAuthRequired = _RequiresAuth(httpContext.Request.Path);
      if (isAuthRequired)
        return _next(httpContext);
      
      var td = _deCodeToken(authToken);
      var user = d.Users.FirstOrDefault(u => u.UserId == td.Id);
      if (user == null) {
        return _next(httpContext);
      }
      
      _logger.LogDebug($"User = {user.Role}\nUser Role = {user.GetUserRole()}");
      _logger.LogDebug($"Id: {td.Id}\tEmail: {td.Email}\tRole: {td.Role}");

      return _next(httpContext);
    }

    /// <summary>
    /// Takes the JWT and decodes it.
    /// This means I can search for a user by Id and the route
    /// require Authorization then I can tack the user onto the Request
    /// </summary>
    /// <param name="authHeader"></param>
    /// <returns>TokenDetails</returns>
    private TokenDetails _deCodeToken(string authHeader) {
      var handler   = new JwtSecurityTokenHandler();
      if (!(handler.ReadToken(authHeader) is JwtSecurityToken tokenS))
        return new TokenDetails();
      
      var id = tokenS.Claims.First(claim => claim.Type == "nameid").Value;
      var email = tokenS.Claims.First(claim => claim.Type == "email").Value;
      var role = tokenS.Claims.First(claim => claim.Type == "role").Value;
      _logger.LogDebug($"id: {id}\nemail: {email}\nrole: {role}");
      
      return new TokenDetails(id, email, role);
    }

    /// <summary>
    /// Checks to see if the url path requires Authorization
    /// This is because I can't work out how to use Role Authorization
    /// </summary>
    /// <param name="path"></param>
    /// <returns>bool</returns>
    private bool _RequiresAuth(string path) {
      var list = _conf.GetSection("AppSettings:Urls").Get<string[]>();
      var result = list?.FirstOrDefault(p => p.Equals(path));
      return result != path;
    }
  }

  public static class RoleMiddlewareExtension {
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IApplicationBuilder UseRoleMiddleware(this IApplicationBuilder builder) {
      return builder.UseMiddleware<RoleMiddleware>();
    }
  }
}