using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CSharp5Nhom2.Models
{
    public class DBSach : DbContext
    {
        public DBSach()
        {
            
        }

        public DBSach(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Sach> sachs { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<GioHang> gioHangs { get; set; }
        public DbSet<GioHangChiTiet> gioHangChiTiets { get; set; }
        public DbSet<HoaDon> hoaDons { get; set; }
        public DbSet<HoaDonChiTiet> hoaDonChiTiets { get; set; }
        public DbSet<TacGia> tacGias { get; set; }
        public DbSet<TheLoai> theLoais { get; set; }
        public DbSet<NhaXuatBan> nhaXuatBans { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source= server-name;Initial Catalog=DBSachNhom2;Integrated Security=True;Trust Server Certificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
