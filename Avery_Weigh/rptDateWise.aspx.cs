using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using Avery_Weigh.Repository;

namespace Avery_Weigh
{
    public partial class rptDateWise : System.Web.UI.Page
    {
         static DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Weighment.Disabled = true;
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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex + "');", true);
            }

        }

        private void FillMaterialdata()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Type", ddlValueType.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@Option", "FillMaterialData");
                      

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

            rptList.DataSource = null;
            rptList.DataBind();

            if (ddlType.SelectedIndex == 0)

            {
                Filldata();

            }
          else  if (ddlType.SelectedIndex != 0)

            {

                if (ddlType.SelectedValue.ToString() == "MaterialName")
                {

                    FillMaterialdata();
                }
                else if (ddlType.SelectedValue.ToString() == "PartyName")
                {

                    FillPartyNameData();
                }
                else if (ddlType.SelectedValue.ToString() == "Transporter")
                {

                    FillTransporterNameData();
                }
                else if (ddlType.SelectedValue.ToString() == "VehicleNo")
                {

                    FillVehicleNoData();
                }
                else if (ddlType.SelectedValue.ToString() == "PlantCode")
                {

                    FillPlantCodeData();
                }
                else if (ddlType.SelectedValue.ToString() == "WeighbridgeId")
                {

                    FillWeighbridgeIdData();
                }
                else if (ddlType.SelectedValue.ToString() == "CreatedBy")
                {

                    FillCreatedByData();
                }
                else if (ddlType.SelectedValue.ToString() == "Shift")
                {

                    FillCreatedByShift();
                }


            }


        }

        private void FillCreatedByData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Type", ddlValueType.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@Option", "FillCreatedByData");


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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex + "');", true);
            }
        }

        private void FillCreatedByShift()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Type", ddlValueType.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@Option", "FillCreatedByShift");


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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex + "');", true);
            }
        }

        private void FillWeighbridgeIdData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Type", ddlValueType.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@Option", "FillWeighbridgeIdData");


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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex + "');", true);
            }
        }

        private void FillPlantCodeData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Type", ddlValueType.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@Option", "FillPlantCodeData");


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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex + "');", true);
            }
        }

        private void FillVehicleNoData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Type", ddlValueType.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@Option", "FillVehicleNoData");


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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex + "');", true);
            }
        }

        private void FillTransporterNameData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Type", ddlValueType.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@Option", "FillTransporterData");


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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex + "');", true);
            }

        }

        private void FillPartyNameData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Type", ddlValueType.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@Option", "FillPartyData");


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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex + "');", true);
            }

        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlValueType.Items.Clear();
            ddlValueType.DataSource = null;
            ddlValueType.DataBind();
           
            if (ddlType.SelectedValue.ToString() == "MaterialName")
            {
               
                FillMaterial();
            }
           else if (ddlType.SelectedValue.ToString() == "PartyName")
            {

                FillPartyName();
            }
            else if (ddlType.SelectedValue.ToString() == "Transporter")
            {
              
                FillTransporterName();
            }
            else if (ddlType.SelectedValue.ToString() == "VehicleNo")
            {
               
                FillVehicleNo();
            }
            else if (ddlType.SelectedValue.ToString() == "PlantCode")
            {

                FillPlantCode();
            }
            else if (ddlType.SelectedValue.ToString() == "WeighbridgeId")
            {

                FillWeighbridgeId();
            }
            else if (ddlType.SelectedValue.ToString() == "CreatedBy")
            {

                FillCreatedBy();
            }
            else if (ddlType.SelectedValue.ToString() == "Shift")
            {

                FillShift();
            }

        }

        private void FillCreatedBy()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Option", "FillCreatedBy");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    ddlValueType.DataSource = dt;
                                    ddlValueType.DataTextField = "CreatedBy";
                                    ddlValueType.DataValueField = "CreatedBy";
                                    ddlValueType.DataBind();
                                }
                                else
                                {
                                    ddlValueType.DataSource = null;
                                    ddlValueType.DataBind();
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

        private void FillShift()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Option", "FillShift");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    ddlValueType.DataSource = dt;
                                    ddlValueType.DataTextField = "Shift";
                                    ddlValueType.DataValueField = "Shift";
                                    ddlValueType.DataBind();
                                }
                                else
                                {
                                    ddlValueType.DataSource = null;
                                    ddlValueType.DataBind();
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

        private void FillWeighbridgeId()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Option", "FillWeighbridgeId");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    ddlValueType.DataSource = dt;
                                    ddlValueType.DataTextField = "WeighbridgeId";
                                    ddlValueType.DataValueField = "WeighbridgeId";
                                    ddlValueType.DataBind();
                                }
                                else
                                {
                                    ddlValueType.DataSource = null;
                                    ddlValueType.DataBind();
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

        private void FillPlantCode()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Option", "FillPlantCode");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    ddlValueType.DataSource = dt;
                                    ddlValueType.DataTextField = "PlantCode";
                                    ddlValueType.DataValueField = "PlantCode";
                                    ddlValueType.DataBind();
                                }
                                else
                                {
                                    ddlValueType.DataSource = null;
                                    ddlValueType.DataBind();
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

        private void FillVehicleNo()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Option", "FillVehicleNo");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                ddlValueType.DataSource = dt;
                                ddlValueType.DataTextField = "TruckNo";
                                ddlValueType.DataValueField = "TruckNo";
                                ddlValueType.DataBind();
                            }
                            else
                            {
                                ddlValueType.DataSource = null;
                                ddlValueType.DataBind();
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

        private void FillTransporterName()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Option", "FillTransporterName");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                                if (dt.Rows.Count > 0)
                            {
                                ddlValueType.DataSource = dt;
                                ddlValueType.DataTextField = "TransporterName";
                                ddlValueType.DataValueField = "TransporterName";
                                ddlValueType.DataBind();
                            }
                            else
                            {
                                ddlValueType.DataSource = null;
                                ddlValueType.DataBind();
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

        private void FillPartyName()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Option", "FillPartyName");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                ddlValueType.DataSource = dt;
                                ddlValueType.DataTextField = "SupplierName";
                                ddlValueType.DataValueField = "SupplierName";
                                ddlValueType.DataBind();
                            }
                            else
                            {
                                ddlValueType.DataSource = null;
                                ddlValueType.DataBind();
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

        private void FillMaterial()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    using (SqlCommand cmd = new SqlCommand("sp_DateWiseDashboard", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Option", "FillMaterial");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                ddlValueType.DataSource = dt;
                                ddlValueType.DataTextField = "MaterialName";
                                ddlValueType.DataValueField = "MaterialName";
                                ddlValueType.DataBind();
                            }
                            else
                            {
                                ddlValueType.DataSource = null;
                                ddlValueType.DataBind();
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

        protected void BtnLinkExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
        //Export:Data into Excel File
        private void ExportToExcel()
        {
            // DataTable dt = repo.Get_SupplierDataTable();
            //if (dt.Rows.Count > 0)
            //{
            //    using (XLWorkbook wb = new XLWorkbook())
            //    {
            //        wb.Worksheets.Add(dt, "Supplier");
            //        Response.Clear();
            //        Response.Buffer = true;
            //        Response.Charset = "";
            //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //        Response.AddHeader("content-disposition", "attachment;filename=Supplier.xlsx");
            //        using (MemoryStream memoryStream = new MemoryStream())
            //        {
            //            wb.SaveAs(memoryStream);
            //            memoryStream.WriteTo(Response.OutputStream);
            //            Response.Flush();
            //            Response.End();
            //        }
            //    }
            //}

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=datewisereports.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            //     Your Repeater Name Mine is "Rep"
            rptList.RenderControl(htmlWrite);
            Response.Write("<table>");
            Response.Write(stringWrite.ToString());
            Response.Write("</table>");
            Response.End();

        }

        [Obsolete]
        TransactionRepository _transRepo = new TransactionRepository();
        PlantmasterRepository _plantRepo = new PlantmasterRepository();

        protected void lnkprintPDF_Click(object sender, EventArgs e)
        {
            
               
                PlantMaster _plant = _plantRepo.getplantByWeighingMachine(Session["PlantID"].ToString(), "com1");
            //BaseFont bfR = iTextSharp.text.pdf.BaseFont.CreateFont(BaseFont.TIMES_ROMAN, iTextSharp.text.pdf.BaseFont.CP1257, iTextSharp.text.pdf.BaseFont.EMBEDDED);
            //PdfWriter writer = PdfWriter.GetInstance(document, fs);
            StyleSheet style1 = new StyleSheet();
            style1.LoadTagStyle(HtmlTags.TABLE, HtmlTags.FONTSIZE, "8px");
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=DateWiseReports.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddStyleAttribute("font-size", "8pt");
            // rptList.DataBind();
            Panel1.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
           // pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            var writer=PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            string CompanyLogo = HttpContext.Current.Server.MapPath(".") + @"\images\companylogo\logo.jpg";
            string header1 = HttpContext.Current.Server.MapPath(".") + @"\images\header1.png";
            string header2 = HttpContext.Current.Server.MapPath(".") + @"\images\header2.png";
            //imgLogo.ImageUrl = "/images/companylogo/" + company.CompanyLogo;
            //added on 24-10-2019
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            var boldTableFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD);
            var NORMALFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);

            PdfPTable table1 = new PdfPTable(4);
            table1.WidthPercentage = 100;
            table1.SetWidths(new float[] { 0.10f, 0.13f, 0.15f, 0.20f });
            //First Row
            PdfPCell cellheaderleft = new PdfPCell();
            cellheaderleft.Border = 0;
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(header1);
            image.ScaleAbsolute(230f, 200f);
            image.Alignment = Element.HEADER;
            cellheaderleft.AddElement(image);
            table1.AddCell(cellheaderleft);
            cellheaderleft = new PdfPCell();
            cellheaderleft.Border = 0;
            image.ScaleAbsolute(230f, 200f);
            image.Alignment = Element.HEADER;
            cellheaderleft.AddElement(image);
            table1.AddCell(cellheaderleft);
            cellheaderleft = new PdfPCell();
            cellheaderleft.Border = 0;
            image.ScaleAbsolute(230f, 200f);
            image.Alignment = Element.HEADER;
            cellheaderleft.AddElement(image);
            table1.AddCell(cellheaderleft);
            PdfPCell cellheaderright = new PdfPCell();
            cellheaderright.Border = 0;
            cellheaderright.Rowspan = 2;
            iTextSharp.text.Image image2 = iTextSharp.text.Image.GetInstance(header2);
            image2.ScaleAbsolute(190f, 500f);
            image2.Alignment = Element.HEADER;
            cellheaderright.AddElement(image2);
            table1.AddCell(cellheaderright);

            //2nd Row

            cellheaderleft = new PdfPCell();
            cellheaderleft.Border = 0;
            iTextSharp.text.Image logoImage = iTextSharp.text.Image.GetInstance(CompanyLogo);
            image.ScaleAbsolute(250f, 200f);
            image.Alignment = Element.HEADER;
            cellheaderleft.AddElement(logoImage);
            table1.AddCell(cellheaderleft);
            cellheaderleft = new PdfPCell();
            cellheaderleft.Border = 0;
            table1.AddCell(cellheaderleft);

            string PlantCodeAddress = _plant.PlantName + "\n" + _plant.PlantAddress1 + "\n" + _plant.PlantAddress2;
            var p = new Paragraph(PlantCodeAddress, boldTableFont);
            cellheaderleft = new PdfPCell();

            cellheaderleft.Border = 0;
            cellheaderleft.HorizontalAlignment = Element.ALIGN_CENTER;
            cellheaderleft.AddElement(p);
            table1.AddCell(cellheaderleft);


            PdfPCell Reportname = new PdfPCell(new Phrase("Date wise Report", boldTableFont));
            Reportname.HorizontalAlignment = Element.ALIGN_LEFT;
            Reportname.Padding = 5;
            Reportname.Colspan = 2;
            table1.AddCell(Reportname);



            PdfPCell tripdatetime = new PdfPCell(new Phrase("Print Date/Time  :", boldTableFont));
            tripdatetime.HorizontalAlignment = Element.ALIGN_RIGHT;
          


            table1.AddCell(tripdatetime);

            PdfPCell tripdatetimeValue = new PdfPCell(new Phrase(DateTime.Now.ToString(), NORMALFont));
            tripdatetimeValue.HorizontalAlignment = Element.ALIGN_LEFT;
           
          
            table1.AddCell(tripdatetimeValue);

            pdfDoc.Add(table1);



            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
    }
}