﻿using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public class CustomerBasketDto
{
    [Required] public string Id { get; set; }
    
    public List<BasketItemDto> Items { get; set; }
    
    public int? DeliveryMethod { get; set; }

    public string ClientSecret { get; set; }

    public string PaymentIntentId { get; set; }

    public double ShippingPrice { get; set; }
}
