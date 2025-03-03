using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataModels;

[Table("Role")]
public partial class Role
{
    [Key]
    public int RoleId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string RoleName { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("Roles")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
