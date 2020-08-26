using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Avery_Weigh
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Filldata();

            }
        }

        private void Filldata()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DailyDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    //rptList.DataSource = dt;
                                    //rptList.DataBind();
                                    GridView1.DataSource = dt;
                                    GridView1.DataBind();
                                }

                            }

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('"+ ex + "');", true);
            }

            }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label l = e.Item.FindControl("lblid") as Label;
            if (l != null)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }
    }
}