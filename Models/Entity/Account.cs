using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Entity
{
    public class Account
    {
        [Key]
        public Guid AccountID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
