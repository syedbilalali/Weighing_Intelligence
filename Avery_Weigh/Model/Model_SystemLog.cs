using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_SystemLog
    {
        public int Id { get; set; }
        public int _Id;
        public string UserId;
        public string PlantCode;
        public string LogTitle;
        public string LogDescription;
        public string URL;
        public System.Nullable<System.DateTime> LogDate;
    }
}