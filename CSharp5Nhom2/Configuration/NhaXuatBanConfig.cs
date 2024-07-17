using CSharp5Nhom2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharp5Nhom2.Configuration
{
    public class NhaXuatBanConfig : IEntityTypeConfiguration<NhaXuatBan>
    {
        public void Configure(EntityTypeBuilder<NhaXuatBan> builder)
        {
            builder.HasKey(x => x.IDNXB);
        }
    }
}
