namespace BookStore.Models.Entity
{
    public class Book
    {
        public Guid BookID { get; set; }
        public string Title { get; set; }
        public Guid AuthorID { get; set; }
        public Guid CategoryID { get; set; }
        public string Publisher { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string CoverImage { get; set; }

        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
