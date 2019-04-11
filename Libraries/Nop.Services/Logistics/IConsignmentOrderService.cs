using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Logistics;

namespace Nop.Services.Logistics
{
    public partial interface IConsignmentOrderService
    {
        IPagedList<ConsignmentOrder> GetAll(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            ShipmentMethod? shipmentMethod = null,
            string startPoint = null,
            string terminal = null,
            string consignor = null,
            string consignee = null,
            int? tripId = null,
            bool? noRelatedTrip = null,
            string[] serialNums = null);

        ConsignmentOrder Get(int id);

        void Insert(ConsignmentOrder entity);

        void Update(ConsignmentOrder entity);

        void Delete(ConsignmentOrder entity);

        void ChangeOrderStatus(ConsignmentOrder entity, OrderStatus orderStatus);

        void InsertGoods(Goods entity);

        void DeleteGoods(Goods entity);

        void DeleteGoods(ICollection<Goods> goods);
    }
}
