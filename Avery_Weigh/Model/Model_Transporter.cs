using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_Transporter
    {
        public int Id { get; set; }
        public string ddCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string GSTNo { get; set; }
        public string PANNo { get; set; }
        public string ContactPerson { get; set; }
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
    }
}