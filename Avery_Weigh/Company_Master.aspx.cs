using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Avery_Weigh
{
    public partial class Company_Master : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                    Response.Redirect("/login.aspx");
                bindData();
            }
        }

        private void bindData()
        {
            IEnumerable<CompanyMaster> company = db.CompanyMasters.ToList();
            if (company.Count() >= 2)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('Multiple Record found. Please contact with service engineer.');", true);
            }
            else
            {
                CompanyMaster _company = db.CompanyMasters.FirstOrDefault(x => x.Id == 1);
                if (_company != null)
                {
                    txtCode.Text = _company.CompanyCode;
                    txtAddress1.Text = _company.CompanyAddress1;
                    txtAddress2.Text = _company.CompanyAddress2;
                    txtContactEmail.Text = _company.ContactEmail;
                    txtContactMobile.Text = _company.ContactMobile;
                    txtContactPerson.Text = _company.ContactPerson;
                    txtName.Text = _company.CompanyName;
                    if (!string.IsNullOrEmpty(_company.CompanyLogo))
                    {
                        companyLogo.Visible = true;
                        companyLogo.ImageUrl = "/images/companylogo/" + _company.CompanyLogo;
                        imgLogo.Value = _company.CompanyLogo;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyMaster company = db.CompanyMasters.FirstOrDefault(x => x.Id == 1);
                if (company == null)
                {
                    company = new CompanyMaster();
                    company.CompanyAddress1 = txtAddress1.Text;
                    company.CompanyAddress2 = txtAddress2.Text;
                    company.CompanyName = txtName.Text;
                    company.CompanyCode= txtCode.Text;
                    company.ContactEmail = txtContactEmail.Text;
                    company.ContactMobile = txtContactMobile.Text;
                    company.ContactPerson = txtContactPerson.Text;
                    if (upload1.HasFile)
                    {
                        string filename = upload1.PostedFile.FileName;
                        upload1.SaveAs(Server.MapPath("~/images/companylogo/") + upload1.PostedFile.FileName);
                        company.CompanyLogo = filename;
                    }
                    db.CompanyMasters.InsertOnSubmit(company);
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Company record added successfully.');", true);
                }
                else
                {
                    company.CompanyAddress1 = txtAddress1.Text;
                    company.CompanyAddress2 = txtAddress2.Text;
                    company.CompanyName = txtName.Text;
                    company.CompanyCode = txtCode.Text;
                    company.ContactEmail = txtContactEmail.Text;
                    company.ContactMobile = txtContactMobile.Text;
                    company.ContactPerson = txtContactPerson.Text;
                    if (upload1.HasFile)
                    {
                        string filename = upload1.PostedFile.FileName;
                        upload1.SaveAs(Server.MapPath("~/images/companylogo/") + upload1.PostedFile.FileName);
                        company.CompanyLogo = filename;
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Company record updated successfully.');", true);
                }
                db.SubmitChanges();
                bindData();
            }
            catch(Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('"+ ex.Message.ToString() + "');", true);
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyMaster company = db.CompanyMasters.FirstOrDefault(x => x.Id == 1);
                if (company == null)
                {
                    company = new CompanyMaster();
                    company.CompanyAddress1 = txtAddress1.Text;
                    company.CompanyAddress2 = txtAddress2.Text;
                    company.CompanyName = txtName.Text;
                    company.CompanyCode = txtCode.Text;
                    company.ContactEmail = txtContactEmail.Text;
                    company.ContactMobile = txtContactMobile.Text;
                    company.ContactPerson = txtContactPerson.Text;
                    if (upload1.HasFile)
                    {
                        string filename = upload1.PostedFile.FileName;
                        upload1.SaveAs(Server.MapPath("~/images/companylogo/") + upload1.PostedFile.FileName);
                        company.CompanyLogo = filename;
                    }
                    db.CompanyMasters.InsertOnSubmit(company);
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Company record added successfully.');", true);
                }
                else
                {
                    company.CompanyAddress1 = txtAddress1.Text;
                    company.CompanyAddress2 = txtAddress2.Text;
                    company.CompanyName = txtName.Text;
                    company.ContactEmail = txtContactEmail.Text;
                    company.ContactMobile = txtContactMobile.Text;
                    company.ContactPerson = txtContactPerson.Text;
                    if (upload1.HasFile)
                    {
                        string filename = upload1.PostedFile.FileName;
                        upload1.SaveAs(Server.MapPath("~/images/companylogo/") + upload1.PostedFile.FileName);
                        company.CompanyLogo = filename;
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Company record updated successfully.');", true);
                }
                db.SubmitChanges();
                bindData();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('" + ex.Message.ToString() + "');", true);
            }
        }
    }
}