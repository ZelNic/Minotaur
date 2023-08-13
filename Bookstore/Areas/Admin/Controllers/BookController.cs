﻿using Bookstore.DataAccess;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[CustomAuthorization(ApplicationDbContext, IHttpContextAccessor)]
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly bool _isAdmin;

        public BookController(ApplicationDbContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;

            if (_contextAccessor.HttpContext.Session.GetInt32("Username") != null)
            {
                CustomAuthorizationAttribute checkUserIsAdmin = new();
                _isAdmin = checkUserIsAdmin.CheckUserIsAdmin(_db, _contextAccessor);
            }
        }

        public IActionResult Index()
        {
            if (_isAdmin == false)
                return NotFound("Отказано в доступе");

            if (_contextAccessor.HttpContext.Session.GetInt32("Username") != null)
            {
                int? userId = _contextAccessor.HttpContext.Session.GetInt32("Username");
                if ((userId != null) && (_db.Employees.Find(userId) != null))
                {
                    List<Book> booksList = _db.Books.ToList();
                    List<Category> categoriesList = _db.Categories.ToList();

                    BookVM bookVM = new()
                    {
                        BooksList = booksList,
                        CategoriesList = categoriesList
                    };

                    return View(bookVM);
                }
                else
                {
                    return NotFound("Отказано в доступе.");
                }
            }
            else
            {
                return NotFound("Отказано в доступе.");
            }
        }

        [HttpGet]
        public IActionResult Upsert(int? bookId)
        {
            var book = new Book();

            if (bookId != null)
            {
                book = _db.Books.Find(bookId);
            }

            BookVM bookVM = new()
            {
                Book = book,
                CategoriesList = _db.Categories.ToList()
            };

            return View(bookVM);
        }



        [HttpPost]
        [ActionName("Upsert")]
        public IActionResult UpsertPost(BookVM bookVM)
        {
            if (bookVM.Book.BookId == 0)
            {
                _db.Books.Add(bookVM.Book);
            }
            else
            {
                var oldVersionBook = _db.Books.Find(bookVM.Book.BookId);

                if (oldVersionBook != null)
                {
                    oldVersionBook.Title = bookVM.Book.Title;
                    oldVersionBook.Author = bookVM.Book.Author;
                    oldVersionBook.ISBN = bookVM.Book.ISBN;
                    oldVersionBook.Description = bookVM.Book.Description;
                    oldVersionBook.Price = bookVM.Book.Price;
                    oldVersionBook.Category = bookVM.Book.Category;
                    oldVersionBook.ImageURL = bookVM.Book.ImageURL;
                    _db.Books.Update(oldVersionBook);
                }
            }

            _db.SaveChanges();
            return RedirectToAction("Index", "Book");
        }



        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int? id)
        {
            if (id != null)
            {
                var bookOnDelete = _db.Books.Find(id);
                return View(bookOnDelete);
            }
            else return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(Book book)
        {
            var bookOnDelete = _db.Books.Find(book.BookId);
            if (bookOnDelete != null)
            {
                _db.Books.Remove(bookOnDelete);
                _db.SaveChanges();
                return RedirectToAction("Index", "Book");
            }
            else
            {
                return NotFound();
            }
        }
    }
}

