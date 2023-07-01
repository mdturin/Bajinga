namespace Core.Entities;

public class BasketItem : BaseEntity
{
    public int Quantity { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
    public string PictureUrl { get; set; }
    public string Brand { get; set; }
    public string Type { get; set; }
}
