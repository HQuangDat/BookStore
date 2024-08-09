using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Entity
{
    public class Payment
    {
        [Key]
        public Guid PaymentID { get; set; }

        [Required]
        public Guid OrderID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        public decimal PaymentAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }
    }
}
