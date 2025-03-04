using BookStore.DataModels;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Book> Books { get; set; }

        public virtual DbSet<Cart> Carts { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Receipt> Receipts { get; set; }

        public virtual DbSet<ReceiptItem> ReceiptItems { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A62AD8BDBE");

                entity.HasMany(d => d.Roles).WithMany(p => p.Accounts)
                    .UsingEntity<Dictionary<string, object>>(
                        "AccountRole",
                        r => r.HasOne<Role>().WithMany()
                            .HasForeignKey("RoleId")
                            .OnDelete(DeleteBehavior.Cascade) // Cascade delete in join table
                            .HasConstraintName("FK_AccountRoles_Role"),
                        l => l.HasOne<Account>().WithMany()
                            .HasForeignKey("AccountId")
                            .OnDelete(DeleteBehavior.Cascade) // Cascade delete in join table
                            .HasConstraintName("FK_AccountRoles_Account"),
                        j =>
                        {
                            j.HasKey("AccountId", "RoleId");
                            j.ToTable("AccountRoles");
                        });
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.BookId).HasName("PK__Book__3DE0C2075FE3A1F9");

                entity.HasMany(b => b.Warehouses)
                .WithMany(w => w.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookWarehouse",  
                    b => b.HasOne<Warehouse>().WithMany().HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_BookWarehouse_Warehouse"),
                    w => w.HasOne<Book>().WithMany().HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_BookWarehouse_Book"),
                    j =>
                    {
                        j.HasKey("BookId", "WarehouseId");
                        j.ToTable("BookWarehouse"); 
                    });

                entity.HasMany(d => d.Categories).WithMany(p => p.Books)
                    .UsingEntity<Dictionary<string, object>>(
                        "BookCategory",
                        r => r.HasOne<Category>().WithMany()
                            .HasForeignKey("CategoryId")
                            .OnDelete(DeleteBehavior.Cascade) // Cascade delete in join table
                            .HasConstraintName("FK_BookCategories_Category"),
                        l => l.HasOne<Book>().WithMany()
                            .HasForeignKey("BookId")
                            .OnDelete(DeleteBehavior.Cascade) // Cascade delete in join table
                            .HasConstraintName("FK_BookCategories_Book"),
                        j =>
                        {
                            j.HasKey("BookId", "CategoryId");
                            j.ToTable("BookCategories");
                        });
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.BookId }); // Composite key

                entity.Property(e => e.Quantity).HasDefaultValue(1);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Cascade) 
                    .HasConstraintName("FK_Cart_Account");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Cart_Book");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B06D26FF6");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.HasKey(e => e.ReceiptId).HasName("PK__Receipt__CC08C42079AC02C1");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Account).WithMany(p => p.Receipts)
                    .OnDelete(DeleteBehavior.Restrict) // Prevent deleting an Account if Receipts exist
                    .HasConstraintName("FK_Receipt_Account");
            });

            modelBuilder.Entity<ReceiptItem>(entity =>
            {
                entity.Property(e => e.Quantity).HasDefaultValue(1);

                entity.HasOne(d => d.Book).WithMany(p => p.ReceiptItems)
                    .OnDelete(DeleteBehavior.Cascade) // Cascade delete ReceiptItems if the Book is deleted
                    .HasConstraintName("FK_ReceiptItems_Book");

                entity.HasOne(d => d.Receipt).WithMany(p => p.ReceiptItems)
                    .OnDelete(DeleteBehavior.Cascade) // Cascade delete ReceiptItems if the Receipt is deleted
                    .HasConstraintName("FK_ReceiptItems_Receipt");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1A31DA8E39");
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFF9415FE8BF");
            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
