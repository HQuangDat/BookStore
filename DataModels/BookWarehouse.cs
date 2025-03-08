
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DataModels
{
    [Table("BookWarehouse")]

    public class BookWarehouse
    {
        [Key]
        public int BookId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; } = 1;

        [ForeignKey("BookId")]
        [InverseProperty("BookWarehouses")]
        public virtual Book Book { get; set; } = null!;

        [ForeignKey("WarehouseId")]
        [InverseProperty("BookWarehouses")]
        public virtual Warehouse Warehouse{ get; set; } = null!;
    }
}
