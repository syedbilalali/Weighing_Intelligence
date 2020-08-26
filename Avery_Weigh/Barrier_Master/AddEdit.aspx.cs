using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;

namespace Avery_Weigh.Barrier_Master
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        BarrierMasterRepository _repo = new BarrierMasterRepository();
        PlantmasterRepository _plantrepo = new PlantmasterRepository();
        WeightMachinMasterRepository _wmrepo = new WeightMachinMasterRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_PlantCode();
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    Get_Barrier_ForEdit();
                }
            }
        }

        //Add:New BarrierMaster Record
        protected void Add_Barrier()
        {
            try
            {
                int count = _repo.GetBarrierMasters_List().Count(x => x.PlantCodeId == ddlplantid.SelectedValue && x.MachineId == ddlmachinid.SelectedValue && x.IsDeleted == false);
                if (count < 2)
                {
                    var check = _repo.GetBarrierMasters_List().Where(x => x.PlantCodeId == ddlplantid.SelectedValue && x.BarrierIP == txtip.Text && x.IsDeleted == false).FirstOrDefault();
                    if (check != null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Barrier IP Exist! Please Try Again.');", true);
                    }
                    else
                    {
                        BarrierMaster _bm = new BarrierMaster();
                        _bm.PlantCodeId = ddlplantid.SelectedValue.ToString();
                        _bm.MachineId = ddlmachinid.SelectedValue.ToString();
                        _bm.BarrierIdentification = txtidentification.Text.ToString();
                        _bm.BarrierIP = txtip.Text.ToString();
                        _bm.BarrierPort = txtport.Text.ToString();
                        _bm.BarrierScheme = ddlscheme.SelectedValue.ToString();
                        _bm.IsDeleted = false;
                        if (_repo.Add_BarrierMaster(_bm))
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Maximum 2 Barrier Allow In 1 Plant.');", true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);

            }
        }

        //Update:BarrierMaster Record
        protected void Edit_Barrier()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                    var data = db.BarrierMasters.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
                    if (data != null)
                    {
                        int count = _repo.GetBarrierMasters_List().Count(x => x.PlantCodeId == ddlplantid.SelectedValue && x.MachineId == ddlmachinid.SelectedValue && x.Id != id && x.IsDeleted == false);
                        if (count >= 2)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Barrier Exist! please Try Again.');", true);
                        }
                        else
                        {
                            var check = _repo.GetBarrierMasters_List().FirstOrDefault(x => x.PlantCodeId == ddlplantid.SelectedValue && x.BarrierIP == txtip.Text && x.IsDeleted == false && x.Id != id);
                            if (check != null)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Barrier IP Exist!.');", true);
                            }
                            else
                            {
                                data.PlantCodeId = ddlplantid.SelectedValue;
                                data.MachineId = ddlmachinid.SelectedValue;
                                data.BarrierIdentification = txtidentification.Text.ToString();
                                data.BarrierIP = txtip.Text.ToString();
                                data.BarrierPort = txtport.Text.ToString();
                                data.BarrierScheme = ddlscheme.SelectedValue;
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
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Barrier data not found!');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
            }
        }

        //Get:BarrierMaster for Edit By Barrier Master Id
        protected void Get_Barrier_ForEdit()
        {
            divoptions.Style.Add("display", "block");
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                var data = _repo.Get_BarrierMasterBy_Id(id);
                if (data != null)
                {
                    ddlplantid.SelectedValue = data.PlantCodeId.ToString();
                    Get_MachineId(ddlplantid.SelectedValue);
                    ddlmachinid.SelectedValue = data.MachineId.ToString();
                    txtidentification.Text = data.BarrierIdentification;
                    txtip.Text = data.BarrierIP;
                    txtport.Text = data.BarrierPort;
                    ddlscheme.SelectedValue = data.BarrierScheme;
                }
            }
        }

        protected void Btnsave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Edit_Barrier();
            }
            else
            {
                Add_Barrier();
            }
        }

        //Get:MachineId on PlantCode dropdown selection changed
        protected void ddlplantid_SelectedIndexChanged(object sender, EventArgs e)
        {
            IEnumerable<Model.Model_WeightMachinMaster> data =  _wmrepo.Get_MachineIdBy_PlantCode(ddlplantid.SelectedValue);
            if (data.Count() > 0)
            {
                ddlmachinid.DataTextField = "MachineId";
                ddlmachinid.DataValueField = "MachineId";
                ddlmachinid.DataSource = data;
                ddlmachinid.DataBind();
                ddlmachinid.Items.Insert(0, new ListItem("Select", ""));
            }
            else
            {
                ddlmachinid.Items.Clear();
                ddlmachinid.Items.Insert(0, new ListItem("Not Available", ""));
            }
        }

        //Get:First record of barrier master
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = _repo.GetBarrierMasters_List().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Barrier master previous record
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            BarrierMaster next = null;
            try
            {
                next = _repo.GetBarrierMasters_List().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Barrier master next record
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = _repo.GetBarrierMasters_List().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Barrier master Last Record
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = _repo.GetBarrierMasters_List().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Machine Id
        protected void Get_MachineId(string code)
        {
            IEnumerable<Model.Model_WeightMachinMaster> data =   _wmrepo.Get_MachineIdBy_PlantCode(code);
            if (data.Count() > 0)
            {
                ddlmachinid.DataTextField = "MachineId";
                ddlmachinid.DataValueField = "MachineId";
                ddlmachinid.DataSource = data;
                ddlmachinid.DataBind();
            }
        }

        //Get:PlantCode
        protected void Get_PlantCode()
        {
            var data = _plantrepo.Get_PlantCodeId();
            if (data != null)
            {
                ddlplantid.DataTextField = "PlantName";
                ddlplantid.DataValueField = "PlantCode";
                ddlplantid.DataSource = data;
                ddlplantid.DataBind();
                ddlplantid.Items.Insert(0, new ListItem("Select", ""));
            }
        }
    }
}
