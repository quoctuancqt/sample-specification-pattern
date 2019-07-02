namespace src.Domain
{
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;

    public class TestDbContext : DbContext
    {
        public TestDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
            
        }

        public virtual DbSet<Order> Order { get; set; }

        public virtual DbSet<OrderDetail> OrderDetail { get; set; }

        public virtual DbSet<History> Histories { get; set; }

        public virtual DbSet<User> Users { get; set; }

    }
}
