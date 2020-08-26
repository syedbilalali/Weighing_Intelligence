using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Avery_Weigh.View
{
    public partial class Header : System.Web.UI.UserControl
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                txtoperatorName.Text = Session["UserName"].ToString();
                txtInstalledOn.Text = Session["InstalledOn"].ToString();
                txtPlantId.Text = Session["PlantID"].ToString();
                txtWBId.Text = Session["WBID"].ToString();
                CompanyMaster company = db.CompanyMasters.FirstOrDefault(x => x.Id == 1);
                if (company != null)
                {
                    if (!string.IsNullOrEmpty(company.CompanyLogo))
                    {
                        imgLogo.ImageUrl = "/images/companylogo/" + company.CompanyLogo;
                    }
                }
            }
        }
    }
}