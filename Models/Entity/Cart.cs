namespace BookStore.Models.Entity
{
    public class Cart
    {
        public int CartID { get; set; }
        public int CustomerID { get; set; }
        public int BookID { get; set; }
        public int Quantity { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Book Book { get; set; }
    }
}
