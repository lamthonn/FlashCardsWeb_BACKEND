using backend_v3.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_v3.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<HocPhan> HocPhans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HocPhan>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.ToTable("tbl_HocPhan");
            });
        }
    }
}
