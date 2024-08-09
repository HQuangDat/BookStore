using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Entity
{
    public class Review
    {
        [Key]
        public Guid ReviewID { get; set; }

        [Required]
        public Guid BookID { get; set; }

        [Required]
        public Guid CustomerID { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ReviewDate { get; set; }

        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
    }
}
