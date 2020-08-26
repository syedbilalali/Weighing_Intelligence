using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;

namespace Avery_Weigh.WeightMachinMaster
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        WeightMachinMasterRepository _repo = new WeightMachinMasterRepository();
        PlantmasterRepository plantrepo = new PlantmasterRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_PlantCode();
                GetWTMachinMasterForEdit();
            }
        }

        //Get:Plant Code from plant master
        protected void Get_PlantCode()
        {
            ddlPlantId.DataTextField = "PlantName";
            ddlPlantId.DataValueField = "PlantCode";
            ddlPlantId.DataSource = plantrepo.Get_PlantCodeId();
            ddlPlantId.DataBind();
            ddlPlantId.Items.Insert(0, new ListItem("Select", ""));
        }

        //Add:New Record
        private void Add()
        {
            if (Convert.ToDateTime(txtinstallationdate.Text) > Convert.ToDateTime(txtwarrantyupto.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Installation date should be less than warranty date');", true);
                return;
            }

            TimeSpan t = Convert.ToDateTime(txtwarrantyupto.Text).Subtract(Convert.ToDateTime(txtinstallationdate.Text));

            if (t.TotalDays < 365)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('warranty should be at least 1 years');", true);
                return;
            }

            if (Convert.ToDateTime(txtdispatchdate.Text) > Convert.ToDateTime(txtinstallationdate.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Dispatch date should be less than Installation date');", true);
                return;
            }

            try
            {
                var _data = _repo.GetMachineMaster_ByMachineId(txtMachinId.Text.Trim());
                if (_data != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Machin Id Exist Please Try Again');", true);
                }
                else
                {
                    WeightMachineMaster wm = new WeightMachineMaster();
                    wm.PlantCodeId = ddlPlantId.SelectedValue;
                    wm.MachineId = txtMachinId.Text;
                    wm.Capacity = txtCapacity.Text.ToString() + ddlcapacityuom.SelectedValue.ToString();
                    wm.Resolution = txtResolution.Text.ToString() + ddlresolutionuom.SelectedValue.ToString();
                    wm.Model = txtModel.Text.ToString();
                    wm.PlatformSize = txtPlatformsize.Text.ToString();
                    wm.MachineNo = txtmachinno.Text.ToString();
                    wm.Indicator = txtindicator.Text.ToString();
                    wm.LCType = txtlctype.Text.ToString();
                    wm.NoOfLoadCells = txtnoofloadcells.Text.ToString();
                    wm.LoadCellSerialNos = txtnoofloadcells.Text.ToString();
                    wm.EquipmentId = Convert.ToInt32(txtequipment.Text);
                    wm.InvoiceNo = txtinvoiceno.Text.ToString();
                    DateTime dispatchdate = DateTime.ParseExact(txtdispatchdate.Text, "dd/MM/yyyy", new CultureInfo("en-GB"));
                    wm.DespatchDate = dispatchdate;
                    DateTime InstallationDate = DateTime.ParseExact(txtinstallationdate.Text, "dd/MM/yyyy", new CultureInfo("en-GB"));
                    wm.InstallationDate = InstallationDate;
                    DateTime WarrentyUpto = DateTime.ParseExact(txtwarrantyupto.Text, "dd/MM/yyyy", new CultureInfo("en-GB"));
                    wm.WarrentyUpto = WarrentyUpto;
                    wm.ReasonWarrentyUptoDate = txtrowud.Text.ToString();
                    wm.WeighingUnit = this.ddlcapacityuom.Text.Trim().ToString();
                    wm.IsDeleted = false;
                    if (_repo.Add_WeightMachineMaster(wm))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Save Successfully');", true);
                        HtmlMeta meta = new HtmlMeta();
                        meta.HttpEquiv = "Refresh";
                        meta.Content = "1;url=AddEdit.aspx";
                        this.Page.Controls.Add(meta);
                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
            }
        }

        //Update:Weight Machine Master
        private void Edit()
        {
            if (Convert.ToDateTime(txtinstallationdate.Text)> Convert.ToDateTime(txtwarrantyupto.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Installation date should be less than warranty date');", true);
                return;
            }

            TimeSpan t = Convert.ToDateTime(txtwarrantyupto.Text).Subtract(Convert.ToDateTime(txtinstallationdate.Text));

            if (t.TotalDays<365)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('warranty should be at least 1 years');", true);
                return;
            }

            if (Convert.ToDateTime(txtdispatchdate.Text) > Convert.ToDateTime(txtinstallationdate.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Dispatch date should be less than Installation date');", true);
                return;
            }


            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            try
            {
                var _data = _repo.GetMachineMasters_List().Where(x => x.MachineId == txtMachinId.Text && x.Id != id && x.IsDeleted == false).FirstOrDefault();
                if (_data != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Machin Id Exist! Please try again');", true);
                }
                else
                {
                    WeightMachineMaster _dt = db.WeightMachineMasters.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
                    _dt.MachineId = txtMachinId.Text.ToString();
                    _dt.PlantCodeId = ddlPlantId.SelectedValue.ToString();
                    _dt.Capacity = txtCapacity.Text.ToString()+ddlcapacityuom.Text.ToString();
                    _dt.Resolution = txtResolution.Text.ToString()+ddlresolutionuom.Text.ToString();
                    _dt.Model = txtModel.Text.ToString();
                    _dt.PlatformSize = txtPlatformsize.Text.ToString();
                    _dt.MachineNo = txtmachinno.Text.ToString();
                    _dt.Indicator = txtindicator.Text.ToString();
                    _dt.LCType = txtlctype.Text.ToString();
                    _dt.NoOfLoadCells = txtnoofloadcells.Text.ToString();
                    _dt.LoadCellSerialNos = txtlcsn.Text.ToString();
                    _dt.EquipmentId = Convert.ToInt32(txtequipment.Text);
                    _dt.InvoiceNo = txtinvoiceno.Text.ToString();
                    _dt.DespatchDate = Convert.ToDateTime(txtdispatchdate.Text);
                    _dt.InstallationDate = Convert.ToDateTime(txtinstallationdate.Text);
                    _dt.WarrentyUpto = Convert.ToDateTime(txtwarrantyupto.Text);
                    _dt.ReasonWarrentyUptoDate = txtrowud.Text.ToString();
                    _dt.WeighingUnit = this.ddlcapacityuom.Text.Trim().ToString();
                    db.SubmitChanges();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Updated Successfully');", true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "1;url=AddEdit.aspx?id="+id;
                    this.Page.Controls.Add(meta);
                }
               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
            }
        }

        //Get:Weight Machine Master for edit
        private void GetWTMachinMasterForEdit()
        {
            if (Request.QueryString["Id"] != null)
            {
                txtMachinId.Enabled = false;
                ddlPlantId.Enabled = false;
                divoptions.Style.Add("display", "block");
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                WeightMachineMaster _wmm = _repo.Get_WeightMachineMasterById(id);
                if (_wmm != null)
                {
                    try
                    {
                        ddlPlantId.SelectedValue = _wmm.PlantCodeId.ToString();
                        txtMachinId.Text = _wmm.MachineId.ToString();

                        txtCapacity.Text = _repo.Get_Number(_wmm.Capacity);
                        ddlcapacityuom.SelectedValue = _repo.Get_String(_wmm.Capacity);



                        txtResolution.Text = _repo.Get_Number(_wmm.Resolution);
                        ddlresolutionuom.SelectedValue = _repo.Get_String(_wmm.Resolution);

                        txtModel.Text = _wmm.Model.ToString();
                        txtPlatformsize.Text = _wmm.PlatformSize.ToString();
                        txtmachinno.Text = _wmm.MachineNo.ToString();
                        txtindicator.Text = _wmm.Indicator.ToString();
                        txtlctype.Text = _wmm.LCType.ToString();
                        txtnoofloadcells.Text = _wmm.NoOfLoadCells.ToString();
                        txtlcsn.Text = _wmm.LoadCellSerialNos.ToString();
                        txtequipment.Text = _wmm.EquipmentId.ToString();
                        txtinvoiceno.Text = _wmm.InvoiceNo.ToString();
                        txtdispatchdate.Text = _wmm.DespatchDate.ToString();
                        txtinstallationdate.Text = _wmm.InstallationDate.ToString();
                        txtwarrantyupto.Text = _wmm.WarrentyUpto.ToString();
                        txtrowud.Text = _wmm.ReasonWarrentyUptoDate.ToString();
                        this.ddlcapacityuom.Text = _wmm.WeighingUnit.ToString();
                    }
                    catch { }

                }
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Edit();
            }
            else
            {
                Add();
            }
        }

        //Get:First record
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = _repo.GetMachineMasters_List().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Previous record
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            WeightMachineMaster next = null;
            try
            {
                next = _repo.GetMachineMasters_List().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Next record
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = _repo.GetMachineMasters_List().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Last record
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = _repo.GetMachineMasters_List().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }              
    } 
}
