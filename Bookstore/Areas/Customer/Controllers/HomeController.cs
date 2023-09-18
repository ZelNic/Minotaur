﻿using Bookstore.DataAccess;
using Bookstore.Models;
using Bookstore.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bookstore.Areas.Customer
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;
        private readonly User? _user;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, ApplicationDbContext db)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _db = db;

            if (_httpContextAccessor.HttpContext.Session.GetInt32("Username") != null)
            {
                int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("Username");
                if ((userId != null) && (_db.User.Find(userId) != null))
                {
                    _user = _db.User.Find(userId);
                }
            }
        }

        public async Task<IActionResult> Index()
        {
            BookVM bookVm = await GetBookVM();
            return View(bookVm);
        }

        public async Task<BookVM> GetBookVM()
        {
            List<Book>? booksList = await _db.Books.ToListAsync();
            List<Category>? categoriesList = await _db.Categories.ToListAsync();
            WishList? wishLists = null;
            ShoppingBasketClient? shoppingBasketClient = null;

            if (_user != null)
            {
                wishLists = await _db.WishLists.Where(u => u.UserId == _user.UserId).FirstOrDefaultAsync();
                ShoppingBasket? sb = await _db.ShoppingBasket.Where(u => u.UserId == _user.UserId).FirstOrDefaultAsync();
                if(sb != null)
                {
                    shoppingBasketClient = new()
                    {
                        Id = sb.UserId,
                        ProductIdAndCount = ShoppingBasketController.ParseProductData(sb)
                    };
                }
            }

            BookVM bookVM = new()
            {
                User = _user,
                BooksList = booksList,
                CategoriesList = categoriesList,
                WishList = wishLists,
                ShoppingBasket = shoppingBasketClient
            };

            return bookVM;
        }

        public async Task<IActionResult> Details(int productId)
        {
            var product = await _db.Books.FindAsync(productId);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string? searchString)
        {
            if (searchString == null)
            {
                return RedirectToAction("Index");
            }
            IEnumerable<Book> books = await _db.Books.Where(book => book.Title.Contains(searchString.ToLower())).ToListAsync();

            return View(books);
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