using LibraryManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<PhanQuyen> PhanQuyens { get; set; }
        public DbSet<DocGia> DocGias { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<TheThuVien> TheThuViens { get; set; }
        public DbSet<Sach> Sachs { get; set; }
        public DbSet<CuonSach> CuonSachs { get; set; }
        public DbSet<DatCho> DatChos { get; set; }
        public DbSet<PhieuMuon> PhieuMuons { get; set; }
        public DbSet<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; }
        public DbSet<TienPhat> TienPhats { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PhanQuyen>()
                .HasKey(pq => new { pq.MaTaiKhoan, pq.MaVaiTro });

            modelBuilder.Entity<ChiTietPhieuMuon>()
                .HasKey(ct => new { ct.MaPhieu, ct.MaCuon });

            modelBuilder.Entity<TaiKhoan>()
                .HasIndex(tk => tk.Email)
                .IsUnique();

            modelBuilder.Entity<DocGia>()
                .HasIndex(dg => dg.MaSinhVien)
                .IsUnique();

            modelBuilder.Entity<DocGia>()
                .HasIndex(dg => dg.MaTaiKhoan)
                .IsUnique();

            modelBuilder.Entity<NhanVien>()
                .HasIndex(nv => nv.MaTaiKhoan)
                .IsUnique();

            modelBuilder.Entity<TaiKhoan>()
                .HasOne(tk => tk.DocGia)
                .WithOne(dg => dg.TaiKhoan)
                .HasForeignKey<DocGia>(dg => dg.MaTaiKhoan)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaiKhoan>()
                .HasOne(tk => tk.NhanVien)
                .WithOne(nv => nv.TaiKhoan)
                .HasForeignKey<NhanVien>(nv => nv.MaTaiKhoan)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhanQuyen>()
                .HasOne(pq => pq.TaiKhoan)
                .WithMany(tk => tk.PhanQuyens)
                .HasForeignKey(pq => pq.MaTaiKhoan)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PhanQuyen>()
                .HasOne(pq => pq.VaiTro)
                .WithMany(vt => vt.PhanQuyens)
                .HasForeignKey(pq => pq.MaVaiTro)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TheThuVien>()
                .HasOne(tv => tv.DocGia)
                .WithMany(dg => dg.TheThuViens)
                .HasForeignKey(tv => tv.MaDocGia)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CuonSach>()
                .HasOne(cs => cs.Sach)
                .WithMany(s => s.CuonSachs)
                .HasForeignKey(cs => cs.MaSach)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DatCho>()
                .HasOne(dc => dc.TheThuVien)
                .WithMany(tv => tv.DatChos)
                .HasForeignKey(dc => dc.MaThe)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DatCho>()
                .HasOne(dc => dc.Sach)
                .WithMany(s => s.DatChos)
                .HasForeignKey(dc => dc.MaSach)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhieuMuon>()
                .HasOne(pm => pm.TheThuVien)
                .WithMany(tv => tv.PhieuMuons)
                .HasForeignKey(pm => pm.MaThe)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhieuMuon>()
                .HasOne(pm => pm.NhanVien)
                .WithMany(nv => nv.PhieuMuons)
                .HasForeignKey(pm => pm.MaNhanVien)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChiTietPhieuMuon>()
                .HasOne(ct => ct.PhieuMuon)
                .WithMany(pm => pm.ChiTietPhieuMuons)
                .HasForeignKey(ct => ct.MaPhieu)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChiTietPhieuMuon>()
                .HasOne(ct => ct.CuonSach)
                .WithMany(cs => cs.ChiTietPhieuMuons)
                .HasForeignKey(ct => ct.MaCuon)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TienPhat>()
                .HasOne(tp => tp.PhieuMuon)
                .WithMany(pm => pm.TienPhats)
                .HasForeignKey(tp => tp.MaPhieu)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ThongBao>()
                .HasOne(tb => tb.TaiKhoan)
                .WithMany(tk => tk.ThongBaos)
                .HasForeignKey(tb => tb.MaTaiKhoan)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}