using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataModels;

[Table("Warehouse")]
public partial class Warehouse
{
    [Key]
    public int WarehouseId { get; set; }

    [Required]
    [StringLength(255)]
    public string location { get; set; } = null!;

    public int? Remaining { get; set; }

    [ForeignKey("WarehouseId")]
    [InverseProperty("Warehouses")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
