using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_BarrierMaster
    {
        public int Id { get; set; }
        public string PlantCodeId { get; set; }
        public string MachineId { get; set; }
        public string Name { get; set; }
        public string BarrierIdentification { get; set; }
        public string BarrierIP { get; set; }
        public string BarrierPORT { get; set; }
        public string BarrierScheme { get; set; }
    }
}
