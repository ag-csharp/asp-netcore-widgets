namespace WidgetExample.Entities {
  public enum Category {
    Production = 2,
    Testing = 4,
    Tracking = 8,
    Qa
  }
  
  // ReSharper disable once ClassNeverInstantiated.Global
  public class Widget {
    public string Id { get; set; }
    public string Color { get; set; }
    public Category Category { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
  }
}