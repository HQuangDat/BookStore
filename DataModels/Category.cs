using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataModels;

[Table("Category")]
public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("Categories")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
