using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_UserClassification
    {
        public int Id { get; set; }
        public string UserType { get; set; }
        public string MasterFileUpdation { get; set; }
        public string MasterRecordDeletion { get; set; }
        public string PendingRecordDeletion { get; set; }
        public string TransactionDeletion { get; set; }
        public string Configuration { get; set; }
        public string PasswordPolicy { get; set; }
        public string PasswordReset { get; set; }
        public string RFIDIssue { get; set; }
        public string GateEntry { get; set; }
        public string Weighment { get; set; }
        public string DatabaseOperation { get; set; }
        public string UserCreation { get; set; }
    }
}