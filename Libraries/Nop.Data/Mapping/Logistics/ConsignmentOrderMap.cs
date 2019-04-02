using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Logistics;

namespace Nop.Data.Mapping.Logistics
{
    public partial class ConsignmentOrderMap : NopEntityTypeConfiguration<ConsignmentOrder>
    {
        public override void Configure(EntityTypeBuilder<ConsignmentOrder> builder)
        {
            builder.ToTable(nameof(ConsignmentOrder));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Deleted).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.CTime).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(x => x.Consignor)
                .WithMany()
                .HasForeignKey(x => x.ConsignorId)
                .IsRequired();

            builder.HasOne(x => x.Consignee)
                .WithMany()
                .HasForeignKey(x => x.ConsigneeId)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
