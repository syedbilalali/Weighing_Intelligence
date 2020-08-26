using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;

namespace Avery_Weigh.Alpha_DisplayMaster
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        AlphaDisplayRepository _repo = new AlphaDisplayRepository();
        WeightMachinMasterRepository _wmRepo = new WeightMachinMasterRepository();
        PlantmasterRepository plantrepo = new PlantmasterRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_PlantCodeId();
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    GetAlphaDisplayMasterForEdit();
                }
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

        //Get:Machine Id by PlantCode
        private void Get_MachineIdByPlantCode(string plantcode)
        {
            var data = _wmRepo.Get_MachineIdBy_PlantCode(plantcode);
            if (data.Count() > 0)
            {
                ddlmachineid.DataValueField = "MachineId";
                ddlmachineid.DataTextField = "MachineId";
                ddlmachineid.DataSource = data;
                ddlmachineid.DataBind();
            }
        }

        //Get:PlantCode Id
        private void Get_PlantCodeId()
        {
            var data = plantrepo.Get_PlantCodeId();
            if (data != null)
            {
                ddlplantcode.DataTextField = "PlantName";
                ddlplantcode.DataValueField = "PlantCode";
                ddlplantcode.DataSource = data;
                ddlplantcode.DataBind();
                ddlplantcode.Items.Insert(0, new ListItem("Select", ""));
            }
        }

        //Add:New AlphaDisplayMaster Record
        private void Add()
        {
            try
            {
                int count = db.AlphaDisplayMasters.Count(x => x.PlantCodeId == ddlplantcode.SelectedValue && x.MachineId == ddlmachineid.SelectedValue && x.IsDeleted == false);
                if (count < 2)
                {
                    var check = db.AlphaDisplayMasters.Where(x => x.PlantCodeId == ddlplantcode.SelectedValue && x.AlphaDisplayIP == txtip.Text && x.IsDeleted == false).FirstOrDefault();
                    if (check != null)
                    {
                        ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('Same Alpha Display IP Exist! Please Try Again.');",true);
                    }
                    else
                    {
                         AlphaDisplayMaster _ad = new AlphaDisplayMaster();
                        _ad.PlantCodeId = ddlplantcode.SelectedValue;
                        _ad.MachineId = ddlmachineid.SelectedValue;
                        _ad.AlphaDisplayIdentification = txtidentification.Text.ToString();
                        _ad.AlphaDisplayIP = txtip.Text.ToString();
                        _ad.AlphaDisplayPort = txtport.Text.ToString();
                        _ad.IsDeleted = false;
                        if (_repo.Add_AlphaDisplay(_ad))
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
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('Maximum 2 Alpha Display Allow In 1 Plant.');",true);
                }
              
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('"+ex.Message.ToString()+"');",true);
                
            }
        }

        //Update:AlphaDisplayMaster Record
        private void Edit()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                    var data = db.AlphaDisplayMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
                    if (data != null)
                    {
                        int count = _repo.GetAlphaDisplayMasters_List().Count(x => x.PlantCodeId == ddlplantcode.SelectedValue && x.MachineId == ddlmachineid.SelectedValue && x.Id != id && x.IsDeleted == false);
                        if (count >= 2)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Alpha Display Exist! please Try Again.');", true);
                        }
                        else
                        {
                            var check = _repo.GetAlphaDisplayMasters_List().FirstOrDefault(x => x.PlantCodeId == ddlplantcode.SelectedValue && x.AlphaDisplayIP == txtip.Text && x.IsDeleted == false && x.Id!=id);
                            if (check != null)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Alpha Display IP Exist!.');", true);
                            }
                            else
                            {
                                data.PlantCodeId = ddlplantcode.SelectedValue;
                                data.MachineId = ddlmachineid.SelectedValue;
                                data.AlphaDisplayIdentification = txtidentification.Text.ToString();
                                data.AlphaDisplayIP = txtip.Text.ToString();
                                data.AlphaDisplayPort = txtport.Text.ToString();
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
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('error');", true);
                    }
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('"+ex.Message.ToString()+"');",true);
            }
        }

        //Get:AlphaDisplayMaster For Edit
        private void GetAlphaDisplayMasterForEdit()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                divoptions.Style.Add("display", "block");
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                var data = _repo.Get_AlphaDisplayById(id);
                if (data != null)
                {
                    ddlplantcode.SelectedValue = data.PlantCodeId;
                    Get_MachineIdByPlantCode(ddlplantcode.SelectedValue);
                    ddlmachineid.SelectedValue = data.MachineId;
                    txtidentification.Text = data.AlphaDisplayIdentification;
                    txtip.Text = data.AlphaDisplayIP;
                    txtport.Text = data.AlphaDisplayPort;
                }
            }
        }

        protected void ddlplantcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = _wmRepo.Get_MachineIdBy_PlantCode(ddlplantcode.SelectedValue);
            if(data.Count() > 0)
            {
                ddlmachineid.DataTextField = "MachineId";
                ddlmachineid.DataValueField = "MachineId";
                ddlmachineid.DataSource = data;
                ddlmachineid.DataBind();
                ddlmachineid.Items.Insert(0, new ListItem("Select Machine ID", ""));
            }
            else
            {
                ddlmachineid.Items.Clear();
                ddlmachineid.Items.Insert(0, new ListItem("Not Available", ""));
            }
        }

        //Get:First Record from AlphaDisplayMaster
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = _repo.GetAlphaDisplayMasters_List().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:AlphaDisplayMaster Previous Record
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            AlphaDisplayMaster next = null;
            try
            {
                next = _repo.GetAlphaDisplayMasters_List().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:AlphaDisplayMaster Next Record
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = _repo.GetAlphaDisplayMasters_List().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:AlphaDisplayMaster Last Record
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = _repo.GetAlphaDisplayMasters_List().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }
    }
}