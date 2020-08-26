using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Avery_Weigh.Packing_Master;
using Avery_Weigh.Material_Classification;
using Avery_Weigh.Repository;
using System.Web.UI.HtmlControls;
using Avery_Weigh.Model;

namespace Avery_Weigh.Material
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        MaterialClassificationRepository _classificationrepo = new MaterialClassificationRepository();
        PackingRepository _packrepo = new PackingRepository();
        MaterialRepository matrepo = new MaterialRepository();
        SystemLogRepository logRepo = new SystemLogRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_MaterialClassificationCode();
                Get_PackingCode();
                if (Request.QueryString["Id"] != null)
                {
                    GetMaterialForUpdate();
                }
            }
        }

       

        protected void btnsave_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] == null)
            {
                AddMaterial();
            }
            else
            {
                UpdateMaterial();
            }
        }

        //Add:New Material
        private void AddMaterial()
        {
            try
            {
                tblMaterial _material =  matrepo.Get_tblMaterialByCode(txtmaterialcode.Text.Trim());
                if (_material != null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "toastr", "toastr.error('Same Material Code Exist! Please try again');", true);
                    adderrorlog("Same Material Code Exist! Please try again", "Material");
                }
                else
                {
                    tblMaterial material = new tblMaterial();
                    material.MaterialCode = txtmaterialcode.Text.ToString();
                    material.MaterialDesc = txtmaterialdesc.Text.ToString();
                    material.PackingCodeId = ddlpackingcodeid.SelectedValue;
                    material.MaterialClassificationCodeId = ddlmaterialclassificationid.SelectedValue;
                    material.IsDeleted = false;
                    if (matrepo.Add_Material(material))
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
                adderrorlog(ex.Message.ToString(), "Material");
            }
        }

        //Update:Material
        private void UpdateMaterial()
        {
            if (Request.QueryString["Id"] != null)
            {
                try
                {
                    int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                    tblMaterial _mat = matrepo.Get_MaterialList().Where(x => x.MaterialCode == txtmaterialcode.Text && x.Id != id).FirstOrDefault();
                    if (_mat != null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Material Code Exist! Please try again');", true);
                        adderrorlog("Same Material Code Exist! Please try again", "Material");
                    }
                    else
                    {
                         tblMaterial _material = db.tblMaterials.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
                        _material.MaterialCode = txtmaterialcode.Text.ToString();
                        _material.MaterialDesc = txtmaterialdesc.Text.ToString();
                        _material.PackingCodeId = ddlpackingcodeid.SelectedValue.ToString();
                        _material.MaterialClassificationCodeId = ddlmaterialclassificationid.SelectedValue.ToString();
                        db.SubmitChanges();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Updated Successfully')", true);
                        HtmlMeta meta = new HtmlMeta();
                        meta.HttpEquiv = "Refresh";
                        meta.Content = "1;url=AddEdit.aspx?id="+id;
                        this.Page.Controls.Add(meta);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
                    adderrorlog(ex.Message.ToString(), "Material");
                }


            }

        }

        //Get:Material for update
        private void GetMaterialForUpdate()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                divoptions.Style.Add("display", "block");
                txtmaterialcode.Enabled = false;
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                try
                {
                    tblMaterial material = matrepo.Get_MaterialById(id);
                    if (material != null)
                    {
                        txtmaterialcode.Text = material.MaterialCode.ToString();
                        txtmaterialdesc.Text = material.MaterialDesc.ToString();
                        ddlpackingcodeid.SelectedValue = material.PackingCodeId.ToString();
                        ddlmaterialclassificationid.SelectedValue = material.MaterialClassificationCodeId.ToString();
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
                    adderrorlog(ex.Message.ToString(), "Material");
                }
            }         
        }

        //Get:Material Classification code from materialclassification
        protected void Get_MaterialClassificationCode()
        {
            ddlmaterialclassificationid.DataTextField = "Name";
            ddlmaterialclassificationid.DataValueField = "Code";
            ddlmaterialclassificationid.DataSource = _classificationrepo.GetMaterialClassifications_Code();
            ddlmaterialclassificationid.DataBind();
            ddlmaterialclassificationid.Items.Insert(0, new ListItem("Select", ""));
        }

        //Get:Packing code from packing master
        protected void Get_PackingCode()
        {
            ddlpackingcodeid.DataTextField = "Name";
            ddlpackingcodeid.DataValueField = "PackingCode";
            ddlpackingcodeid.DataSource = _packrepo.Get_PackingCode();
            ddlpackingcodeid.DataBind();
            ddlpackingcodeid.Items.Insert(0, new ListItem("Select", ""));
        }

        //Get:First Material record
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = matrepo.Get_MaterialList().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Previous record from material
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            tblMaterial next = null;
            try
            {
                next = matrepo.Get_MaterialList().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Second Record from material
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = matrepo.Get_MaterialList().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Last record from material
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = matrepo.Get_MaterialList().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        private void adderrorlog(string varerrdesc, string varerrortitle)
        {
            #region Add log to table
            Model_SystemLog log = new Model_SystemLog();
            log.UserId = Session["UserName"].ToString();
            log.LogDate = DateTime.Now;
            log.LogDescription = varerrdesc;
            log.LogTitle = varerrortitle.ToString();
            log.URL = HttpContext.Current.Request.Url.AbsoluteUri;
            logRepo.SaveSystemLog(log);
            #endregion
        }
    }
}
