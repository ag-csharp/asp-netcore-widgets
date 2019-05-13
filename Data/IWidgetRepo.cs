using System.Collections.Generic;
using System.Threading.Tasks;
using WidgetExample.Dto;
using WidgetExample.Entities;

namespace WidgetExample.Data {
  public interface IWidgetRepo {
    Task<bool> AddToken(AccessTokenDto accessTokenDto);
    Task<IEnumerable<AccessToken>> ListTokens();
    Task<bool> DeleteToken(int id);
    Task<AccessToken> UpdateToken(AccessToken accessToken);

    Task<Widget> Add(Widget widget);
    Task<IEnumerable<Widget>> GetAll();
    Task<Widget> Get(string id);
    Task<bool> Remove(string id);
    Task<Widget> Update(Widget widget);
  }
}