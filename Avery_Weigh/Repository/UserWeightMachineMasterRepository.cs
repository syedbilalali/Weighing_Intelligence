using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Repository
{
    public class UserWeightMachineMasterRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        //Add:UserWeightMachineMaster
        public bool Add_UserWeightMachineMaster(UserWeightMachineMaster user)
        {
            bool status = false;
            if (user != null)
            {
                db.UserWeightMachineMasters.InsertOnSubmit(user);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }
    }
}