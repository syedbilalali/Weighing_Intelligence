using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Avery_Weigh
{
    public partial class Logout : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoggedOutUser();
            }
            Session["UserName"] = null;
            Session["Password"] = null;
            Session["WBID"] = null;
            Session["PlantID"] = null;
            FormsAuthentication.SignOut();
            Response.Redirect("/Login.aspx");
        }

        private void LoggedOutUser()
        {
            if (Session["WBID"] != null)
            {
                try
                {
                    LoggedUser tblloged = db.LoggedUsers.FirstOrDefault(x => x.MachineId == Session["WBID"].ToString() && x.IsDeleted==false );
                    tblloged.LogOutUser = true;
                    tblloged.LogOutUserDateTime = DateTime.Now;
                    tblloged.IsDeleted = true;
                    db.SubmitChanges();
                }
                catch { }
            }
        }
    }
}