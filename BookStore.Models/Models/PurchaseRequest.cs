﻿using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models.Models
{
    public class PurchaseRequest
    {
        [Key]
        public int Id { get; set; }
        public DateTime ApplicationTime { get; set; }
        public int ResponsiblePersonId { get; set; }
        public int StockId { get; set; }
        public List<int> ProductsId { get; set; }
        public List<int> CountProduct { get; set; }
    }
}
