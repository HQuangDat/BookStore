using BookStore.DataModels;

namespace BookStore.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();

        Task<Account> Login(string username, string password);

        void Register(Account user);

        void GrantAdmin(int userID);

        bool Remove(int? userID);

        Account Details(int userId);

        Account getById(int id);

        Task<Account> getByEmailAsync(string email);

        Task<Account> getByUsernameAsync(string username);

        PasswordReset createPasswordReset(string email, string token);

        PasswordReset getPasswordReset(string token);

        void removePasswordReset(PasswordReset passwordReset);

        void Save();
        Task resetPassword(Account user, string newpassword);

    }
}
