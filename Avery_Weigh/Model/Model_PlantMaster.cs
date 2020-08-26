using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_PlantMaster
    {
        public int Id { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string PlantName { get; set; }
        public string  Name { get; set; }
        public string PlantAddress1 { get; set; }
        public string PlantAddress2 { get; set; }
        public string PlantContactPerson { get; set; }
        public string Designation { get; set; }
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public int NoOfMachine { get; set; }
    }
}