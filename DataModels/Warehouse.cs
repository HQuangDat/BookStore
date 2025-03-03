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

    public int? Remaining { get; set; }

    [InverseProperty("Warehouse")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
