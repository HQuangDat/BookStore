namespace BookStore.Models.Entity
{
    public class Author
    {
        public Guid AuthorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
