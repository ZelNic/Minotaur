﻿using Bookstore.DataAccess;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> categoryList = _db.Categories.ToList();
            return View(categoryList);
        }

        public IActionResult BookCategory(int categoryId)
        {
            CategoryVM categoryVM = new()
            {
                BookList = _db.Books.Where(u => u.Category == categoryId).ToList(),
                CategoryList = _db.Categories.ToList()
            };

            return View(categoryVM);
        }

        public IActionResult Upsert(int? categoryId)
        {
            if (categoryId == 0 || categoryId == null)
            {
                Category category = new Category();

                return View(category);
            }

            var book = _db.Categories.Find(categoryId);

            return View(book);
        }

        [HttpPost]
        public IActionResult Upsert(Category category)
        {
            if (category.Id == 0)
            {
                _db.Add(category);

            }
            else
            {
                _db.Update(category);
            }

            _db.SaveChanges();
            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int? categoryId)
        {
            if (categoryId != null)
            {
                var categoryOnDelete = _db.Categories.Find(categoryId);
                return View(categoryOnDelete);
            }
            else return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(int? categoryId)
        {
            var categoryOnDelete = _db.Categories.Find(categoryId);
            if (categoryOnDelete != null)
            {
                _db.Categories.Remove(categoryOnDelete);
                _db.SaveChanges();
                return RedirectToAction("Index","Category");
            }
            else
            {
                return NotFound();
            }
        }
    }
}