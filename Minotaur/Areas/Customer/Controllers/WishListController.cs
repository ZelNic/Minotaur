﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Minotaur.DataAccess;
using Minotaur.Models;
using Minotaur.Models.Models;

namespace Minotaur.Areas.Customer
{
    [Area("Customer")]
    [Authorize]
    public class WishListController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<MinotaurUser> _userManager;
        public WishListController(ApplicationDbContext db, UserManager<MinotaurUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> GetWishList()
        {
            MinotaurUser user = await _userManager.GetUserAsync(User);

            WishList? wishList = await _db.WishLists.Where(u => u.UserId == user.Id).FirstOrDefaultAsync();
            if (wishList == null) { return BadRequest(new { error = "Список желаний пуст" }); }

            List<int>? listId = wishList.ProductId.Split('|').Select(int.Parse).ToList();

            var wishListJson = await _db.Products
                .Where(u => listId.Contains(u.ProductId))
                .Join(_db.Categories, b => b.Category, c => c.Id, (b, c) => new
                {
                    image = b.ImageURL,
                    nameProduct = b.Name,
                    author = b.Author,
                    category = c.Name,
                    price = b.Price,
                    productId = b.ProductId
                }).ToListAsync();

            return Json(new { data = wishListJson });
        }


        [HttpPost]
        public async Task<IActionResult> AddWishList(string newProductId)
        {
            MinotaurUser user = await _userManager.GetUserAsync(User);

            WishList? wishList = await _db.WishLists.Where(u => u.UserId == user.Id).FirstOrDefaultAsync();

            List<int> listNewIdProducts = newProductId.Split('|').Select(int.Parse).ToList();

            if (wishList != null)
            {
                List<int> oldProductList = wishList.ProductId.Split('|').Select(int.Parse).ToList();

                for (int i = 0; i < listNewIdProducts.Count; i++)
                {
                    if (oldProductList.Contains(listNewIdProducts[i]))
                    {
                        listNewIdProducts.Remove(oldProductList[i]);
                    }
                    else
                    {
                        wishList.ProductId += "|" + listNewIdProducts[i].ToString();
                    }
                }

                _db.WishLists.Update(wishList);
            }
            else
            {
                WishList newProductInWishList = new()
                {
                    ProductId = string.Join("|", listNewIdProducts),
                    UserId = user.Id,
                };

                await _db.WishLists.AddAsync(newProductInWishList);
            }

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromWishList(int productId)
        {
            MinotaurUser user = await _userManager.GetUserAsync(User);

            WishList? wishList = _db.WishLists.Where(i => i.UserId == user.Id).FirstOrDefault();
            if (wishList == null)
                return NotFound();

            List<string> listId = wishList.ProductId.Split('|').ToList();
            listId.Remove(productId.ToString());
            wishList.ProductId = string.Join("|", listId);

            if (string.IsNullOrWhiteSpace(wishList.ProductId))
            {
                _db.WishLists.Remove(wishList);
            }
            else
            {
                _db.WishLists.Update(wishList);
            }

            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}