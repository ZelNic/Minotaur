﻿using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Areas.Purchase
{
    [Area("Purchase")]
    public class PurchaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}