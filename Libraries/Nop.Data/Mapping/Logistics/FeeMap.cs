using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Logistics;

namespace Nop.Data.Mapping.Logistics
{
    public partial class FeeMap : NopEntityTypeConfiguration<Fee>
    {
        public override void Configure(EntityTypeBuilder<Fee> builder)
        {
            builder.ToTable(nameof(Fee));
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Trip)
                .WithMany(x => x.Fees)
                .HasForeignKey(x => x.TripId)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
