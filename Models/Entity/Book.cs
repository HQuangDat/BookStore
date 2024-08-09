using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Entity
{
    public class Book
    {
        [Key]
        public Guid BookID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public Guid AuthorID { get; set; }

        [Required]
        public Guid CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string Publisher { get; set; }

        [Required]
        [Range(0, 9999.99)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [StringLength(255)]
        public string CoverImage { get; set; }

        [ForeignKey("AuthorID")]
        public virtual Author Author { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
