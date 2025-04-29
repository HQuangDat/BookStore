using BookStore.DataModels;

namespace BookStore.Repositories
{
    public interface ICartRepository
    {
        IEnumerable<Cart> getAllByUserID(string userId);
        Cart findCartbyBookID(int? id);
        void Remove(Cart cart);
        void Update(Cart cart);
        void Add(Cart cart);
        Book findBookbyID(int? id);

        Cart existingCartItem(int? accountId, int? bookId);
        void Save();
    }
}
