using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_Packing
    {
        public string Id { get; set; }
        public string  Name { get; set; }
        public string PackingCode { get; set; }
        public string PackingName { get; set; }
        public string PackingUOM { get; set; }
        public string PackingWeight { get; set; }
    }
}