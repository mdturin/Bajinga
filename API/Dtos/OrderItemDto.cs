using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public class OrderItemDto
{
    [Required] public int ProductId { get; set; }
    [Required] public string ProductName { get; set; }
    [Required] public string PictureUrl { get; set; }
    [Required] public double Price { get; set; }
    [Required] public int Quantity { get; set; }
}
