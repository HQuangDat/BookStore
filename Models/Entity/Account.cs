namespace BookStore.Models.Entity
{
    public class Account
    {
        public int AccountID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
