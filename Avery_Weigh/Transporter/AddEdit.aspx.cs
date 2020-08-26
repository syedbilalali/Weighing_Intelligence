using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Avery_Weigh.Repository;
using Avery_Weigh.Model;

namespace Avery_Weigh.Transporter
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        TransporterRepository repo = new TransporterRepository();
        SystemLogRepository logRepo = new SystemLogRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetTransporterForUpdate();
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {

            if (Request.QueryString["Id"] == null)
            {
                AddTransporter();
            }
            else
            {
                UpdateTransporter();
            }

        }

        //Get:Transporter for update
        private void GetTransporterForUpdate()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                divoptions.Style.Add("display", "block");
                txtcode.Enabled = false;
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                tblTransporter _transporter = repo.Get_TransporterById(id);
                if (_transporter != null)
                {
                    txtcode.Text = _transporter.Code.ToString();
                    txtName.Text = _transporter.Name.ToString();
                    txtaddress1.Text = _transporter.Address1.ToString();
                    txtaddress2.Text = _transporter.Address2.ToString();
                    txtcity.Text = _transporter.City.ToString();
                    txtstate.Text = _transporter.State.ToString();
                    txtcountry.Text = _transporter.Country.ToString();
                    txtgstno.Text = _transporter.GSTNo.ToString();
                    txtpanno.Text = _transporter.PanNo.ToString();
                    txtperson.Text = _transporter.ContactPerson.ToString();
                    txtmobile.Text = _transporter.ContactMobile.ToString();
                    txtemail.Text = _transporter.ContactEmail.ToString();
                }
            }
        }   

        //Add:New Transporter
        private void AddTransporter()
        {
            if (Request.QueryString["Id"] == null)
            {
                tblTransporter _tran = repo.GetTransporter_ByCode(txtcode.Text.Trim());
                if (_tran != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Transporter code exist! Please try again');", true);
                    adderrorlog("Same Transporter code exist! Please try again", "Transporter");
                }
                else
                {
                    tblTransporter _trans = new tblTransporter();
                    _trans.Code = txtcode.Text.ToString();
                    _trans.Name = txtName.Text.ToString();
                    _trans.Address1 = txtaddress1.Text.ToString();
                    _trans.Address2 = txtaddress2.Text.ToString();
                    _trans.City = txtcity.Text.ToString();
                    _trans.State = txtstate.Text.ToString();
                    _trans.Country = txtcountry.Text.ToString();
                    _trans.GSTNo = txtgstno.Text.ToString();
                    _trans.PanNo = txtpanno.Text.ToString();
                    _trans.ContactPerson = txtperson.Text.ToString();
                    _trans.ContactMobile = txtmobile.Text.ToString();
                    _trans.ContactEmail = txtemail.Text.ToString();
                    _trans.IsDeleted = false;
                    if (repo.Add_Transporter(_trans))
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
        } 

        //Update:Transporter
        private void UpdateTransporter()
        {
            if (Request.QueryString["Id"] != null)
            {
                try
                {
                    int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                    tblTransporter _tans = repo.GetTransportersList().Where(x => x.Code == txtcode.Text && x.IsDeleted == false && x.Id != id).FirstOrDefault();
                    if (_tans != null)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "toastr", "toastr.error('Same Transporter code exist! Please try again');", true);
                        adderrorlog("Same Transporter code exist! Please try again", "Transporter");
                    }
                    else
                    {
                        tblTransporter _trans = db.tblTransporters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
                        _trans.Code = txtcode.Text.ToString();
                        _trans.Name = txtName.Text.ToString();
                        _trans.Address1 = txtaddress1.Text.ToString();
                        _trans.Address2 = txtaddress2.Text.ToString();
                        _trans.City = txtcity.Text.ToString();
                        _trans.State = txtstate.Text.ToString();
                        _trans.Country = txtcountry.Text.ToString();
                        _trans.GSTNo = txtgstno.Text.ToString();
                        _trans.PanNo = txtpanno.Text.ToString();
                        _trans.ContactPerson = txtperson.Text.ToString();
                        _trans.ContactMobile = txtmobile.Text.ToString();
                        _trans.ContactEmail = txtemail.Text.ToString();
                        db.SubmitChanges();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Updated Successfully')", true);
                        HtmlMeta meta = new HtmlMeta
                        {
                            HttpEquiv = "Refresh",
                            Content = "3;url=AddEdit.aspx?id="+id
                        };
                        this.Page.Controls.Add(meta);

                    }
                }
                catch { }
            }
        }
        
        //Get:First Transporter record
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = repo.GetTransportersList().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Previous Transporter record
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            tblTransporter next = null;
            try
            {
                next = repo.GetTransportersList().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Next Transporter record
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = repo.GetTransportersList().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Last Transporter record
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = repo.GetTransportersList().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
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