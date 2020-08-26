using Avery_Weigh.Repository;
using System;
using System.Collections;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Model;
using System.Collections.Generic;

namespace Avery_Weigh.Machine
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        PlantmasterRepository _repo = new PlantmasterRepository();
        WeightMachinMasterRepository _machineRepo = new WeightMachinMasterRepository();
        MachineParametersRepository _workrepo = new MachineParametersRepository();
        TaretrTareToleranceRepository _toltype = new TaretrTareToleranceRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_PlantCodeId();
                Get_ToleranceType();
                lblUnit.Text = Session["WEIGHINGUNIT"].ToString();
                lblUnit1.Text = Session["WEIGHINGUNIT"].ToString();
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    GetMachineForEdit();
                }
            }
        }

        //Get:Machine Working Parameters for Edit
        protected void GetMachineForEdit()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                divoptions.Style.Add("display", "block");
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                string code = _workrepo.Get_PlantCode_By_Id(id);
                var data = db.tblMachineWorkingParameters.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
                if (data != null)
                {
                    ddlPlantCode.SelectedValue = data.PlantCode.ToString();
                    Get_MachineId(code);//Bind machinedd
                    ddlMachineName.SelectedValue = data.MachineId.ToString();
                    txtIPPort.Text = data.IPPort;
                    txtModeofComs.Text = data.ModeOfComs;
                    txtPortNo.Text = data.PortNo;
                    txtStabilityNos.Text = data.StabilityNos;
                    txtStabilityRange.Text = data.StabilityRange;
                    ddlstoredtare.SelectedValue = data.StoredTare.ToString();
                    ddltarecheck.SelectedValue = data.TareCheck.ToString();
                    txtTransactionPrefix.Text = data.TransactionNoPrefix;
                    ddlzerointerlock.SelectedValue = data.ZeroInterlock.ToString();
                    txtZeroInterlockRange.Text = data.ZeroInterlockRange.ToString();
                    txtNetWtLimit.Text = data.NetWeightLimit.ToString();
                    ddlWeightInterlock.Text = data.WeightInterlock.ToString();
                    txtTicketPaperSize.Text= data.TicketPaperSize;
                    try
                    {
                        txtWeightInterlockRange.Text = data.WeightInterlockRange.ToString();
                    }
                    catch { }
                    txtLastStoredWeight.Text = data.LastStoredWeight.ToString();
                    try
                    {
                        ddlTareToleranceType.Text = data.TareScheme.ToString();
                    }
                    catch { }
                    try
                    {
                        txtWtValue.Text = data.TareWeightValue.ToString();
                    }
                    catch { }
            }
            }
        }

        //Get Machine Id By PlantCode
        protected void Get_MachineId(string plantcode)
        {
            IEnumerable<Model_WeightMachinMaster> data = _machineRepo.Get_MachineIdBy_PlantCode(plantcode);
            if (data.Count() > 0)
            {
                ddlMachineName.DataTextField = "MachineId";
                ddlMachineName.DataValueField = "MachineId";
                ddlMachineName.DataSource = data;
                ddlMachineName.DataBind();
            }
        }

        //Get Tolerance Type 
        protected void Get_ToleranceType()
        {
            IEnumerable<Model_TareToletrance> data = _toltype.Get_TareToleranceType_Add();
            if (data != null)
            {
                ddlTareToleranceType.DataTextField = "Description";
                ddlTareToleranceType.DataValueField = "Description";
                ddlTareToleranceType.DataSource = data;
                ddlTareToleranceType.DataBind();
            }
        }

        //Get:PlantCode
        protected void Get_PlantCodeId()
        {
            var data = _repo.Get_PlantCodeId();
            if (data != null)
            {
                ddlPlantCode.DataTextField = "PlantName";
                ddlPlantCode.DataValueField = "PlantCode";
                ddlPlantCode.DataSource = data;
                ddlPlantCode.DataBind();
                ddlPlantCode.Items.Insert(0, new ListItem("Select", ""));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] == null)
            {
                AddMachine();
            }
            else
            {
                UpdateMachine();
            }
        }

        //Add:New Machine Working Parameter Record
        protected void AddMachine()
        {
            try
            {
                tblMachineWorkingParameter data = _workrepo.GetMachineWorkingParameters_List().FirstOrDefault(x => x.PlantCode == ddlPlantCode.SelectedValue && x.MachineId == ddlMachineName.SelectedValue && x.IsDeleted == false);
                tblMachineWorkingParameter data2 = _workrepo.GetMachineWorkingParameters_List().FirstOrDefault(x => x.PlantCode == ddlPlantCode.SelectedValue && x.IPPort == txtIPPort.Text.Trim() && x.IsDeleted == false);
                if (data != null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "toastr", "toastr.error('Same Machine Configuration Exist! Please try again');", true);
                }
                else if (data2 != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same IP Port Already Exist! Please Try Again');", true);
                }
                else
                {
                    tblMachineWorkingParameter machine = new tblMachineWorkingParameter();
                    machine.IPPort = txtIPPort.Text;
                    machine.IsDeleted = false;
                    machine.MachineId = ddlMachineName.SelectedValue;
                    machine.ModeOfComs = txtModeofComs.SelectedValue;  //txtModeofComs.Text;
                    machine.PlantCode = ddlPlantCode.SelectedValue;
                    machine.PortNo = txtPortNo.Text;
                    machine.StabilityNos = txtStabilityNos.Text;
                    machine.StabilityRange = txtStabilityRange.Text;
                    machine.StoredTare = Convert.ToInt32(ddlstoredtare.SelectedValue);
                    machine.TareCheck = Convert.ToInt32(ddltarecheck.SelectedValue);
                    machine.TransactionNoPrefix = txtTransactionPrefix.Text;
                    machine.ZeroInterlock = Convert.ToInt32(ddlzerointerlock.SelectedValue);
                    if (txtZeroInterlockRange.Text.Trim() == "")
                        txtZeroInterlockRange.Text = "0";
                    machine.ZeroInterlockRange = txtZeroInterlockRange.Text;
                    machine.TareScheme = ddlTareToleranceType.Text;
                    if (!string.IsNullOrEmpty(txtWtValue.Text))
                    machine.TareWeightValue = Convert.ToInt32(txtWtValue.Text);
                    if (txtNetWtLimit.Text.Trim() == "")
                        txtNetWtLimit.Text = "0";
                    machine.NetWeightLimit= Convert.ToInt32(txtNetWtLimit.Text);

                    machine.WeightInterlock = Convert.ToInt32(ddlWeightInterlock.SelectedValue);
                    if (txtWeightInterlockRange.Text.Trim() == "")
                        txtWeightInterlockRange.Text = "0";
                    machine.WeightInterlockRange = txtWeightInterlockRange.Text.Trim();
                    if (txtLastStoredWeight.Text.Trim() == "")
                        txtLastStoredWeight.Text = "0";
                    machine.LastStoredWeight = Convert.ToDecimal(txtLastStoredWeight.Text);

                    machine.TicketPaperSize = this.txtTicketPaperSize.SelectedValue;

                    if (_workrepo.Add_WorkingParameter(machine))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Saved Successfully.')", true);
                        HtmlMeta meta = new HtmlMeta
                        {
                            HttpEquiv = "Refresh",
                            Content = "1;url=AddEdit.aspx"
                        };
                        this.Page.Controls.Add(meta);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
            }

        }

        //Update:MachineWorkingParameter Record
        protected void UpdateMachine()
        {
            if (Request.QueryString["Id"] != null)
            {
                try
                {
                    int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                    tblMachineWorkingParameter data = _workrepo.GetMachineWorkingParameters_List().FirstOrDefault(x => x.PlantCode == ddlPlantCode.SelectedValue && x.MachineId == ddlMachineName.SelectedValue && x.IsDeleted == false && x.Id != id);
                    tblMachineWorkingParameter data2 = _workrepo.GetMachineWorkingParameters_List().FirstOrDefault(x => x.IPPort == txtIPPort.Text && x.PlantCode == ddlPlantCode.SelectedValue && x.Id != id && x.IsDeleted == false);
                    if (data != null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Machine Configuration Exist! Please try again');", true);
                    }
                    else if (data2 != null)
                    {
                        ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('Same IP Port Already Exist! Please Try Again');",true);
                    }
                    else { 
                        tblMachineWorkingParameter machine = db.tblMachineWorkingParameters.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
                        if (machine != null)
                        {
                            machine.IPPort = txtIPPort.Text;
                            machine.IsDeleted = false;
                            machine.MachineId = ddlMachineName.SelectedValue;
                            machine.ModeOfComs = txtModeofComs.SelectedValue;  // txtModeofComs.Text;
                            machine.PlantCode = ddlPlantCode.SelectedValue;
                            machine.PortNo = txtPortNo.Text;
                            machine.StabilityNos = txtStabilityNos.Text;
                            machine.StabilityRange = txtStabilityRange.Text;
                            machine.StoredTare = Convert.ToInt32(ddlstoredtare.SelectedValue);
                            machine.TareCheck = Convert.ToInt32(ddltarecheck.SelectedValue);
                            machine.TransactionNoPrefix = txtTransactionPrefix.Text;
                            machine.ZeroInterlock = Convert.ToInt32(ddlzerointerlock.SelectedValue);
                            if (txtZeroInterlockRange.Text.Trim() == "")
                                txtZeroInterlockRange.Text = "0";
                            machine.ZeroInterlockRange = txtZeroInterlockRange.Text;
                            machine.TareScheme = ddlTareToleranceType.Text;
                            if (!string.IsNullOrEmpty(txtWtValue.Text))
                                machine.TareWeightValue = Convert.ToInt32(txtWtValue.Text);

                            if (txtNetWtLimit.Text.Trim() == "")
                                txtNetWtLimit.Text = "0";
                            machine.NetWeightLimit = Convert.ToInt32(txtNetWtLimit.Text);

                            machine.WeightInterlock = Convert.ToInt32(ddlWeightInterlock.SelectedValue);
                            if (txtWeightInterlockRange.Text.Trim() == "")
                                txtWeightInterlockRange.Text = "0";
                            machine.WeightInterlockRange = txtWeightInterlockRange.Text.Trim();
                            if (txtLastStoredWeight.Text.Trim() == "")
                                txtLastStoredWeight.Text = "0";
                            machine.LastStoredWeight = Convert.ToDecimal(txtLastStoredWeight.Text);
                            machine.TicketPaperSize = this.txtTicketPaperSize.SelectedValue;

                            db.SubmitChanges();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Updated Successfully')", true);
                            HtmlMeta meta = new HtmlMeta
                            {
                                HttpEquiv = "Refresh",
                                Content = "1;url=AddEdit.aspx?id=" + id
                            };
                            this.Page.Controls.Add(meta);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
                }
            }
        }

        //Get:Machine Id on plantcodeId dropdown selection change
        protected void ddlPlantCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _plantcode = ddlPlantCode.SelectedValue;
            IEnumerable<Model_WeightMachinMaster> data = _machineRepo.Get_MachineIdBy_PlantCode(_plantcode);
            if (data.Count() > 0)
            {
                ddlMachineName.DataTextField = "MachineId";
                ddlMachineName.DataValueField = "MachineId";
                ddlMachineName.DataSource = data;
                ddlMachineName.DataBind();
                ddlMachineName.Items.Insert(0, new ListItem("Select MachineID", ""));
            }
            else
            {
                ddlMachineName.Items.Clear();
                ddlMachineName.Items.Insert(0, new ListItem("Not Available", ""));
            }
        }

        //Get:First Record from machine working parameter
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = _workrepo.GetMachineWorkingParameters_List().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Previous Record from machine working parameter
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            tblMachineWorkingParameter next = null;
            try
            {
                next = _workrepo.GetMachineWorkingParameters_List().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Next Record from machine working parameter
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = _workrepo.GetMachineWorkingParameters_List().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Last Record from machine working parameter
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = _workrepo.GetMachineWorkingParameters_List().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        protected void ddlTareToleranceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTareToleranceType.Text.Trim()))
            {
                AverageTareSchema _avgtarewt = db.AverageTareSchemas.Where(x => x.Description == ddlTareToleranceType.Text.Trim()).FirstOrDefault();
                this.txtWtValue.Text = _avgtarewt.weightvalue.ToString();
            }
        }

        //protected void ddlTareToleranceType_TextChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(ddlTareToleranceType.Text.Trim()))
        //    {
        //        AverageTareSchema _avgtarewt = db.AverageTareSchemas.Where(x => x.Description == ddlTareToleranceType.Text.Trim()).FirstOrDefault();
        //        this.txtWtValue.Text = _avgtarewt.weightvalue.ToString();
        //    }
        //}
    }
}