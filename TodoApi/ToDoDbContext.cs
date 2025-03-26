using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TodoApi
{
    public partial class ToDoDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        // קונסטרוקטור שמקבל את הקונפיגורציה
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // לבדוק אם לא הוגדר קודם
            {
                var connectionString = _configuration.GetConnectionString("ToDoDB");
                optionsBuilder.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.ToTable("items");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Email).HasName("PRIMARY");
                entity.ToTable("users");
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Nmae).HasMaxLength(50); // תוקןפה ולא שיניתי כי זה היה שגיעה מה 
                entity.Property(e => e.Password).HasMaxLength(45);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
