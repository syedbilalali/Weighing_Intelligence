using Avery_Weigh.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Repository
{
    public class SystemLogRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public void SaveSystemLog(Model_SystemLog log)
        {
            Log _log = new Log();
            _log.LogDate = DateTime.Now;
            _log.LogDescription = log.LogDescription;
            _log.LogTitle = log.LogTitle;
            _log.PlantCode = HttpContext.Current.Session["WBID"].ToString();
            _log.URL = log.URL;
            _log.UserId = HttpContext.Current.Session["UserName"].ToString();
            db.Logs.InsertOnSubmit(_log);
            db.SubmitChanges();
        }
    }
}