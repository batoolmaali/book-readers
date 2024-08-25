using BookReaders.Data;
using BookReaders.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Components
{
    public class BestBooks:ViewComponent
    {
        private readonly AppDbContext _DbContext;
        public BestBooks(AppDbContext dbContext)
        {
            this._DbContext = dbContext;

        }
        public IViewComponentResult Invoke()
        {

            var bestBooks = _DbContext.Borrows
                .GroupBy(b => b.BookId)
                .Select(g => new { BookId = g.Key, BorrowCount = g.Count() })
                .OrderByDescending(x => x.BorrowCount)
                .Take(5)
                .ToList();

            var bestBookIds = bestBooks.Select(b => b.BookId).ToList();

            var bestBookEntities = _DbContext.Books
                .Where(b => bestBookIds.Contains(b.Id))
                .ToList();

            // Assuming a Borrow model has a Book property
            var bestBorrows = bestBookEntities.Select(book => new Borrow { Book = book });

            return View("Default", bestBorrows);


        }
    }
}
