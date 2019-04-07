using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Logistics;

namespace Nop.Data.Mapping.Logistics
{
    public partial class ConsignmentUserMap : NopEntityTypeConfiguration<ConsignmentUser>
    {
        public override void Configure(EntityTypeBuilder<ConsignmentUser> builder)
        {
            builder.ToTable(nameof(ConsignmentUser));
            builder.HasKey(x => x.Id);

            base.Configure(builder);
        }
    }
}
