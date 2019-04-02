using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Logistics;

namespace Nop.Data.Mapping.Logistics
{
    public partial class GoodsMap : NopEntityTypeConfiguration<Goods>
    {
        public override void Configure(EntityTypeBuilder<Goods> builder)
        {
            builder.ToTable(nameof(Goods));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Deleted).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.CTime).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            base.Configure(builder);
        }
    }
}
