using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Avery_Weigh.Repository;

namespace Avery_Weigh.Material_Classification
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        MaterialClassificationRepository _matrepo = new MaterialClassificationRepository();
        SupplierRepository supRepo = new SupplierRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_VendorSupplierCode();
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    GetMaterialClassificationById();
                }
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] == null)
            {
                SaveMaterialClassification();               
            }
            else
            {               
                UpdateMaterialClassification();
            }

        }

        //Update:Material Classification record
        private void UpdateMaterialClassification()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                try
                {
                    int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                    MaterialClassification _mc = db.MaterialClassifications.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
                    if (_mc != null)
                    {
                        MaterialClassification material = db.MaterialClassifications.Where(x => x.MaterialClassificationCode == txtmcc.Text && x.Id != id).SingleOrDefault();
                        if (material != null)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "toastr", "toastr.error('Same Material Classification Code Exist! Please Try Again');", true);
                        }
                        else
                        {
                            _mc.MaterialClassificationCode = txtmcc.Text.ToString();
                            _mc.MaterialClassificationDesc = txtmcd.Text.ToString();
                            _mc.Supplier_VendorCode = ddlsupplier.SelectedValue;
                            db.SubmitChanges();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Updated Successfully')", true);
                            HtmlMeta meta = new HtmlMeta();
                            meta.HttpEquiv = "Refresh";
                            meta.Content = "1;url=AddEdit.aspx?id="+id;
                            this.Page.Controls.Add(meta);
                        }
                    }
                }
                catch { }          
            }         
        }

        //Add:New Material Classification
        private void SaveMaterialClassification()
        {
            if(string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                try
                {
                    MaterialClassification _material = _matrepo.Get_Material_ClassificationByCode(txtmcc.Text.Trim());
                    if (_material != null)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "toastr", "toastr.error('Same Material Classification Code Exist! Please try again');", true);
                    }
                    else
                    {
                         MaterialClassification _mc = new MaterialClassification();
                        _mc.MaterialClassificationCode = txtmcc.Text.ToString();
                        _mc.MaterialClassificationDesc = txtmcd.Text.ToString();
                        _mc.Supplier_VendorCode = ddlsupplier.SelectedValue;
                        _mc.IsDeleted = false;
                        if (_matrepo.Add_MaterialClassification(_mc))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Save Successfully')", true);
                            HtmlMeta meta = new HtmlMeta();
                            meta.HttpEquiv = "Refresh";
                            meta.Content = "1;url=AddEdit.aspx";
                            this.Page.Controls.Add(meta);
                        }
                    }
                }
                catch { }
            }
        } 

        //Get: material classification for update
        private void GetMaterialClassificationById()
        {
            divoptions.Style.Add("display", "block");
            txtmcc.Enabled = false;
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            MaterialClassification _mc = _matrepo.Get_MaterialClassificationById(id);
            if (_mc != null)
            {
                txtmcc.Text = _mc.MaterialClassificationCode.ToString();
                txtmcd.Text = _mc.MaterialClassificationDesc.ToString();
                Get_VendorSupplierCode();
                ddlsupplier.SelectedValue = _mc.Supplier_VendorCode.ToString();              
            }
            else
            {
                return;
            }           
        }

        protected void linkAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }  

        //Get:Supplier Vendor Code
        private void Get_VendorSupplierCode()
        {
            ddlsupplier.DataTextField = "Name";
            ddlsupplier.DataValueField = "Code";
            ddlsupplier.DataSource = supRepo.Get_SupplierCode();
            ddlsupplier.DataBind();
            ddlsupplier.Items.Insert(0, new ListItem("Select"));
        }
       
        //Get:First record from material classification
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = _matrepo.Get_MaterialClassification_List().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Previous record from material classification
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            MaterialClassification next = null;
            try
            {
                next = _matrepo.Get_MaterialClassification_List().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Next record from material calssification
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = _matrepo.Get_MaterialClassification_List().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Last record from material calssification
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = _matrepo.Get_MaterialClassification_List().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }
    }
}