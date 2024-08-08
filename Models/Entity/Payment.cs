namespace BookStore.Models.Entity
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentMethod { get; set; }

        public virtual Order Order { get; set; }
    }
}
