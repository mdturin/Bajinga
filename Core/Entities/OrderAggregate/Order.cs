namespace Core.Entities.OrderAggregate;

public class Order : BaseEntity
{
    public string BuyerEmail { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public Address ShipToAddress { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public IReadOnlyList<OrderItem> OrderItems { get; set; }
    public double Subtotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string PaymentIntentId { get; set; }

    public Order() { }

    public Order(IReadOnlyList<OrderItem> orderItems,
                 string buyerEmail,
                 Address shipToAddress,
                 DeliveryMethod deliveryMethod,
                 double subtotal)
    {
        BuyerEmail = buyerEmail;
        ShipToAddress = shipToAddress;
        DeliveryMethod = deliveryMethod;
        OrderItems = orderItems;
        Subtotal = subtotal;
    }

    public double GetTotal()
    {
        return Subtotal + DeliveryMethod.Price;
    }
}
