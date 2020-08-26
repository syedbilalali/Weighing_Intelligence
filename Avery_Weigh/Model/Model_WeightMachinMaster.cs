using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_WeightMachinMaster
    {
        public int Id { get; set; }
        
        public string PlantId { get; set; }
        public string MachineId { get; set; }
        public string Capacity { get; set; }
        public string Resolution { get; set; }
        public string Model { get; set; }
        public string PlatformSize { get; set; }
        public string MachineNo { get; set; }
        public string Indicator { get; set; }
        public string LCType { get; set; }
        public string NoOfLoadCells { get; set; }
        public string LoadCellSerialNos { get; set; }
        public Nullable<int> EquipmentId { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<DateTime> DespatchDate { get; set; }
        public Nullable<DateTime> InstallationDate { get; set; }
        public Nullable<DateTime> WarrantyUpto { get; set; }
        public string ReasonOfWarrantyUptoDate { get; set; }
    }
}