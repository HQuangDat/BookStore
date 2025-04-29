using BookStore.DataModels;

namespace BookStore.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        void Add(Category category);
        void Update(Category category);

        void Delete(Category category);

        Category GetCategoryById(int? id);
        void Save();
    }
}
