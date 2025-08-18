using BookStore.Data;
using BookStore.DataModels;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _db;
        public BookRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddnewBook(Book book)
        {
            book.Categories = _db.Categories.Where(c => book.SelectedCategories.Contains(c.CategoryId)).ToList();
            _db.Books.Add(book);
            _db.SaveChanges();
        }

        public void EditBook(Book book)
        {
            _db.Books.Update(book);
            _db.SaveChanges();
        }

        public Book findById(int? id)
        {
            var book = _db.Books.Include(ct => ct.Categories)
                //.Include(wh=>wh.Warehouse)
                .FirstOrDefault(y => y.BookId == id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }
            return book;
        }

        public IEnumerable<Book> getAll()
        {
            return _db.Books.Include(b => b.Categories).AsNoTracking().ToList();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _db.Categories.AsNoTracking().ToList();
        }

        public void RemoveBook(Book book)
        {
            _db.Books.Remove(book);
            _db.SaveChanges();
        }
    }
}
