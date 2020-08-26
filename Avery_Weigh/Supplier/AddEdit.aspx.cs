using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Model;
using Avery_Weigh.Repository;


namespace Avery_Weigh.Supplier
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SupplierRepository repo = new SupplierRepository();
        SystemLogRepository logRepo = new SystemLogRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Session["WarningShow"] = null;
                GetSupplierForUpdate();
            }

        }

        protected void btnsave(object sender, EventArgs e)
        {
            if(Request.QueryString["Id"] == null)
            {
                AddSupplier();
            }
            else
            {
                UpdateSupplier();
            }
          
        }

        //Add New Supplier
        private void AddSupplier()
        {
            tblSupplier supplier = repo.Get_SupplierbyCode(txtcode.Text.Trim());
            if (supplier != null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "toastr", "toastr.error('Same code already exist,please try again');", true);
                adderrorlog("Same code already exist,please try again", "Supplier");

            }
            else
            {
                if(db.tblSuppliers.FirstOrDefault(x=>x.Name == txtName.Text.Trim() && x.IsDeleted == false) != null)
                {
                    if (Session["WarningShow"] == null)
                    {
                        Session["WarningShow"] = "1";
                        ClientScript.RegisterStartupScript(this.GetType(), "toastr", "toastr.warning('Same name already exist. Click on save again to save record.');", true);
                        adderrorlog("Same name already exist. Click on save again to save record.", "Supplier");
                    }
                    
                }
                if (Session["WarningShow"] == "1")
                {
                    Session["WarningShow"] = "2";
                }
                else
                {
                    tblSupplier tblSupplier = new tblSupplier();
                    tblSupplier.Code = txtcode.Text.ToString();
                    tblSupplier.Name = txtName.Text.ToString();
                    tblSupplier.Address1 = txtaddress1.Text.ToString();
                    tblSupplier.Address2 = txtaddress2.Text.ToString();
                    tblSupplier.City = txtcity.Text.ToString();
                    tblSupplier.State = txtstate.Text.ToString();
                    tblSupplier.Country = txtcountry.Text.ToString();
                    tblSupplier.GSTNo = txtgstno.Text.ToString();
                    tblSupplier.PanNo = txtpanno.Text.ToString();
                    tblSupplier.ContactPerson = txtperson.Text.ToString();
                    tblSupplier.ContactMobile = txtmobile.Text.ToString();
                    tblSupplier.ContactEMail = txtemail.Text.ToString();
                    tblSupplier.IsDeleted = false;
                    if (repo.Add_Supplier(tblSupplier))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Supplier Record Saved Successfully')", true);
                        HtmlMeta meta = new HtmlMeta();
                        meta.HttpEquiv = "Refresh";
                        meta.Content = "1;url=AddEdit.aspx";
                        this.Page.Controls.Add(meta);
                    }
                }
            }
        }

        //Update:SupplierMaster 
        private void UpdateSupplier()
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            tblSupplier _sup =  repo.Get_SuppliersList().Where(x => x.Code == txtcode.Text && x.Id != id && x.IsDeleted == false).SingleOrDefault();
            if (_sup != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Supplier Code Exist! Please try again');", true);
                adderrorlog("Same Supplier Code Exist! Please try again.", "Supplier");
            }
            else
            {
                tblSupplier _supplier = repo.Get_SupplierById(id);
                if (_supplier != null)
                {
                    _supplier = db.tblSuppliers.Where(x => x.Id == id).FirstOrDefault();
                    _supplier.Code = txtcode.Text.ToString();
                    _supplier.Name = txtName.Text.ToString();
                    _supplier.Address1 = txtaddress1.Text.ToString();
                    _supplier.Address2 = txtaddress2.Text.ToString();
                    _supplier.City = txtcity.Text.ToString();
                    _supplier.State = txtstate.Text.ToString();
                    _supplier.Country = txtcountry.Text.ToString();
                    _supplier.GSTNo = txtgstno.Text.ToString();
                    _supplier.PanNo = txtpanno.Text.ToString();
                    _supplier.ContactPerson = txtperson.Text.ToString();
                    _supplier.ContactMobile = txtmobile.Text.ToString();
                    _supplier.ContactEMail = txtemail.Text.ToString();
                    _supplier.IsDeleted = false;
                    db.SubmitChanges();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Updated Successfully')", true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "1;url=AddEdit.aspx?id="+id;
                    this.Page.Controls.Add(meta);
                }
                else
                {
                    return;
                }               
            }          
        }

        //Get:Get Supplier For Update
        private void GetSupplierForUpdate()
        {
            if (Request.QueryString["Id"] != null)
            {
                txtcode.Enabled = false;
                divoptions.Style.Add("display", "block");
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                tblSupplier supplier = repo.Get_SupplierById(id);
                if (supplier != null)
                {
                    txtcode.Text = supplier.Code.ToString();
                    txtName.Text = supplier.Name.ToString();
                    txtaddress1.Text = supplier.Address1.ToString();
                    txtaddress2.Text = supplier.Address2.ToString();
                    txtcity.Text = supplier.City.ToString();
                    txtstate.Text = supplier.State.ToString();
                    txtcountry.Text = supplier.Country.ToString();
                    txtgstno.Text = supplier.GSTNo.ToString();
                    txtpanno.Text = supplier.PanNo.ToString();
                    txtperson.Text = supplier.ContactPerson.ToString();
                    txtmobile.Text = supplier.ContactMobile.ToString();
                    txtemail.Text = supplier.ContactEMail.ToString();
                }
            }
        }

        //Get:Previous Record
        protected void lnkPrevious_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            tblSupplier next = null;
            try
            {
                next = repo.Get_SuppliersList().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);           
        }

        //Get:Next Record
        protected void lnkNextFirst_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = repo.Get_SuppliersList().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if(next!= null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Last Record
        protected void lnkNextLast_Click(object sender, EventArgs e)
        {
            var next = repo.Get_SuppliersList().Where(x=> x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);           
        }

        //Get:First Record
        protected void lnkPreviousFirst_Click(object sender, EventArgs e)
        {
            var next = repo.Get_SuppliersList().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
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
