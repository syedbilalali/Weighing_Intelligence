using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_Materials
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDesc { get; set; }
        public string PackingCodeId { get; set; }
        public string PackingCode { get; set; }
        public string MaterialClassificationId { get; set; }
        public string MaterialClassificationCode { get; set; }
       
    }
}