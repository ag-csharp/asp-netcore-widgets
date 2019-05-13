using System.Collections.Generic;
using System.Threading.Tasks;
using WidgetExample.Entities;

namespace WidgetExample.Data {
  public interface IWidgetRepo {
    Task<Widget> Add(Widget widget);
    Task<IEnumerable<Widget>> GetAll();
    Task<Widget> Get(string id);
    Task<bool> Remove(string id);
    Task<Widget> Update(Widget widget);
  }
}