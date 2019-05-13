using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WidgetExample.Entities {
  // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
  public enum UserRole {
    None = 0,
    Admin = 2,
    Tier1 = 4,
    Tier2 = 8
  }
  
  public class User {
    public string UserId { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
    [Column(TypeName = "nvarchar(24)")]
    public string Role { get; set; }

    public UserRole GetUserRole() {
      // Enum.TryParse("Active", out StatusEnum myStatus);
      return Enum.TryParse(Role, out UserRole myEnum) ? myEnum : UserRole.None;
    }
  }
}