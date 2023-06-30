﻿using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(o => o.ShipToAddress, a => a.WithOwner());

        builder
            .Navigation(o => o.ShipToAddress)
            .IsRequired();

        builder
            .Property(s => s.Status)
            .HasConversion(
                o => o.ToString(), 
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o));

        builder
            .HasMany(o => o.OrderItems)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}