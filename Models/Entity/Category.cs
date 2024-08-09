using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Entity
{
    public class Category
    {
        [Key]
        public Guid CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
