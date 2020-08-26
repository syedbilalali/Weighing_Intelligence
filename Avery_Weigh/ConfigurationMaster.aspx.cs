using Avery_Weigh.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Avery_Weigh
{
    public partial class ConfigurationMaster : System.Web.UI.Page
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DataClasses1DataContext db = new DataClasses1DataContext();
        MaterialRepository _materialrepo = new MaterialRepository();
        SupplierRepository _supplierrepo = new SupplierRepository();
        TransporterRepository _transrepo = new TransporterRepository();
        PackingRepository _Packingrepo = new PackingRepository();
        TransactionRepository _transactionRepo = new TransactionRepository();
        UserMasterRepository umRepo = new UserMasterRepository();
        DataClasses1DataContext db1 = new DataClasses1DataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
               

            //    getuserAccess();
            //}
        }

        private void getuserAccess()
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/Login");

            int userid = Convert.ToInt32(User.Identity.Name);
            UserClassification uc = umRepo.GetUserAuthorization(userid);
            if (uc != null)
            {
                #region check Weighing page access
                if (uc.Weighment == false)
                    WeighMenu.Style.Add("display", "none");
                //else if (uc. == false)
                //     ManageMasters.Style.Add("display", "none");
                else
                    WeighMenu.Style.Add("display", "block");
                #endregion
                #region check Configuration page access
                if (uc.Weighment == false)
                    configurationMenu.Style.Add("display", "none");
                else
                    configurationMenu.Style.Add("display", "block");
                #endregion
                #region check Configuration page access
                if (uc.Weighment == false)
                    configurationMenu.Style.Add("display", "none");
                else
                    configurationMenu.Style.Add("display", "block");
                #endregion
            }


        }
    }
}