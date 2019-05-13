using System.Collections.Generic;
using System.Threading.Tasks;
using WidgetExample.Entities;

namespace WidgetExample.Data {
  public class WidgetRepo : IWidgetRepo {
    private readonly DataContext _context;

    public WidgetRepo(DataContext context) {
      _context = context;
    }

    public Task<Widget> Add(Widget widget) {
      throw new System.NotImplementedException();
    }

    public Task<IEnumerable<Widget>> GetAll() {
      throw new System.NotImplementedException();
    }

    public Task<Widget> Get(string id) {
      throw new System.NotImplementedException();
    }

    public Task<bool> Remove(string id) {
      throw new System.NotImplementedException();
    }

    public Task<Widget> Update(Widget widget) {
      throw new System.NotImplementedException();
    }
  }
}