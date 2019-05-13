using System.Threading.Tasks;
using WidgetExample.Dto;
using WidgetExample.Entities;

namespace WidgetExample.Data {
  public interface IAuthRepo {
    Task<bool> DeleteUser(string uuid);
    Task<User> Register(User user, string password);
    Task<User> Login(UserLoginDto userLogin);
    Task<bool> UserExists(string username);
  }
}