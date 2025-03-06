using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataModels;

[Table("Receipt")]
public partial class Receipt
{
    [Key]
    public int ReceiptId { get; set; }

    public int AccountId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PaymentType { get; set; }

    public decimal TotalAmount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Receipts")]
    public virtual Account Account { get; set; } = null!;

    [InverseProperty("Receipt")]
    public virtual ICollection<ReceiptItem> ReceiptItems { get; set; } = new List<ReceiptItem>();
}
