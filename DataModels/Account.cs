using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataModels;

[Table("Account")]
[Index("Email", Name = "UQ__Account__A9D105349BEE982C", IsUnique = true)]
public partial class Account
{
    [Key]
    public int AccountId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    [Required]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    [Required]
    public string Password { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    [Required]
    [EmailAddress(ErrorMessage = "You must enter a valid email address!")]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    [Required]
    public string? Address { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("Account")]
    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    [ForeignKey("AccountId")]
    [InverseProperty("Accounts")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}

