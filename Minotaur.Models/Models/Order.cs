﻿using System.ComponentModel.DataAnnotations;

namespace Minotaur.Models.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverLastName { get; set; }
        public string? OrderedProducts { get; set; }
        public string? ShippedProducts { get; set; }
        public string? MissingItems { get; set; }
        [DataType(DataType.DateTime)] public DateTime PurchaseDate { get; set; }
        public int PurchaseAmount { get; set; }
        public string? OrderStatus { get; set; }
        public bool IsCourierDelivery { get; set; }
        public Guid OrderPickupPointId { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        [DataType(DataType.PhoneNumber)] public string? PhoneNumber { get; set; }
        public Guid AssemblyResponsibleWorkerId { get; set; }
        public int ConfirmationCode { get; set; }
        public int RefundAmount { get; set; } = 0;
    }
}
