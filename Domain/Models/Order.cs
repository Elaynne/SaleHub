﻿using Domain.Enums;

namespace Domain.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid ClientId { get; set; }
        public Guid SellerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice {get;set;}
        public OrderStatus Status { get; set; }

    }

    public class OrderItem
    {
        public Guid BookId { get; set; }
        public int Quantity { get; set; }
    }
}
