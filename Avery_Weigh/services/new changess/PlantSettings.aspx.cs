using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Avery_Weigh.Model;
using Avery_Weigh.Repository;

namespace Avery_Weigh
{
    public partial class PlantSettings : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        PlantmasterRepository _plantrepo = new PlantmasterRepository();
        WeightMachinMasterRepository _wmrepo = new WeightMachinMasterRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/login.aspx");
            IEnumerable<SiteParameterSetting> setting = db.SiteParameterSettings.ToList();
            if (setting.Count() >= 3)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('Multiple Record found. Please contact with service engineer.');", true);
               
            }
            if (!IsPostBack)
            {
                BindData();
                Get_PlantCode();
            }
        }

        private void BindData()
        {
            SiteParameterSetting setting = db.SiteParameterSettings.FirstOrDefault();
            if (setting != null)
            {
                ddlAlphaNumericDisplay.SelectedValue = ddlAlphaNumericDisplay.Items.FindByValue(setting.AlphaNumericDisplay.Value.ToString()).Value;
                ddlAuthorizeforTare.SelectedValue = ddlAuthorizeforTare.Items.FindByValue(setting.AuthorizedForTARE.Value.ToString()).Value;
                ddlAxleWeighting.SelectedValue = ddlAxleWeighting.Items.FindByValue(setting.AxleWeighting.Value.ToString()).Value;
                ddlBarriers.SelectedValue = ddlBarriers.Items.FindByValue(setting.Barriers.Value.ToString()).Value;
                ddlCamera.SelectedValue = ddlCamera.Items.FindByValue(setting.Cameras.Value.ToString()).Value;
                ddlConnectivityToCustomers.SelectedValue = ddlConnectivityToCustomers.Items.FindByValue(setting.ConnectivityToCustomer.Value.ToString()).Value;
                ddlgateEntry.SelectedValue = ddlgateEntry.Items.FindByValue(setting.IsGateEntry.Value.ToString()).Value;
                ddlNoSpecialCharacter.SelectedValue = ddlNoSpecialCharacter.Items.FindByValue(setting.NoSpectalCharacterForTruck.Value.ToString()).Value;
                ddlPASystem.SelectedValue = ddlPASystem.Items.FindByValue(setting.PASystem.Value.ToString()).Value;
                ddlRFIDReader.SelectedValue = ddlRFIDReader.Items.FindByValue(setting.RFIDReader.Value.ToString()).Value;
                ddlSendors.SelectedValue = ddlSendors.Items.FindByValue(setting.Sensors.Value.ToString()).Value;
                ddlTMS.SelectedValue = ddlTMS.Items.FindByValue(setting.TMS.Value.ToString()).Value;
                ddlCusTolerance.SelectedValue = ddlCusTolerance.Items.FindByValue(setting.ToleranceCheckforCustQty.Value.ToString()).Value;
                ddlSupTolerance.SelectedValue = ddlSupTolerance.Items.FindByValue(setting.ToleranceCheckforSupQty.Value.ToString()).Value;
            }
        }

        //Get:PlantCode from PlantMaster
        protected void Get_PlantCode()
        {
            IEnumerable<Model_PlantMaster> data = _plantrepo.Get_PlantCodeId();
            if (data != null)
            {
                ddlplantCode.DataTextField = "PlantName";
                ddlplantCode.DataValueField = "PlantCode";
                ddlplantCode.DataSource = data;
                ddlplantCode.DataBind();
                ddlplantCode.Items.Insert(0, new ListItem("Select", ""));
            }
        }

        //Get:MachineId By PlantCode on plantcode dropdown selection change
        protected void ddlplantcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlplantCode.SelectedValue))
            {
                IEnumerable<Model_WeightMachinMaster> data = _wmrepo.Get_MachineIdBy_PlantCode(ddlplantCode.SelectedValue);
                if (data.Count() > 0)
                {
                    ddlMachineId.DataTextField = "MachineId";
                    ddlMachineId.DataValueField = "MachineId";
                    ddlMachineId.DataSource = data;
                    ddlMachineId.DataBind();
                    ddlMachineId.Items.Insert(0, new ListItem("Select", ""));
                }
                else
                {
                    ddlMachineId.Items.Clear();
                    ddlMachineId.Items.Insert(0, new ListItem("Not Available", ""));
                }
            }
        }

       
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string varPlantCode = string.Empty;
            string varMachineId = string.Empty;

            bool varPlantCode_Exist = false;

            if (ddlplantCode.SelectedItem.Value == "0")
            {
                varPlantCode = ddlplantCode.SelectedItem.Value;
                varPlantCode = "";
            }
            else
            {
                varPlantCode = ddlplantCode.SelectedItem.Value;
                
            }
            varMachineId = ddlMachineId.SelectedItem.Value;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select * from SiteParameterSettings where PlantCodeId='" + varPlantCode + "' and WeightMachineId='" + varMachineId + "'", con))
                {
                    using (SqlDataAdapter ds = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dtbl = new DataTable())
                        {
                            ds.Fill(dtbl);

                            if (dtbl.Rows.Count > 0)
                            {

                                varPlantCode_Exist = true;

                            }
                            else
                            {
                                varPlantCode_Exist = false;
                            }
                        }

                    }
                }

            }

            try
            {
                int id = 0;
                SiteParameterSetting setting = db.SiteParameterSettings.FirstOrDefault();  // (x => x.PlantCodeId == varPlantCode && x.WeightMachineId == varMachineId);

                if (varPlantCode_Exist == false)  // setting == null)
                {

                    AddSiteParameters(varPlantCode,varMachineId );

                    //setting.PlantCodeId = varPlantCode;
                    
                    //setting.WeightMachineId = varMachineId;
                    //setting.AlphaNumericDisplay = Convert.ToInt32(ddlAlphaNumericDisplay.SelectedItem.Value);
                    //setting.AuthorizedForTARE = Convert.ToInt32(ddlAuthorizeforTare.SelectedItem.Value);
                    //setting.AxleWeighting = Convert.ToInt32(ddlAxleWeighting.SelectedItem.Value);
                    //setting.Barriers = Convert.ToInt32(ddlBarriers.SelectedItem.Value);
                    //setting.Cameras = Convert.ToInt32(ddlCamera.SelectedItem.Value);
                    //setting.ConnectivityToCustomer = Convert.ToInt32(ddlConnectivityToCustomers.SelectedItem.Value);
                    //setting.CreateDate = DateTime.Now;
                    //setting.IsGateEntry = Convert.ToInt32(ddlgateEntry.SelectedItem.Value);
                    //setting.NoSpectalCharacterForTruck = Convert.ToInt32(ddlNoSpecialCharacter.SelectedItem.Value);
                    //setting.PASystem = Convert.ToInt32(ddlPASystem.SelectedItem.Value);
                    //setting.RFIDReader = Convert.ToInt32(ddlRFIDReader.SelectedItem.Value);
                    //setting.Sensors = Convert.ToInt32(ddlSendors.SelectedItem.Value);
                    //setting.TMS = Convert.ToInt32(ddlTMS.SelectedItem.Value);
                    //setting.ToleranceCheckforCustQty = Convert.ToInt32(ddlCusTolerance.SelectedItem.Value);
                    //setting.ToleranceCheckforSupQty = Convert.ToInt32(ddlSupTolerance.SelectedItem.Value);
                    //db.SiteParameterSettings.InsertOnSubmit(setting);
                    //db.SubmitChanges();
                }
                else
                {
                    setting.PlantCodeId = varPlantCode;

                    setting.WeightMachineId = varMachineId;
                    setting.AlphaNumericDisplay = Convert.ToInt32(ddlAlphaNumericDisplay.SelectedItem.Value);
                    setting.AuthorizedForTARE = Convert.ToInt32(ddlAuthorizeforTare.SelectedItem.Value);
                    setting.AxleWeighting = Convert.ToInt32(ddlAxleWeighting.SelectedItem.Value);
                    setting.Barriers = Convert.ToInt32(ddlBarriers.SelectedItem.Value);
                    setting.Cameras = Convert.ToInt32(ddlCamera.SelectedItem.Value);
                    setting.ConnectivityToCustomer = Convert.ToInt32(ddlConnectivityToCustomers.SelectedItem.Value);
                    setting.CreateDate = DateTime.Now;
                    setting.IsGateEntry = Convert.ToInt32(ddlgateEntry.SelectedItem.Value);
                    setting.NoSpectalCharacterForTruck = Convert.ToInt32(ddlNoSpecialCharacter.SelectedItem.Value);
                    setting.PASystem = Convert.ToInt32(ddlPASystem.SelectedItem.Value);
                    setting.RFIDReader = Convert.ToInt32(ddlRFIDReader.SelectedItem.Value);
                    setting.Sensors = Convert.ToInt32(ddlSendors.SelectedItem.Value);
                    setting.TMS = Convert.ToInt32(ddlTMS.SelectedItem.Value);
                    setting.ToleranceCheckforCustQty = Convert.ToInt32(ddlCusTolerance.SelectedItem.Value);
                    setting.ToleranceCheckforSupQty = Convert.ToInt32(ddlSupTolerance.SelectedItem.Value);
                    db.SubmitChanges();
                }
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Settings saved successfully.');", true);
            }
            catch(Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('" + ex.Message.ToString() + "');", true);
            }
        }

        private void AddSiteParameters(string varPlantCode,string varMachineId)
        {
            // SqlConnection con = new SqlConnection();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(@"INSERT INTO SiteParameterSettings
                      (IsGateEntry, Cameras, Sensors, AlphaNumericDisplay, PlantCodeId, WeightMachineId, CreateDate, ToleranceCheckforSupQty, ToleranceCheckforCustQty, AuthorizedForTARE, TMS,
                      AxleWeighting, NoSpectalCharacterForTruck, ConnectivityToCustomer, RFIDReader, PASystem, Barriers)
                        VALUES( @IsGateEntry, @Cameras, @Sensors, @AlphaNumericDisplay, @PlantCodeId, @WeightMachineId, GETDATE(), @ToleranceCheckforSupQty, @ToleranceCheckforCustQty, @AuthorizedForTARE, @TMS,
                      @AxleWeighting, @NoSpectalCharacterForTruck, @ConnectivityToCustomer, @RFIDReader, @PASystem, @Barriers)",con);
        

            cmd.Parameters.AddWithValue("@PlantCodeId", varPlantCode);

            cmd.Parameters.AddWithValue("@WeightMachineId", varMachineId);
            cmd.Parameters.AddWithValue("@AlphaNumericDisplay", Convert.ToInt32(ddlAlphaNumericDisplay.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@AuthorizedForTARE", Convert.ToInt32(ddlAuthorizeforTare.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@AxleWeighting", Convert.ToInt32(ddlAxleWeighting.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@Barriers", Convert.ToInt32(ddlBarriers.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@Cameras", Convert.ToInt32(ddlCamera.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@ConnectivityToCustomer", Convert.ToInt32(ddlConnectivityToCustomers.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@IsGateEntry", Convert.ToInt32(ddlgateEntry.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@NoSpectalCharacterForTruck", Convert.ToInt32(ddlNoSpecialCharacter.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@PASystem", Convert.ToInt32(ddlPASystem.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@RFIDReader", Convert.ToInt32(ddlRFIDReader.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@Sensors", Convert.ToInt32(ddlSendors.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@TMS", Convert.ToInt32(ddlTMS.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@ToleranceCheckforCustQty", Convert.ToInt32(ddlCusTolerance.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@ToleranceCheckforSupQty", Convert.ToInt32(ddlSupTolerance.SelectedItem.Value));
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
