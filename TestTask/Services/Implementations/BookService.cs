using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _dbContext;

        public BookService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns The book with the highest total published value.
        /// </summary>
        public async Task<Book> GetBook()
        {
            var highestTotalPublishedValue = await _dbContext.Books
                .MaxAsync(b => b.Price * b.QuantityPublished);

            var book = await _dbContext.Books
                .Include(b => b.Author)
                .Where(b => b.Price * b.QuantityPublished == highestTotalPublishedValue)
                .FirstOrDefaultAsync();

            return book;
        }

        /// <summary>
        /// Returns a list of books with "Red" in their title and publishing date after the release of album "Carolus Rex" by Sabaton (May 25 2012).
        /// </summary>
        public async Task<List<Book>> GetBooks()
        {
            var referenceDate = new DateTime(2012, 5, 25);

            var books = await _dbContext.Books
                .Include(b => b.Author)
                .Where(b => b.Title.Contains("Red") && b.PublishDate > referenceDate)
                .ToListAsync();

            return books;
        }
    }
}
