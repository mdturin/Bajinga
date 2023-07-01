using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Interfaces.IServices;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entities.Product;

namespace Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketRepository _basketRepository;
    private readonly IConfiguration _config;

    public PaymentService(
        IUnitOfWork unitOfWork,
        IBasketRepository basketRepository,
        IConfiguration config)
    {
        _unitOfWork = unitOfWork;
        _basketRepository = basketRepository;
        _config = config;
    }

    public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
    {
        StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

        var basket = await _basketRepository.GetBasketAsync(basketId);

        if(basket == null) return null;

        var deliveryMethod = await _unitOfWork
            .Repository<DeliveryMethod>()
            .GetByIdAsync(basket.DeliveryMethodId);
        var shippingPrice = deliveryMethod?.Price ?? 0d;

        foreach(var item in basket.Items)
        {
            var productItem = await _unitOfWork
                .Repository<Product>().GetByIdAsync(item.Id);
            if(item.Price != productItem!.Price)
                item.Price = productItem.Price;
        }

        var service = new PaymentIntentService();

        if(string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = GetAmount(basket, shippingPrice),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };
            PaymentIntent intent = await service.CreateAsync(options);
            basket.PaymentIntentId = intent.Id;
            basket.ClientSecret = intent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = GetAmount(basket, shippingPrice)
            };
            await service.UpdateAsync(basket.PaymentIntentId, options);
        }

        await _basketRepository.UpdateBasketAsync(basket);

        return basket;
    }

    private static long GetAmount(CustomerBasket basket, double shippingPrice)
    {
        return GetTotal(basket) + (long)(shippingPrice * 100);
    }

    private static long GetTotal(CustomerBasket basket)
    {
        return (long)basket.Items.Sum(i => i.Quantity * i.Price * 100);
    }

    public async Task<Order?> UpdateOrderPaymentFailed(string paymentIntentId)
    {
        var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
        var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

        if(order == null) return null;
        order.Status = OrderStatus.PaymentFailed;

        await _unitOfWork.CompleteAsync();
        return order;
    }

    public async Task<Order?> UpdateOrderPaymentSucceeded(string paymentIntentId)
    {
        var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
        var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

        if(order == null) return null;
        order.Status = OrderStatus.PaymentReceived;
        
        await _unitOfWork.CompleteAsync();
        
        return order;
    }
}
