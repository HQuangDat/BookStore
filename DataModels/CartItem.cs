using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataModels;

[PrimaryKey("CartId", "BookId")]
public partial class CartItem
{
    [Key]
    public int CartId { get; set; }

    [Key]
    public int BookId { get; set; }

    public int? Quantity { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("CartItems")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("CartId")]
    [InverseProperty("CartItems")]
    public virtual Cart Cart { get; set; } = null!;
}
