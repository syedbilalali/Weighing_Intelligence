using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_VehicleClassification
    {
        public int Id { get; set; }
        public string ClassificationCode { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public Nullable<decimal> NoOfAxies { get; set; }
        public string BodyType { get; set; }
        public Nullable<decimal> KerbWT { get; set; }
        public Nullable<int> ManufactureYear { get; set; }
        public Nullable<decimal> GrossWeight { get; set; }
        public string UOMWeight { get; set; }
        public bool IsDeleted { get; set; }
    }
}