﻿namespace Core.Entities.OrderAggregate;

public class DeliveryMethod : BaseEntity
{
    public string ShortName { get; set; }
    public DateTime DeliveryTime { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
}
