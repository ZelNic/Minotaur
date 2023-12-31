﻿using System.ComponentModel.DataAnnotations;

namespace Minotaur.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Price { get; set; }
        public string? ImageURL { get; set; }
        public int? Category { get; set; }
        public float? ProductRating { get; set; }
        public int EditorId { get; set; }        
    }
}
