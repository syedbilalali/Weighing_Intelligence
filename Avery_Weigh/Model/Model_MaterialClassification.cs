using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_MaterialClassification
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MaterialClassificationDesc { get; set; }
        public string MaterialClassificationCode { get; set; }
        public string Supplier_VendorCode { get; set; }
        public bool IsDeleted { get; set; }
    }
}