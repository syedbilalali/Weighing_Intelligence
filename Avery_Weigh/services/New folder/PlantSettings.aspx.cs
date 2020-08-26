using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Avery_Weigh
{
    public partial class PlantSettings : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/login.aspx");
            IEnumerable<SiteParameterSetting> setting = db.SiteParameterSettings.ToList();
            if (setting.Count() >= 2)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('Multiple Record found. Please contact with service engineer.');", true);
               
            }
            if (!IsPostBack)
            {
                BindData();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int id = 0;
                SiteParameterSetting setting = db.SiteParameterSettings.FirstOrDefault();
                if (setting == null)
                {
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
                    db.SiteParameterSettings.InsertOnSubmit(setting);
                    db.SubmitChanges();
                }
                else
                {
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
    }
}
