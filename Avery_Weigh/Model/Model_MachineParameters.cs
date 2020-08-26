using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_MachineParameters
    {
        public string Id { get; set; }
        public string PlantCode { get; set; }
        public string MachineId { get; set; }
        public string IPPort { get; set; }
        public string PortNo { get; set; }
        public string ModeOfComs { get; set; }
        public string StabilityNos { get; set; }
        public string StabilityRange { get; set; }
        public string ZeroInterlock { get; set; }
        public string ZeroInterlockRange { get; set; }
        public string TransactionNoPrefiex { get; set; }
        public string TareCheck { get; set; }
        public string StoredTare { get; set; }
    }
}