using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Avery_Weigh
{
    public partial class Shift_Master : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                    Response.Redirect("/login.aspx");
                bindData();
            }
        }

        private void bindData()
        {
           
                SHIFTTIME  _shift = db.SHIFTTIMEs.FirstOrDefault(x => x.Id == 1);
                if (_shift != null)
                {
                    ddlShiftId.Text = _shift.NOOFSHIFTS;
                    dtStartTimeA.Text = _shift.STA;
                    dtEndTimeA.Text = _shift.EDA;
                    dtStartTimeB.Text = _shift.STB;
                    dtEndTimeB.Text = _shift.EDB;
                    dtStartTimeC.Text = _shift.STC;
                    dtEndTimeC.Text = _shift.EDC;
                    
                }
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SHIFTTIME _shift = db.SHIFTTIMEs.FirstOrDefault(x => x.Id == 1);
                if (_shift == null)
                {
                    _shift = new SHIFTTIME();
                     _shift.NOOFSHIFTS= ddlShiftId.Text.Trim();
                    _shift.STA=dtStartTimeA.Text.Trim();
                    _shift.EDA=dtEndTimeA.Text.Trim();
                    _shift.STB=dtStartTimeB.Text.Trim();
                    _shift.EDB=dtEndTimeB.Text.Trim();
                    _shift.STC=dtStartTimeC.Text.Trim();
                    _shift.EDC=dtEndTimeC.Text.Trim();
                    
                    db.SHIFTTIMEs.InsertOnSubmit(_shift);
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Shift record added successfully.');", true);
                }
                else
                {
                    _shift.NOOFSHIFTS = ddlShiftId.Text.Trim();
                    _shift.STA = dtStartTimeA.Text.Trim();
                    _shift.EDA = dtEndTimeA.Text.Trim();
                    _shift.STB = dtStartTimeB.Text.Trim();
                    _shift.EDB = dtEndTimeB.Text.Trim();
                    _shift.STC = dtStartTimeC.Text.Trim();
                    _shift.EDC = dtEndTimeC.Text.Trim();
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Company record updated successfully.');", true);
                }
                db.SubmitChanges();
                bindData();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('" + ex.Message.ToString() + "');", true);
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            try
            {
                SHIFTTIME _shift = db.SHIFTTIMEs.FirstOrDefault(x => x.Id == 1);
                if (_shift == null)
                {
                    _shift = new SHIFTTIME();
                    _shift.NOOFSHIFTS = ddlShiftId.Text.Trim();
                    _shift.STA = dtStartTimeA.Text.Trim();
                    _shift.EDA = dtEndTimeA.Text.Trim();
                    _shift.STB = dtStartTimeB.Text.Trim();
                    _shift.EDB = dtEndTimeB.Text.Trim();
                    _shift.STC = dtStartTimeC.Text.Trim();
                    _shift.EDC = dtEndTimeC.Text.Trim();

                    db.SHIFTTIMEs.InsertOnSubmit(_shift);
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Shift record added successfully.');", true);
                }
                else
                {
                    _shift.NOOFSHIFTS = ddlShiftId.Text.Trim();
                    _shift.STA = dtStartTimeA.Text.Trim();
                    _shift.EDA = dtEndTimeA.Text.Trim();
                    _shift.STB = dtStartTimeB.Text.Trim();
                    _shift.EDB = dtEndTimeB.Text.Trim();
                    _shift.STC = dtStartTimeC.Text.Trim();
                    _shift.EDC = dtEndTimeC.Text.Trim();
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Shift record updated successfully.');", true);
                }
                db.SubmitChanges();
                bindData();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('" + ex.Message.ToString() + "');", true);
            }
        }
    }
}