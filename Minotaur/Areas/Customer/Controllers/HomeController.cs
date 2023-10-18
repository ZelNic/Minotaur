﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Minotaur.DataAccess.Repository.IRepository;
using Minotaur.Models;
using Minotaur.Models.Models;
using System.Diagnostics;

namespace Minotaur.Areas.Customer
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<MinotaurUser> _userManager;

        public HomeController(IUnitOfWork unitOfWork, UserManager<MinotaurUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ProductVM productVM = await GetProductsVM();
            return View(productVM);
        }

        public async Task<ProductVM> GetProductsVM()
        {
            List<Product>? productsList = _unitOfWork.Products.GetAllAsync().Result.ToList();
            List<Category>? categoriesList = _unitOfWork.Categories.GetAllAsync().Result.ToList();
            WishList? wishLists = null;
            ShoppingBasketClient? shoppingBasketClient = null;

            MinotaurUser? user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                wishLists = await _unitOfWork.WishLists.GetAsync(u => u.UserId == user.Id);
                ShoppingBasket? sb = _unitOfWork.ShoppingBaskets.GetAllAsync(u => u.UserId == Guid.Parse(user.Id)).Result.Where(n => n.IsPurchased == false).FirstOrDefault();
                if (sb != null)
                {
                    shoppingBasketClient = new()
                    {
                        Id = sb.UserId,
                        ProductIdAndCount = ShoppingBasketController.ParseProductData(sb.ProductIdAndCount)
                    };
                }
            }

            ProductVM bookVM = new()
            {
                User = user?.Id,
                ProductsList = productsList,
                CategoriesList = categoriesList,
                WishList = wishLists,
                ShoppingBasket = shoppingBasketClient
            };

            return bookVM;
        }

        public async Task<IActionResult> Details(int productId)
        {
            var product = _unitOfWork.Products.GetAsync(p => p.ProductId == productId);

            return View(product);
        }


        public async Task<IActionResult> Search(string? searchString)
        {
            if (searchString == null)
            {
                return RedirectToAction("Index");
            }
            List<Product> products = _unitOfWork.Products.GetAllAsync(product => product.Name.Contains(searchString.ToLower())).Result.ToList();

            return View(products);
        }

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}