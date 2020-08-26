using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Model;
using Avery_Weigh.Repository;

namespace Avery_Weigh.UsersClassification
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        UserClassificationRepository repo = new UserClassificationRepository();
        SystemLogRepository logRepo = new SystemLogRepository();
        UserMasterRepository umRepo = new UserMasterRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_UserClassificationForUpdate();
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["Id"]))
                Add_UserClassification();
            else
                Update_UserClassification();
        }


        //:Add New UserType in UserClassification
        private void Add_UserClassification()
        {
            if (Session["UserName"].ToString().ToUpper() != "admin".ToUpper())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('user account doesn’t have permission.');", true);
                Response.Redirect(Request.UrlReferrer.ToString());

            }

            if (Request.QueryString["Id"] == null)
            {
                try
                {
                    UserClassification Uc = repo.GetUserClassification_by_UserType(txtUserType.Text.Trim());
                    if (Uc != null)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('Same User type exist. Please try again later.');", true);
                        adderrorlog("Same User type exist. Please try again later.", "User Classifications");
                    }
                    else
                    {
                        Uc = new UserClassification();
                        Uc.UserType = txtUserType.Text.Trim();
                        Uc.Configuration = ddlConfiguaration.SelectedItem.Value == "0" ? false : true;
                        Uc.DatabaseOperation = ddlDatabaseOperation.SelectedItem.Value == "0" ? false : true;
                        Uc.GateEntry = ddlGateEntry.SelectedItem.Value == "0" ? false : true;
                        Uc.MasterFileUpdation = ddlMasterFileDeletion.SelectedItem.Value == "0" ? false : true;
                        Uc.MasterRecordDeletion = ddlMasterRecordDeletion.SelectedItem.Value == "0" ? false : true;
                        Uc.PasswordPolicy = ddlPasswordPolicy.SelectedItem.Value == "0" ? false : true;
                        Uc.PasswordReset = ddlPasswordReset.SelectedItem.Value == "0" ? false : true;
                        Uc.PendingRecordDeletion = ddlPendingRecordDeletion.SelectedItem.Value == "0" ? false : true;
                        Uc.RFIDIssue = ddlRFIDIssue.SelectedItem.Value == "0" ? false : true;
                        Uc.TransactionDeletion = ddlTransactionDeletion.SelectedItem.Value == "0" ? false : true;
                        Uc.UserCreation = ddlUserCreation.SelectedItem.Value == "0" ? false : true;
                        Uc.Weighment = ddlWeighment.SelectedItem.Value == "0" ? false : true;
                        Uc.TareToleranceApproval  = ddlTareTolApproval.SelectedItem.Value == "0" ? false : true;
                        Uc.IsDeleted = false;
                        if (repo.Add_UserClassification(Uc))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('User Classification added successfully.');", true);
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
                    adderrorlog(ex.Message.ToString(), "User Classifications");
                }
            }
        }

        //Get:Update User Classification
        private void Update_UserClassification()
        {
            //User.Identity.Name 
            
            if (Session["UserName"].ToString().ToUpper() !="admin".ToUpper())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('user account doesn’t have permission.');", true);
                return;
                //Response.Redirect(Request.UrlReferrer.ToString());
               
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                try
                {
                    int Id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                    //UserClassification Uc = repo.GetUserClassification_by_Id(Id);
                    UserClassification Uc = db.UserClassifications.FirstOrDefault(x => x.Id == Id && x.IsDeleted == false);
                    if (Uc != null)
                    {
                        UserClassification _data = repo.Get_ActiveUserClassificationList().Where(x => x.UserType == txtUserType.Text && x.IsDeleted == false && x.Id != Id).FirstOrDefault();
                        if (_data != null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same usertype exist.please try again.');", true);
                        }
                        else
                        {
                            Uc.UserType = txtUserType.Text;
                            Uc.Configuration = ddlConfiguaration.SelectedItem.Value == "0" ? false : true;
                            Uc.DatabaseOperation = ddlDatabaseOperation.SelectedItem.Value == "0" ? false : true;
                            Uc.GateEntry = ddlGateEntry.SelectedItem.Value == "0" ? false : true;
                            Uc.MasterFileUpdation = ddlMasterFileDeletion.SelectedItem.Value == "0" ? false : true;
                            Uc.MasterRecordDeletion = ddlMasterRecordDeletion.SelectedItem.Value == "0" ? false : true;
                            Uc.PasswordPolicy = ddlPasswordPolicy.SelectedItem.Value == "0" ? false : true;
                            Uc.PasswordReset = ddlPasswordReset.SelectedItem.Value == "0" ? false : true;
                            Uc.PendingRecordDeletion = ddlPendingRecordDeletion.SelectedItem.Value == "0" ? false : true;
                            Uc.RFIDIssue = ddlRFIDIssue.SelectedItem.Value == "0" ? false : true;
                            Uc.TransactionDeletion = ddlTransactionDeletion.SelectedItem.Value == "0" ? false : true;
                            Uc.UserCreation = ddlUserCreation.SelectedItem.Value == "0" ? false : true;
                            Uc.Weighment = ddlWeighment.SelectedItem.Value == "0" ? false : true;
                            Uc.TareToleranceApproval = ddlTareTolApproval.SelectedItem.Value == "0" ? false : true;
                            db.SubmitChanges();
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('User Classification updated successfully.');", true);
                            HtmlMeta meta = new HtmlMeta
                            {
                                HttpEquiv = "Refresh",
                                Content = "1;url=AddEdit.aspx?id=" + Id
                            };
                            this.Page.Controls.Add(meta);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
                }
            }
        }

        //Get:User Classfication for update
        private void Get_UserClassificationForUpdate()
        {
            if (Request.QueryString["Id"] != null)
            {
                try
                {
                    divoptions.Style.Add("display", "block");
                    txtUserType.Enabled = false;
                    int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                    UserClassification Uc = repo.GetUserClassification_by_Id(id);
                    if (Uc != null)
                    {
                        txtUserType.Text = Uc.UserType;
                        ddlConfiguaration.SelectedValue = ddlConfiguaration.Items.FindByValue(Uc.Configuration.Value == true ? "1" : "0").Value;
                        ddlDatabaseOperation.SelectedValue = ddlDatabaseOperation.Items.FindByValue(Uc.DatabaseOperation.Value == true ? "1" : "0").Value;
                        ddlGateEntry.SelectedValue = ddlGateEntry.Items.FindByValue(Uc.GateEntry.Value == true ? "1" : "0").Value;
                        ddlMasterFileDeletion.SelectedValue = ddlMasterFileDeletion.Items.FindByValue(Uc.MasterFileUpdation.Value == true ? "1" : "0").Value;
                        ddlMasterRecordDeletion.SelectedValue = ddlMasterRecordDeletion.Items.FindByValue(Uc.MasterRecordDeletion.Value == true ? "1" : "0").Value;
                        ddlPasswordPolicy.SelectedValue = ddlPasswordPolicy.Items.FindByValue(Uc.PasswordPolicy.Value == true ? "1" : "0").Value;
                        ddlPasswordReset.SelectedValue = ddlPasswordReset.Items.FindByValue(Uc.PasswordReset.Value == true ? "1" : "0").Value;
                        ddlPendingRecordDeletion.SelectedValue = ddlPendingRecordDeletion.Items.FindByValue(Uc.PendingRecordDeletion.Value == true ? "1" : "0").Value;
                        ddlRFIDIssue.SelectedValue = ddlRFIDIssue.Items.FindByValue(Uc.RFIDIssue.Value == true ? "1" : "0").Value;
                        ddlTransactionDeletion.SelectedValue = ddlTransactionDeletion.Items.FindByValue(Uc.TransactionDeletion.Value == true ? "1" : "0").Value;
                        ddlUserCreation.SelectedValue = ddlUserCreation.Items.FindByValue(Uc.UserCreation.Value == true ? "1" : "0").Value;
                        ddlWeighment.SelectedValue = ddlWeighment.Items.FindByValue(Uc.Weighment.Value == true ? "1" : "0").Value;
                        ddlTareTolApproval.SelectedValue = ddlTareTolApproval.Items.FindByValue(Uc.TareToleranceApproval.Value == true ? "1" : "0").Value;
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
                }
            }
        }

        //Get:First record from user classification
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = repo.Get_ActiveUserClassificationList().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Previous Record from user classification
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            UserClassification next = null;
            try
            {
                next = repo.Get_ActiveUserClassificationList().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Next Record from user classification
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = repo.Get_ActiveUserClassificationList().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Last record from user classification
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = repo.Get_ActiveUserClassificationList().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
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