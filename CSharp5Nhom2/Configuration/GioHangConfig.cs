using CSharp5Nhom2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharp5Nhom2.Configuration
{
    public class GioHangConfig : IEntityTypeConfiguration<GioHang>
    {
        public void Configure(EntityTypeBuilder<GioHang> builder)
        {
            builder.HasKey(x => x.IDGH);
            builder.HasOne(p => p.User).WithOne(p => p.GioHang).HasForeignKey<GioHang>(p => p.IDUser);
        }
    }
}
