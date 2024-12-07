using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns the author who wrote a book with the longest title. If there are multiple authors like that, returns the one with the smallest id.
        /// </summary>
        public async Task<Author> GetAuthor()
        {
            var author = await _dbContext.Authors
                .Include(a => a.Books)
                .Where(a => a.Books.Any())
                .OrderByDescending(a => a.Books.Max(b => b.Title.Length))
                .ThenBy(a => a.Id)
                .FirstOrDefaultAsync();
            return author;
        }

        /// <summary>
        /// Returns a list of authors who have written an even number of books after 2015.
        /// </summary>
        public async Task<List<Author>> GetAuthors()
        {
            var authors = await _dbContext.Authors
                .Include(a => a.Books)
                .Where(a => a.Books.Count(b => b.PublishDate.Year > 2015) % 2 == 0 && a.Books.Count(b => b.PublishDate.Year > 2015) != 0) 
                .ToListAsync();
            return authors;
        }
    }
}
