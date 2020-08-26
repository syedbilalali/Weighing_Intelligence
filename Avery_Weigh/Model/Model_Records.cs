using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_Records
    {
        public int Id { get; set; }
        public string TruckNo { get; set; }
        public string IsMultiProduct { get; set; }
        public string TripType { get; set; }
        public string Materials { get; set; }
        public string GrossWt { get; set; }
        public string DateIn { get; set; }
        public string TimeIn { get; set; }
        public string TareWt { get; set; }
        public string DateOut { get; set; }
        public string TimeOut { get; set; }
        public string NetWt { get; set; }
        public string Supplier { get; set; }
        public string Transporter { get; set; }
    }
}