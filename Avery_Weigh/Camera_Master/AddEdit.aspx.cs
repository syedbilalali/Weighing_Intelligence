using Avery_Weigh.Repository;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Avery_Weigh.Model;

namespace Avery_Weigh.Camera_Master
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        CameraMasterRepository _repo = new CameraMasterRepository();
        PlantmasterRepository _plantrepo = new PlantmasterRepository();
        WeightMachinMasterRepository _wmrepo = new WeightMachinMasterRepository();


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Get_PlantCode();
                GetCameraMasterForEdit();
            }
        }

        protected void Btnsave_Click(object sender, EventArgs e)
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

        //Get:PlantCode from PlantMaster
        protected void Get_PlantCode()
        {
            IEnumerable<Model_PlantMaster> data = _plantrepo.Get_PlantCodeId();
            if (data != null)
            {
                ddlplantcode.DataTextField = "PlantName";
                ddlplantcode.DataValueField = "PlantCode";
                ddlplantcode.DataSource = data;
                ddlplantcode.DataBind();
                ddlplantcode.Items.Insert(0, new ListItem("Select", ""));
            }
        }

        //Add:New CameraMaster
        protected void Add()
        {
            try
            {
                int count = _repo.GetCameraMasters_List().Count(x => x.PlantCodeID == ddlplantcode.SelectedValue && x.MachineId == ddlmachineid.SelectedValue && x.IsDeleted == false);
                if (count < 3)
                {
                    var check = _repo.GetCameraMasters_List().Where(x => x.PlantCodeID == ddlplantcode.SelectedValue && x.CameraIP == txtip.Text && x.IsDeleted == false).FirstOrDefault();
                    if (check != null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Camera IP Exist! Please Try Again.');", true);
                    }
                    else
                    {
                        CameraMaster _cm = new CameraMaster();
                        _cm.PlantCodeID = ddlplantcode.SelectedValue.ToString();
                        _cm.MachineId = ddlmachineid.SelectedValue;
                        _cm.CameraIndentification = txtidentification.Text.ToString();
                        _cm.CameraIP = txtip.Text.ToString();
                        _cm.CameraPort = txtport.Text.ToString();
                        _cm.CameraUserName = txtCameraUserName.Text.Trim();
                        _cm.CameraPwd = txtCameraPwd.Text.Trim();
                        _cm.IsDeleted = false;
                        if (_repo.Add_CameraMaster(_cm))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Save Successfully')", true);
                            HtmlMeta meta = new HtmlMeta();
                            meta.HttpEquiv = "Refresh";
                            meta.Content = "1;url=AddEdit.aspx";
                            this.Page.Controls.Add(meta);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Maximum 3 Camera Allow In 1 Plant.');", true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);

            }
        }

        //Update:CameraMaster Record
        protected void Edit()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                    var data = db.CameraMasters.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
                    if (data != null)
                    {
                        int count = _repo.GetCameraMasters_List().Count(x => x.PlantCodeID == ddlplantcode.SelectedValue && x.MachineId == ddlmachineid.SelectedValue && x.Id != id && x.IsDeleted == false);
                        if (count >= 3)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Camera Configuration Exist! please Try Again.');", true);
                        }
                        else
                        {
                            var check = _repo.GetCameraMasters_List().FirstOrDefault(x => x.PlantCodeID == ddlplantcode.SelectedValue && x.CameraIP == txtip.Text && x.IsDeleted == false && x.Id != id);
                            if (check != null)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Camera IP Exist!.');", true);
                            }
                            else
                            {
                                data.PlantCodeID = ddlplantcode.SelectedValue;
                                data.MachineId = ddlmachineid.SelectedValue;
                                data.CameraIndentification = txtidentification.Text.ToString();
                                data.CameraIP = txtip.Text.ToString();
                                data.CameraPort = txtport.Text.ToString();
                                data.CameraUserName = txtCameraUserName.Text.Trim();
                                data.CameraPwd = txtCameraPwd.Text.Trim();
                                db.SubmitChanges();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Updated Successfully');", true);
                                HtmlMeta meta = new HtmlMeta();
                                meta.HttpEquiv = "Refresh";
                                meta.Content = "1;url=AddEdit.aspx?id=" + id;
                                this.Page.Controls.Add(meta);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Camera data not found!');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
            }
        }

        //Get:CameraMaster for Edit
        protected void GetCameraMasterForEdit()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                divoptions.Style.Add("display", "block");
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                var data = _repo.Get_CameraMaster_ById(id);
                if (data != null)
                {
                    ddlplantcode.SelectedValue = data.PlantCodeID.ToString();
                    Get_MachineId_By_PlantCode(data.PlantCodeID);
                    ddlmachineid.SelectedValue = data.MachineId.ToString();
                    txtidentification.Text = data.CameraIndentification;
                    txtip.Text = data.CameraIP;
                    txtport.Text = data.CameraPort;
                    txtCameraUserName.Text = data.CameraUserName;
                    txtCameraPwd.Text = data.CameraPwd;
                }
            }
        }

        //Get:Machine By Plant Code from WeightMachineMaster
        protected void Get_MachineId_By_PlantCode(string plantcodeId)
        {
            IEnumerable<Model_WeightMachinMaster> data = _wmrepo.Get_MachineIdBy_PlantCode(plantcodeId);
            if (data.Count() > 0)
            {
                ddlmachineid.DataTextField = "MachineId";
                ddlmachineid.DataValueField = "MachineId";
                ddlmachineid.DataSource = data;
                ddlmachineid.DataBind();
                ddlmachineid.Items.Insert(0, new ListItem("Select", ""));
            }
        }

        //Get:MachineId By PlantCode on plantcode dropdown selection change
        protected void ddlplantcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlplantcode.SelectedValue))
            {
                IEnumerable<Model_WeightMachinMaster> data = _wmrepo.Get_MachineIdBy_PlantCode(ddlplantcode.SelectedValue);
                if (data.Count() > 0)
                {
                    ddlmachineid.DataTextField = "MachineId";
                    ddlmachineid.DataValueField = "MachineId";
                    ddlmachineid.DataSource = data;
                    ddlmachineid.DataBind();
                    ddlmachineid.Items.Insert(0, new ListItem("Select", ""));
                }
                else
                {
                    ddlmachineid.Items.Clear();
                    ddlmachineid.Items.Insert(0, new ListItem("Not Available", ""));
                }
            }
        }

        //Get:First record from cameramaster
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = _repo.GetCameraMasters_List().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Previous record from cameramaster
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            CameraMaster next = null;
            try
            {
                next = _repo.GetCameraMasters_List().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Next record from cameramaster
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = _repo.GetCameraMasters_List().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Last record from cameramaster
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = _repo.GetCameraMasters_List().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }
    }
}