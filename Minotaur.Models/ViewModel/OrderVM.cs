﻿using Minotaur.Models.Models;

namespace Minotaur.Models.ViewModel
{
    public class OrderVM
    {
        //public IEnumerable<ProductData> ProductData { get; set; }
        public IEnumerable<Order>? Orders { get; set; }
        public Order? Order { get; set; }
        public string? OperationName { get; set; }
    }
}
