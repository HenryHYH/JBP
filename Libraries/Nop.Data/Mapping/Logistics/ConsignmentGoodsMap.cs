using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Logistics;

namespace Nop.Data.Mapping.Logistics
{
    public partial class ConsignmentGoodsMap : NopEntityTypeConfiguration<ConsignmentGoods>
    {
        public override void Configure(EntityTypeBuilder<ConsignmentGoods> builder)
        {
            builder.ToTable(NopMappingDefaults.ConsignmentGoodsTable);
            builder.HasKey(x => new { x.OrderId, x.GoodsId });

            builder.Property(x => x.OrderId).HasColumnName("Order_Id");
            builder.Property(x => x.GoodsId).HasColumnName("Goods_Id");

            builder.HasOne(x => x.Order)
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .IsRequired();

            builder.HasOne(x => x.Goods)
                .WithMany()
                .HasForeignKey(x => x.GoodsId)
                .IsRequired();

            builder.Property(x => x.Amount).IsRequired();

            builder.Ignore(x => x.Id);

            base.Configure(builder);
        }
    }
}
