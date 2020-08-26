using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Mode_transactionWeight
    {
        public int TransactionId { get; set; }
        public int Id { get; set; }
        public decimal Weight { get; set; }
        public DateTime WeightDate { get; set; }
        public DateTime WeightTime { get; set; }
    }
}