using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WidgetExample.Dto;
using WidgetExample.Entities;

namespace WidgetExample.Data {
  public class AuthRepo : IAuthRepo {
    private readonly DataContext _context;

    public AuthRepo(DataContext context) {
      _context = context;
    }

    public async Task<bool> DeleteUser(string uuid) {
      var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == uuid);
      if (user == null) {
        return false;
      }

      _context.Users.Remove(user);
      await _context.SaveChangesAsync();
      return true;
    }
    
    public async Task<User> Login(UserLoginDto userLogin) {
      var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userLogin.Email);

      if (user == null)
        return null;
      
      return !_VerifyPasswordHash(userLogin.Password, user.PasswordHash, user.PasswordSalt) ? null : user;
    }
    
    public async Task<User> Register(User user, string password) {
      if (await UserExists(user.Email)) {
        return null;
      }
      
      _CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

      user.UserId = Guid.NewGuid().ToString();
      user.PasswordHash = passwordHash;
      user.PasswordSalt = passwordSalt;
      user.Role = UserRole.Tier1.ToString();

      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();

      return user;
    }

    public async Task<bool> UserExists(string email) {
      return await _context.Users.AnyAsync(x => x.Email == email);
    }
    
    private static void _CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
            
    }
    
    private bool _VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
      {
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        for (var i = 0; i < computedHash.Length; i++)
        {
          if (computedHash[i] != passwordHash[i]) return false;
        }
        return true;
      }
    }
  }
}