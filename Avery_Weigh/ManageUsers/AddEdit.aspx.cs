using Avery_Weigh.Model;
using Avery_Weigh.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Avery_Weigh.ManageUsers
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        PlantmasterRepository pmRepo = new PlantmasterRepository();
        UserClassificationRepository ucRepo = new UserClassificationRepository();
        UserMasterRepository umRepo = new UserMasterRepository();
        WeightMachinMasterRepository wmRepo = new WeightMachinMasterRepository();
        UserWeightMachineMasterRepository uwRepo = new UserWeightMachineMasterRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindDropdown();
                bindCheckboxList();
                if (Request.QueryString["Id"] != null)
                {
                    GetUserForUpdate();
                }
            }
        }

        //Get: Weight Machines and bind with checkbox list
        private void bindCheckboxList()
        {
            IEnumerable<Model_WeightMachinMaster> data = wmRepo.GetWeightMachinMastersList();
            chkWeighbridgeId.DataSource = data;
            chkWeighbridgeId.DataTextField = "MachineId";
            chkWeighbridgeId.DataValueField = "Id";
            chkWeighbridgeId.DataBind();
        }

        //Bind PlantCode and UserType Dropdownlist
        private void bindDropdown()
        {
            ddlPlantList.DataSource = pmRepo.Get_PlantCodeId(); //Retruns the plant code to the data source
            ddlPlantList.DataTextField = "PlantName";
            ddlPlantList.DataValueField = "PlantCode";
            ddlPlantList.DataBind();
            ddlPlantList.Items.Insert(0, new ListItem("Select", "0"));

            ddlUserTypeList.DataSource = ucRepo.Get_ActiveUserClassificationList(); // return the user type and id to the data source
            ddlUserTypeList.DataTextField = "UserType";
            ddlUserTypeList.DataValueField = "Id";
            ddlUserTypeList.DataBind();
            ddlUserTypeList.Items.Insert(0, new ListItem("Select", "0"));
        }

        //Call the addnew or update method on save button click
        protected void btnsave_Click(object sender, EventArgs e)
        {
            if (Session["UserName"].ToString().ToUpper() != "admin".ToUpper())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('user account doesn’t have permission.');", true);
                return;
               // Response.Redirect(Request.UrlReferrer.ToString());

            }
            if (Request.QueryString["Id"] == null)
            {
                AddUser();
            }
            else
            {
                UpdateUser();
            }
        }

        //Add New user master to the database
        private void AddUser()
        {
            try
            {
                UserMaster _user = umRepo.Get_UserMasterByUserName(txtUserName.Text.Trim());
                if (_user != null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "toastr", "toastr.error('Same User Name already Exist! Please try again');", true);
                }
                else
                {
                    int Userid = 0;
                    UserMaster uM = new UserMaster();
                    uM.Password = txtPassword.Text.ToString();
                    uM.Plantcode = ddlPlantList.SelectedValue;
                    uM.UserType = Convert.ToInt32(ddlUserTypeList.SelectedValue);
                    uM.UserName = txtUserName.Text;
                    
                    uM.IsDeleted = false;
                    if (umRepo.Add_UserMaster(uM))
                    {
                        Userid = uM.Id;

                        for (int i = 0; i < chkWeighbridgeId.Items.Count; i++)
                        {
                            if (chkWeighbridgeId.Items[i].Selected)
                            {
                                UserWeightMachineMaster uwm = new UserWeightMachineMaster();
                                uwm.UserId = Userid;
                                uwm.WeightMachineId = Convert.ToInt32(chkWeighbridgeId.Items[i].Value);
                                uwRepo.Add_UserWeightMachineMaster(uwm);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('User Saved Successfully')", true);
                        HtmlMeta meta = new HtmlMeta();
                        meta.HttpEquiv = "Refresh";
                        meta.Content = "1;url=AddEdit.aspx";
                        this.Page.Controls.Add(meta);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
            }

        }

        //Update User Master
        private void UpdateUser()
        {
            if (Request.QueryString["Id"] != null)
            {
                try
                {
                    int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                    UserMaster _mat = umRepo.Get_UserMaster_List().Where(x => x.UserName == txtUserName.Text.Trim() && x.Id != id).FirstOrDefault();
                    if (_mat != null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same User Name Exist! Please try again');", true);
                    }
                    else
                    {
                        UserMaster uM = db.UserMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
                        if (uM != null)
                        {
                            uM.Password = txtPassword.Text.ToString();
                            uM.Plantcode =ddlPlantList.SelectedValue;
                            uM.UserType = Convert.ToInt32(ddlUserTypeList.SelectedValue);
                            uM.UserName = txtUserName.Text;
                            db.SubmitChanges();
                            for (int i = 0; i < chkWeighbridgeId.Items.Count; i++)
                            {
                                if (chkWeighbridgeId.Items[i].Selected)
                                {
                                    UserWeightMachineMaster uwm = db.UserWeightMachineMasters.FirstOrDefault(x => x.UserId == uM.Id && x.WeightMachineId == Convert.ToInt32(chkWeighbridgeId.Items[i].Value));
                                    if (uwm == null)
                                    {
                                        uwm = new UserWeightMachineMaster();
                                        uwm.UserId = uM.Id;
                                        uwm.WeightMachineId = Convert.ToInt32(chkWeighbridgeId.Items[i].Value);
                                        db.UserWeightMachineMasters.InsertOnSubmit(uwm);
                                        db.SubmitChanges();
                                    }
                                }
                                else
                                {
                                    UserWeightMachineMaster uwm = db.UserWeightMachineMasters.FirstOrDefault(x => x.UserId == uM.Id && x.WeightMachineId == Convert.ToInt32(chkWeighbridgeId.Items[i].Value));
                                    if (uwm != null)
                                    {
                                        db.UserWeightMachineMasters.DeleteOnSubmit(uwm);
                                        db.SubmitChanges();
                                    }
                                }
                            }

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('User Updated Successfully')", true);
                            HtmlMeta meta = new HtmlMeta();
                            meta.HttpEquiv = "Refresh";
                            meta.Content = "1;url=AddEdit.aspx?id="+id;
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

        //Get:User Master by id for update
        private void GetUserForUpdate()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                txtUserName.Enabled = false;
                divoptions.Style.Add("display", "block");
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                UserMaster uM = umRepo.GetUserMaster_ById(id);
                if (uM != null)
                {
                    txtUserName.Text = uM.UserName;
                    txtPassword.Text = uM.Password;
                    ddlPlantList.SelectedValue = uM.Plantcode.ToString();
                    ddlUserTypeList.SelectedValue = uM.UserType.ToString();
                }
                IEnumerable<UserWeightMachineMaster> getUserMachines = umRepo.getUserWeighMachines(id);
                foreach (UserWeightMachineMaster m in getUserMachines.ToList())
                {
                    for (int i = 0; i < chkWeighbridgeId.Items.Count; i++)
                    {
                        if (chkWeighbridgeId.Items[i].Value == m.WeightMachineId.ToString())
                        {
                            chkWeighbridgeId.Items[i].Selected = true;
                        }
                    }
                }
            }
        }

        //Retrun the first record
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = umRepo.Get_UserMaster_List().ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //return the previous record
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            UserMaster next = null;
            try
            {
                next = umRepo.Get_UserMaster_List().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //return the next record
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = umRepo.Get_UserMaster_List().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //return the last record
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = umRepo.Get_UserMaster_List().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }
    }
}