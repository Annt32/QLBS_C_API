using CSharp5Nhom2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharp5Nhom2.Configuration
{
    public class GHCTConfig : IEntityTypeConfiguration<GioHangChiTiet>
    {
        public void Configure(EntityTypeBuilder<GioHangChiTiet> builder)
        {
            builder.HasKey(x => x.IDGHCT);
            builder.HasOne(p => p.GioHang).WithMany(p => p.GioHangChiTiets).HasForeignKey(p => p.IDGH).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.Sach).WithMany().HasForeignKey( p => p.IDSach).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
