using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Repository
{
    public class GateEntryRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public int GetGateEntryNo()
        {
            int result = 0;
            var gateRecord = db.tblGateEntryRecords.ToList();
            if (gateRecord.Count == 0)
                result = 1;
            else
                result = gateRecord.Count() + 1;

            return result;
        }

        internal string gettuckNofromgateEntry(string gateNo)
        {
            string result = "0";
            tblGateEntryRecord rec = db.tblGateEntryRecords.FirstOrDefault(x => x.GatePassNo == Convert.ToInt32(gateNo));
            if(rec != null)
            {
                result = rec.TruckNo.ToString();
            }

            return result;
        }
    }
}