using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Avery_Weigh.Repository;

namespace Avery_Weigh.VC
{
    public partial class Add : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        VehicleClassificationRepository repo = new VehicleClassificationRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetVCForUpdate();
                txtcode.Focus();
            }
        }


        protected void btnsave_Click(object sender, EventArgs e)
        {

            if (Request.QueryString["Id"] == null)
            {
                AddVC();
            }
            else
            {
                UpdateVC();
            }

        }

        //Update:VehicleClassification
        private void UpdateVC()
        {
            if (Request.QueryString["Id"] != null)
            {
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                VehicleClassification _tran = repo.Get_VehicleClassification_List().Where(x => x.Id != id && x.ClassificationCode == txtcode.Text.Trim()).SingleOrDefault();
                if (_tran != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Vehicle Classification code exist! Please try again');", true);
                }
                else
                {
                     VehicleClassification _vc = db.VehicleClassifications.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();
                    _vc.BodyType = txtBodyType.Text.ToString();
                    _vc.ClassificationCode = txtcode.Text.ToString();
                    _vc.GrossWeight = Convert.ToDecimal(txtGrossWt.Text);
                    _vc.KerbWt = Convert.ToDecimal(txtkerbWt.Text);
                    _vc.Make = txtMake.Text.ToString();
                    _vc.ManufactureYear = Convert.ToInt32(txtYearManufacture.Text.ToString());
                    _vc.Model = txtModel.Text.ToString();
                    _vc.NoOfAxies = Convert.ToDecimal(txtNoOfAxles.Text.ToString());
                    _vc.UOMWeight = ddluom.SelectedValue.ToString();
                    db.SubmitChanges();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Update Successfully')", true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "1;url=Add.aspx?id="+id;
                    this.Page.Controls.Add(meta);
                }

            }
        }

        //Get:Vehicle classification record for update
        private void GetVCForUpdate()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                divoptions.Style.Add("display", "block");
                txtcode.Enabled = false;
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                VehicleClassification _vc = repo.Get_vcById(id);
                if (_vc != null)
                {
                    txtBodyType.Text = _vc.BodyType.ToString();
                    txtcode.Text = _vc.ClassificationCode.ToString();
                    txtGrossWt.Text = _vc.GrossWeight.ToString();
                    txtkerbWt.Text = _vc.KerbWt.ToString();
                    txtMake.Text = _vc.Make.ToString();
                    txtYearManufacture.Text = _vc.ManufactureYear.ToString();
                    txtModel.Text = _vc.Model.ToString();
                    txtNoOfAxles.Text = _vc.NoOfAxies.ToString();
                    ddluom.SelectedValue = _vc.UOMWeight.Trim().ToString();
                }
            }
        }

        //Add:New VehicleClassification record
        private void AddVC()
        {
            if (Request.QueryString["Id"] == null)
            {
                VehicleClassification _tran = repo.Get_VehicleClassificationByCode(txtcode.Text.Trim());
                if (_tran != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Vehicle Classification code exist! Please try again');", true);
                }
                else
                {
                    VehicleClassification _vc = new VehicleClassification();
                    _vc.BodyType = txtBodyType.Text.ToString();
                    _vc.ClassificationCode = txtcode.Text.ToString();
                    _vc.GrossWeight = Convert.ToDecimal(txtGrossWt.Text);
                    _vc.KerbWt = Convert.ToDecimal(txtkerbWt.Text);
                    _vc.Make = txtMake.Text.ToString();
                    _vc.ManufactureYear = Convert.ToInt32(txtYearManufacture.Text.ToString());
                    _vc.Model = txtModel.Text.ToString();
                    _vc.NoOfAxies = Convert.ToDecimal(txtNoOfAxles.Text.ToString());
                    _vc.UOMWeight = ddluom.SelectedValue.ToString();
                    _vc.IsDeleted = false;
                    if (repo.Add_vc(_vc))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Saved Successfully')", true);
                        HtmlMeta meta = new HtmlMeta();
                        meta.HttpEquiv = "Refresh";
                        meta.Content = "1;url=Add.aspx";
                        this.Page.Controls.Add(meta);
                    }
                }

            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("Add.aspx");
        }

        //Get:First record
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = repo.Get_VehicleClassification_List().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("Add.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Previous record
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            VehicleClassification next = null;
            try
            {
                next = repo.Get_VehicleClassification_List().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("Add.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Next record
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = repo.Get_VehicleClassification_List().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("Add.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Last record
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = repo.Get_VehicleClassification_List().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("Add.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }
    }
}