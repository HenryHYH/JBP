using Nop.Core.Domain.Logistics;
using Nop.Services.ExportImport.Help;
using System.Collections.Generic;

namespace Nop.Services.ExportImport
{
    public partial class ImportConsignmentOrderMetaData
    {
        public int EndRow { get; internal set; }

        public PropertyManager<ConsignmentOrder> Manager { get; internal set; }

        public IList<PropertyByName<ConsignmentOrder>> Properties { get; set; }

        public PropertyManager<Goods> GoodsManager { get; internal set; }

        public List<int> ConsignmentOrdersInFile { get; set; }

        public string[] DriverNames { get; set; }

        public string[] CarLicenses { get; set; }
    }
}
