using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Logistics;

namespace Nop.Data.Mapping.Logistics
{
    public partial class DriverMap : NopEntityTypeConfiguration<Driver>
    {
        public override void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.ToTable(nameof(Driver));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Enabled).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.Deleted).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.CTime).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            base.Configure(builder);
        }
    }
}
