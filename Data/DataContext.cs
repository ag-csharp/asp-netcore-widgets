using Microsoft.EntityFrameworkCore;
using WidgetExample.Entities;

namespace WidgetExample.Data {
  // ReSharper disable once ClassNeverInstantiated.Global
  public class DataContext : DbContext {
    public DataContext(DbContextOptions<DataContext> options) : base (options) {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Widget> Widgets { get; set; }
  }
}