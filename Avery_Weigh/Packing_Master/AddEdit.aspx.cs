using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Avery_Weigh.Repository;

namespace Avery_Weigh.Packing_Master
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        PackingRepository repo = new PackingRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                GetPackingForUpdate();
            }
        
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            if(Request.QueryString["Id"] == null)
            {
                AddPacking();
            }
            else
            {
                UpdatePacking();
            }
                   
        }
        
        //: Save Packing Master
        private void AddPacking()
        {
            if(Request.QueryString["Id"] == null)
            {
                try
                {
                    PackingMaster pp = repo.Get_PackingByCode(txtPackingCode.Text.Trim());
                    if (pp != null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same packing code Exist! Please try again');", true);
                    }
                    else
                    {
                        PackingMaster packingMaster = new PackingMaster();
                        packingMaster.PackingCode = txtPackingCode.Text.ToString();
                        packingMaster.PackingName = txtPackingName.Text.ToString();
                        packingMaster.PackingUOM = ddlpackinguom.SelectedValue.ToString();
                        packingMaster.PackingWT = txtPackingwt.Text.ToString();
                        packingMaster.IsDeleted = false;
                        if (repo.Add_PackingMaster(packingMaster))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Save Successfully')", true);
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
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('"+ex.Message.ToString()+"');",true);
                }
            }
        }
        
        //Update: Packing Master
        private void UpdatePacking()
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            try
            {
                PackingMaster _model = repo.GetPackingMasters_List().Where(x => x.PackingCode == txtPackingCode.Text.Trim() && x.Id != id && x.IsDeleted == false).SingleOrDefault();
                if (_model != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same packing code exist please try again');", true);
                }
                else
                {
                    PackingMaster packing = db.PackingMasters.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
                    if (packing != null)
                    {
                        packing.PackingCode = txtPackingCode.Text.ToString();
                        packing.PackingName = txtPackingName.Text.ToString();
                        packing.PackingUOM = ddlpackinguom.SelectedValue.ToString();
                        packing.PackingWT = txtPackingwt.Text.ToString();
                        db.SubmitChanges();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Updated Successfully')", true);
                        HtmlMeta meta = new HtmlMeta
                        {
                            HttpEquiv = "Refresh",
                            Content = "1;url=AddEdit.aspx?id="+id
                        };
                        this.Page.Controls.Add(meta);
                    }
                }
            }
            catch(Exception ex)
            {
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('"+ex.Message.ToString()+"');", true);
            }
          
          
        }
        
        //: Get Packing Master By Id For Update
        private void GetPackingForUpdate()
        {
            if (Request.QueryString["Id"] != null)
            {
                divoptions.Style.Add("display", "block");
                txtPackingCode.Enabled = false;
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                PackingMaster _model = repo.Get_PackingMasterById(id);
                if (_model != null)
                {
                    txtPackingCode.Text = _model.PackingCode.ToString();
                    txtPackingName.Text = _model.PackingName.ToString();
                    ddlpackinguom.SelectedValue = _model.PackingUOM.ToString();
                    txtPackingwt.Text = _model.PackingWT.ToString();
                }
            }
        }

        //: Return first record from the packing master
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = repo.GetPackingMasters_List().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }
        
        //: Retrun the Previous record from packing master
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            PackingMaster next = null;
            try
            {
                next = repo.GetPackingMasters_List().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }
       
        //: return next record from packing master
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = repo.GetPackingMasters_List().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }
        
        //: return last record from packing master
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = repo.GetPackingMasters_List().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }
    }
}