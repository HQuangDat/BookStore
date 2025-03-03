using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataModels;

[PrimaryKey("ReceiptId", "BookId")]
public partial class ReceiptItem
{
    [Key]
    public int ReceiptId { get; set; }

    [Key]
    public int BookId { get; set; }

    public int? Quantity { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("ReceiptItems")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("ReceiptId")]
    [InverseProperty("ReceiptItems")]
    public virtual Receipt Receipt { get; set; } = null!;
}
