using BookStore.Data;
using BookStore.DataModels;

namespace BookStore.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(Category category)
        {
            _db.Categories.Add(category);
        }

        public void Delete(Category category)
        {
            _db.Categories.Remove(category);
        }

        public IEnumerable<Category> GetAll()
        {
            return _db.Categories.ToList();
        }

        public Category GetCategoryById(int? id)
        {
            var category = _db.Categories.FirstOrDefault(y => y.CategoryId == id);
            if (category == null)
            {
                return null;
            }
            return category;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }
    }
}
