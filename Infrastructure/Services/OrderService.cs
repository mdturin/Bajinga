using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Interfaces.IServices;
using Core.Specifications;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> CreateOrderAsync(
        string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        var orderRepo = _unitOfWork.Repository<Order>();
        var deliveryMethodRepo = _unitOfWork.Repository<DeliveryMethod>();
        var productRepo = _unitOfWork.Repository<Product>();

        var basket = await _basketRepository.GetBasketAsync(basketId);

        var items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var productItem = await productRepo.GetByIdAsync(item.Id);
            var itemOrdered = new ProductItemOrdered(
                productItem!.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }

        var deliveryMethod = await deliveryMethodRepo.GetByIdAsync(deliveryMethodId);

        var subTotal = items.Sum(item => item.Price * item.Quantity);

        var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
        var order = await orderRepo.GetEntityWithSpec(spec);

        if (order != null)
        {
            order.ShipToAddress = shippingAddress;
            order.DeliveryMethod = deliveryMethod!;
            order.Subtotal = subTotal;
            orderRepo.Update(order);
        }
        else
        {
            order = new Order(
                items, 
                buyerEmail, 
                shippingAddress, 
                deliveryMethod!, 
                subTotal, 
                basket.PaymentIntentId);
            orderRepo.Add(order);
        }

        var result = await _unitOfWork.CompleteAsync();

        return result <= 0 ? null : order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id, string buyerEmail)
    {
        var spec = new OrderWithItemsAndOrderingSpecification(id, buyerEmail);
        return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var spec = new OrderWithItemsAndOrderingSpecification(buyerEmail);
        return await _unitOfWork.Repository<Order>().ListAsync(spec);
    }
}
