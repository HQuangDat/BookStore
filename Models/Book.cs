using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataModels;

[Table("Book")]
public partial class Book
{
    [Key]
    public int BookId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string BookName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Provider { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    public int AuthorId { get; set; }

    public int WarehouseId { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("Books")]
    public virtual Author Author { get; set; } = null!;

    [InverseProperty("Book")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [InverseProperty("Book")]
    public virtual ICollection<ReceiptItem> ReceiptItems { get; set; } = new List<ReceiptItem>();

    [ForeignKey("WarehouseId")]
    [InverseProperty("Books")]
    public virtual Warehouse Warehouse { get; set; } = null!;

    [ForeignKey("BookId")]
    [InverseProperty("Books")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
