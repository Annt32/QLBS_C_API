using CSharp5Nhom2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharp5Nhom2.Configuration
{
    public class HDCTConfig : IEntityTypeConfiguration<HoaDonChiTiet>
    {
        public void Configure(EntityTypeBuilder<HoaDonChiTiet> builder)
        {
            builder.HasKey(x => x.IDHoaDonChiTiet);
            builder.HasOne(p => p.HoaDon)
                   .WithMany(p => p.HoaDonChiTiets)
                   .HasForeignKey(p => p.IDHD);
            builder.HasOne(p => p.Sach)
                   .WithMany(p => p.HoaDonChiTiets) // Thay đổi thành WithMany
                   .HasForeignKey(p => p.IDSach);
        }
    }
}
