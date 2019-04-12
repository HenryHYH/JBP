using Nop.Core.Domain.Logistics;
using Nop.Services.ExportImport.Help;
using System.Collections.Generic;

namespace Nop.Services.ExportImport
{
    public partial class ImportTripMetadata
    {
        public int EndRow { get; internal set; }

        public PropertyManager<Trip> Manager { get; set; }

        public IList<PropertyByName<Trip>> Properties { get; set; }

        public List<int> TripsInFile { get; set; }

        public int SerialNumCellNum { get; set; }

        public string[] SerialNums { get; set; }

        public string[] DriverNames { get; set; }

        public string[] CarLicenses { get; set; }

        public string[] ConsignmentOrderSerialNums { get; internal set; }
    }
}
