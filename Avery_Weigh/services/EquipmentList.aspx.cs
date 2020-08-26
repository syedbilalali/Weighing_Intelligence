using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Avery_Weigh.services
{
    public partial class EquipmentList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Id");

                DataRow row = dt.NewRow();
                row[0] = "1";
                dt.Rows.Add(row);
                row = dt.NewRow();
                row[0] = "2";
                dt.Rows.Add(row);
                row = dt.NewRow();
                row[0] = "3";
                dt.Rows.Add(row);

                rptList.DataSource = dt;
                rptList.DataBind();
            }
        }
    }
}