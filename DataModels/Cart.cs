using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataModels;

[Table("Cart")]
public partial class Cart
{
    [Key]
    public int AccountId { get; set; }

    public int BookId { get; set; }

    public int Quantity { get; set; } = 1; 

    [ForeignKey("AccountId")]
    [InverseProperty("Carts")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("BookId")]
    [InverseProperty("Carts")]
    public virtual Book Book { get; set; } = null!;
}

