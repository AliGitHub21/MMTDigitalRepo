﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMTDigital.Model
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public int OrderId { get; set; } //FOREIGN KEY REFERENCES orders
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int ProductId { get; set; } //FOREIGN KEY REFERENCES products
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool Returnable { get; set; }
    }
}
