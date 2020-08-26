using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_ManualWeight
    {
        public tblTransaction trans { get; set; }
        public IEnumerable<tblTransactionMaterial> transmaterials { get; set; }
        public tblTransactionMaterial material { get; set; }
        public TruckMaster truckMaster { get; set; }
        public VehicleClassification VC { get; set; }
        public string TruckNo { get; set; }

        public string UserName { get; set; }
        public string WeibridgeId { get; set; }
        public string shiftName { get; set; }
        public string plantCode { get; set; }
        public string companyCode { get; set; }

    }
}