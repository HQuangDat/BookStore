using BookStore.DataModels;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> getAll();
        void AddnewBook(Book book);
        void EditBook(Book book);
        void RemoveBook(Book book);

        Book findById(int? id);

        IEnumerable<Category> GetAllCategories();
        void Save();
    }
}
