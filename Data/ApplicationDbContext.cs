using BookStore.Models.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Account> Accounts { get; set; }

        //Lien ket moi quan he cac bang
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Book Entity
            builder.Entity<Book>()
                .HasKey(b => b.BookID);
            builder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorID);
            builder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryID);

            // Author Entity
            builder.Entity<Author>()
                .HasKey(a => a.AuthorID);

            // Category Entity
            builder.Entity<Category>()
                .HasKey(c => c.CategoryID);

            // Customer Entity
            builder.Entity<Customer>()
                .HasKey(c => c.CustomerID);
            builder.Entity<Customer>()
                .HasOne(c => c.Account)
                .WithOne(a => a.Customer)
                .HasForeignKey<Customer>(c => c.AccountID);

            // Order Entity
            builder.Entity<Order>()
                .HasKey(o => o.OrderID);
            builder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerID);

            // OrderDetail Entity
            builder.Entity<OrderDetail>()
                .HasKey(od => od.OrderDetailID);
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID);
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Book)
                .WithMany(b => b.OrderDetails)
                .HasForeignKey(od => od.BookID);

            // Review Entity
            builder.Entity<Review>()
                .HasKey(r => r.ReviewID);
            builder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookID);
            builder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerID);

            // Cart Entity
            builder.Entity<Cart>()
                .HasKey(sci => sci.CartID);
            builder.Entity<Cart>()
                .HasOne(sci => sci.Customer)
                .WithMany(c => c.Carts)
                .HasForeignKey(sci => sci.CustomerID);
            builder.Entity<Cart>()
                .HasOne(sci => sci.Book)
                .WithMany(b => b.Carts)
                .HasForeignKey(sci => sci.BookID);

            // Payment Entity
            builder.Entity<Payment>()
                .HasKey(p => p.PaymentID);
            builder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderID);

            // Account Entity
            builder.Entity<Account>()
                .HasKey(a => a.AccountID);
        }
    }
}
