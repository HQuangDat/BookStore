namespace BookStore.Models.Entity
{
    public class Review
    {
        public Guid ReviewID { get; set; }
        public Guid BookID { get; set; }
        public Guid CustomerID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        public virtual Book Book { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
