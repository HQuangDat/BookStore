namespace BookStore.Models.Entity
{
    public class OrderDetail
    {
        public Guid OrderDetailID { get; set; }
        public Guid OrderID { get; set; }
        public Guid BookID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public virtual Order Order { get; set; }
        public virtual Book Book { get; set; }
    }
}
