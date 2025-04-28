using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher<Account> _passwordHasher;
        public AccountRepository(ApplicationDbContext db, IPasswordHasher<Account> passwordHasher)
        {
            _db = db;
            _passwordHasher = passwordHasher;
        }

        public PasswordReset createPasswordReset(string email, string token)
        {
            var passwordreset = new PasswordReset
            {
                Email = email,
                Token = token,
                ExpireDate = DateTime.UtcNow.AddHours(1)
            };
            _db.PasswordResets.Add(passwordreset);
            return passwordreset;
        }

        public Account Details(int userId)
        {
            var account = _db.Accounts.Include(rol => rol.Roles).FirstOrDefault(u => u.AccountId == userId);
            if(account == null)
                return null;
            return account;
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _db.Accounts.ToList();
        }

        public async Task<Account> getByEmailAsync(string email)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(u => u.Email == email);
            if (account == null)
                return null;
            return account;
        }

        public async Task<Account> getByUsernameAsync(string username)
        {
            var account =  await _db.Accounts.Include(role => role.Roles).
                FirstOrDefaultAsync(name => name.Username == username);
            if (account == null)
                return null;
            return account;
        }

        public void GrantAdmin(int userID)
        {
            throw new NotImplementedException();
        }

        public Task<Account> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void Register(Account user)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int? userID)
        {
            Account user = _db.Accounts.FirstOrDefault(us => us.AccountId == userID);
            if (user != null)
            {
                _db.Accounts.Remove(user);
                return true;
            }
            return false;
        }

        public PasswordReset getPasswordReset(string token)
        {
            var passwordReset = _db.PasswordReset.FirstOrDefault(p => p.Token == token);
            if (passwordReset == null)
                return null;
            return passwordReset;
        }

        public void removePasswordReset(PasswordReset passwordReset)
        {
            _db.PasswordReset.Remove(passwordReset);
        }

        public async Task resetPassword(Account user, string newpassword)
        {
            if(user!=null)
            {
                user.Password = await _passwordHasher.HashPassword(user, newpassword);
                _db.Accounts.Update(user);
            }
            
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public Account getById(int id)
        {
           var account = _db.Accounts.Include(rl => rl.Roles).FirstOrDefault(u => u.AccountId == id);
            if (account == null)
                return null;
            return account;
        }
    }
}
