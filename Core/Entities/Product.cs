namespace Core.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string PictureUrl { get; set; }
    public string ProductType { get; set; }
    public int ProductTypeId { get; set; }
    public string ProductBrand { get; set; }
    public int ProductBrandId { get; set; }
}
