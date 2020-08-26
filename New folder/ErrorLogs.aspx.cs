using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Avery_Weigh.Model;
using System.IO;

namespace Avery_Weigh
{
    public partial class RePrintTicket : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


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
                    if (ddlTransactionType.SelectedValue == "2")
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@Option", "DateWise");

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                using (DataTable dt = new DataTable())
                                {
                                    da.Fill(dt);
                                    if (dt.Rows.Count > 0)
                                    {
                                        rptList.DataSource = dt;
                                        rptList.DataBind();
                                    }
                                    else
                                    {
                                        rptList.DataSource = null;
                                        rptList.DataBind();
                                    }

                                }

                            }
                        }
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@Option", "DateWisePendingTransaction");

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                using (DataTable dt = new DataTable())
                                {
                                    da.Fill(dt);
                                    if (dt.Rows.Count > 0)
                                    {
                                        rptList.DataSource = dt;
                                        rptList.DataBind();
                                    }
                                    else
                                    {
                                        rptList.DataSource = null;
                                        rptList.DataBind();
                                    }

                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex + "');", true);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Filldata();
        }
        static string tripid;
        protected void checkRecord_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            RepeaterItem item = (RepeaterItem)chk.NamingContainer;

            Label lblTripId = (Label)item.FindControl("lblTripId");
            tripid = lblTripId.Text;
        }

        protected void linkPrint_Click(object sender, EventArgs e)
        {
            string strTripId = tripid;
            if (strTripId == "0")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('No Records');", true);
            else
            {
                if (File.Exists(Server.MapPath("~/pdfs/" + strTripId + ".pdf")))
                {
                    File.Delete(Server.MapPath("~/pdfs/" + strTripId + ".pdf"));
                }

                Ticket ts = new Ticket();
                ts.GetTicket(Convert.ToInt32(strTripId));
                //lnkPrint.OnClientClick = "target='_blank'";
                //Response.Redirect("~/pdfs/" + strTripId + ".pdf");
                //lnkPrint.Attributes.Add("href",String.Format("/pdfs/" + strTripId + ".pdf"));
                //lnkPrint.Attributes.Add("target","_blank");
                var varurl = "/pdfs/" + strTripId + ".pdf";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('" + varurl + "','_newtab');", true);

                //new line added for duplicate print

                tblTransaction _trantkt = db.tblTransactions.FirstOrDefault(x => x.TripId == Convert.ToInt32(strTripId) && x.WeighbridgeId == Session["WBID"].ToString());

                if (_trantkt != null && _trantkt.SecondWeight != null)
                {
                    // _trantkt.print_ticket = "Y";
                    _trantkt.PRINT_TICKET = "Y";
                    db.SubmitChanges();
                }

                //end of duplicate
            }
        }
    }
}