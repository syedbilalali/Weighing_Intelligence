using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_TruckMaster
    {
        public int Id { get; set; }
        public string TruckRegNo { get; set; }
        public string ClassificationCode { get; set; }
        public string StoredTareWeight { get; set; }
        public string TareValidityDate { get; set; }
        public string AverageTareScheme { get; set; }
        public string CurrentAverageTareValue { get; set; }
        public string UOMWeight { get; set; }
        public string Make { get; set; }
    }
}