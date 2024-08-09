using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Entity
{
    public class Author
    {
        [Key]
        public Guid AuthorID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
