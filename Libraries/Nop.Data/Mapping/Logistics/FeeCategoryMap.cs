using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Logistics;

namespace Nop.Data.Mapping.Logistics
{
    public partial class FeeCategoryMap : NopEntityTypeConfiguration<FeeCategory>
    {
        public override void Configure(EntityTypeBuilder<FeeCategory> builder)
        {
            builder.ToTable(nameof(FeeCategory));
            builder.HasKey(x => x.Id);

            base.Configure(builder);
        }
    }
}
