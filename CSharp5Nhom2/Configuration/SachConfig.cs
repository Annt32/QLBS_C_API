using CSharp5Nhom2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharp5Nhom2.Configuration
{
    public class SachConfig : IEntityTypeConfiguration<Sach>
    {
        public void Configure(EntityTypeBuilder<Sach> builder)
        {
            builder.HasKey(x => x.IDSach);
            builder.HasOne(p => p.TacGia)
                   .WithMany(p => p.Sachs)
                   .HasForeignKey(p => p.IDTacGia);
            builder.HasOne(p => p.NhaXuatBan)
                   .WithMany(p => p.Sachs)
                   .HasForeignKey(p => p.IDNXB);
            builder.HasOne(p => p.TheLoai)
                   .WithMany(p => p.Sachs)
                   .HasForeignKey(p => p.IDTheLoai);
            builder.HasMany(p => p.HoaDonChiTiets) // Thêm quan hệ Many-to-One
                   .WithOne(p => p.Sach)
                   .HasForeignKey(p => p.IDSach);
        }
    }
}
