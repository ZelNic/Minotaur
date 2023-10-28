﻿using System.ComponentModel.DataAnnotations;

namespace Minotaur.Models.Models.ModelReview
{
    internal class PickUpReviews
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        [Required][Range(1, 5)] public int PickUpRating { get; set; }
        public string? PickUpReviewText { get; set; }
    }
}