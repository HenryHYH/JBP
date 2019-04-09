using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Logistics;

namespace Nop.Data.Mapping.Logistics
{
    public partial class TripMap : NopEntityTypeConfiguration<Trip>
    {
        public override void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.ToTable(nameof(Trip));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Deleted).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.CTime).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(x => x.Car)
                .WithMany()
                .HasForeignKey(x => x.CarId);

            builder.HasOne(x => x.Driver)
                .WithMany()
                .HasForeignKey(x => x.DriverId);

            base.Configure(builder);
        }
    }
}
