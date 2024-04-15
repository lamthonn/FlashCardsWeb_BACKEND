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
        public DbSet<ThuMuc> ThuMucs { get; set; }
        public DbSet<TheHoc> TheHocs { get; set; }
        public DbSet<NgonNgu> NgonNgus { get; set; }
        public DbSet<YkienGopY> YkienGopYs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.ToTable("Users");
            });

            modelBuilder.Entity<HocPhan>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.ToTable("tbl_HocPhan");
            });

            modelBuilder.Entity<ThuMuc>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.ToTable("tbl_ThuMuc");
                entity.HasMany(e => e.HocPhans)
                    .WithOne(e => e.ThuMuc)
                    .HasForeignKey(x => x.ThuMucId);
            });

            modelBuilder.Entity<TheHoc>(entity =>
            {
                entity.HasKey(x=> x.Id);
                entity.ToTable("tbl_TheHoc");
                entity.HasOne(e => e.HocPhan)
                    .WithMany(e => e.TheHocs)
                    .HasForeignKey(k => k.HocPhanId);
            });

            modelBuilder.Entity<NgonNgu>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.ToTable("tbl_NgonNgu");
            });

            modelBuilder.Entity<YkienGopY>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.ToTable("tbl_YKienGopY");
            });
        }
    }
}

