using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_UserMasters
    {
        public UserMaster _UserMaster { get; set; }
        public UserClassification _UserClassification { get; set; }
        public PlantMaster _PlantMaster { get; set; }
        public WeightMachineMaster _weightMachineMaster { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public string PlantCode { get; set; }
        public string WeighbridgeId { get; set; }
    }
}