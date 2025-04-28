using BookStore.DataModels;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();

        Task<Account> Login(string username, string password);

        void createNewUser(Account user);

        void GrantAdmin(Account user, out string errorMessage);

        bool Remove(int? userID);

        Account Details(int userId);

        Account getById(int id);

        Task<Account> getByEmailAsync(string email);

        Task<Account> getByUsernameAsync(string username);

        PasswordReset createPasswordReset(string email, string token);

        PasswordReset getPasswordReset(string token);

        void removePasswordReset(PasswordReset passwordReset);

        void Save();
        void resetPassword(Account user, string newpassword);

        PasswordVerificationResult passwordVerificationResult(Account user, string userPassword, string inputPassword);

    }
}
