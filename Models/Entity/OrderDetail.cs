using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Entity
{
    public class OrderDetail
    {
        [Key]
        public Guid OrderDetailID { get; set; }

        [Required]
        public Guid OrderID { get; set; }

        [Required]
        public Guid BookID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, 9999.99)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }
    }
}
