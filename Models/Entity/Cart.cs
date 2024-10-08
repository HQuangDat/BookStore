﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Entity
{
    public class Cart
    {
        [Key]
        public Guid CartID { get; set; }

        [Required]
        public Guid CustomerID { get; set; }

        [Required]
        public Guid BookID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }
    }
}
