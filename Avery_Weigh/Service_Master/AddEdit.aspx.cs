using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;

namespace Avery_Weigh.Service_Master
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        ServiceMasterRepository smrepo = new ServiceMasterRepository();
        WeightMachinMasterRepository _repo = new WeightMachinMasterRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_ServiceMaster();
                bindData();
            }
        }

        protected void Btnsave_Click(object sender, EventArgs e)
        {
            if (Session["UserName"].ToString().ToUpper() != "admin".ToUpper())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('user account doesn’t have permission.');", true);
                return;
                // Response.Redirect(Request.UrlReferrer.ToString());

            }
            ServiceMaster sm = db.ServiceMasters.FirstOrDefault(x => x.Id == 1);
            if (sm == null)   // (string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Add();
            }
            else
            {
                Update();
            }
        }

        private void bindData()
        {
            IEnumerable<ServiceMaster> service = db.ServiceMasters.ToList();
            if (service.Count() >= 2)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('Multiple Record found. Please contact with service engineer.');", true);
            }
            else
            {
                ServiceMaster sm = db.ServiceMasters.FirstOrDefault(x => x.Id == 1);
                if (sm != null)
                {
                    ddlamctype.SelectedValue = sm.AMCType;
                    ddlWarrantee.SelectedValue = sm.Warrantee;
                    ddlGarrantee.SelectedValue = sm.Garrantee;
                    txtamccontactnumber.Text = sm.AMCContactNo.ToString();
                    txtamcreminder.Text = sm.AMCReminder.ToString();
                    DateTime? validupto = sm.AMCValidUpto;
                    string AMCValidUpto = String.Format("{0:dd/MM/yyyy}", validupto);
                    DateTime? stampingdate = sm.StampingDate;
                    string Stampingdate = string.Format("{0:dd/MM/yyyy}", stampingdate);
                    txtamcvalidupto.Text = AMCValidUpto;
                    txtstampingdate.Text = Stampingdate;
                    txtstampingreminder.Text = sm.StampingReminder.ToString();
                }
            }
        }

        protected void Add()
        {
            try
            {
                if (!string.IsNullOrEmpty(ddlamctype.SelectedValue))
                {
                    ServiceMaster sm = new ServiceMaster();
                    sm.AMCType = ddlamctype.SelectedValue;
                    sm.AMCContactNo = txtamccontactnumber.Text.Trim().ToString();
                    if (txtamcvalidupto.Text.Trim().Length != 0)
                    {
                        DateTime AMCValidUpto = DateTime.ParseExact(txtamcvalidupto.Text, "dd/MM/yyyy", new CultureInfo("en-GB"));
                        sm.AMCValidUpto = AMCValidUpto;
                    }
                    sm.AMCReminder = Convert.ToInt32(txtamcreminder.Text.Trim());
                    sm.StampingReminder = Convert.ToInt32(txtstampingreminder.Text.Trim());
                    DateTime Stampingdate = DateTime.ParseExact(txtstampingdate.Text, "dd/MM/yyyy", new CultureInfo("en-GB"));
                    sm.StampingDate = Stampingdate;
                    sm.IsDeleted = false;
                    sm.Warrantee = ddlWarrantee.SelectedValue;
                    sm.Garrantee = ddlGarrantee.SelectedValue;

                    //new
                    try
                    {
                        string filename = string.Empty;
                        //if (ddlamctype.SelectedItem.Text.ToLower().Contains("gold"))
                        //{
                        //    filename = "gold";
                        //}
                        //else if (ddlamctype.SelectedItem.Text.ToLower().Contains("silver"))
                        //{
                        //    filename = "silver";
                        //}
                        //else if (ddlamctype.SelectedItem.Text.ToLower().Contains("platinum"))
                        //{
                        //    filename = "platinum";
                        //}
                        //if (ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                        //{
                        //    filename = filename + "_no_warrantee";
                        //}
                        //else if (ddlWarrantee.SelectedValue.ToLower().Contains("yes"))
                        //{
                        //    filename = filename + "_yes_warrantee";
                        //}
                        //if (ddlGarrantee.SelectedValue.ToLower().Contains("no"))
                        //{
                        //    filename = filename + "_no_guarantee";
                        //}
                        //else if (ddlGarrantee.SelectedValue.ToLower().Contains("yes"))
                        //{
                        //    filename = filename + "_yes_guarantee";
                        //}

                        if (ddlamctype.SelectedItem.Text.ToLower().Contains("gold") && Convert.ToDateTime(txtamcvalidupto.Text.Trim())<=DateTime.Now  && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                        {
                            filename = "gold_amc";
                        }
                        else if (ddlamctype.SelectedItem.Text.ToLower().Contains("gold") && Convert.ToDateTime(txtamcvalidupto.Text.Trim()) >= DateTime.Now && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                        {
                            filename = "gold_amc_exp";
                        }
                        else if (ddlamctype.SelectedItem.Text.ToLower().Contains("silver") && Convert.ToDateTime(txtamcvalidupto.Text.Trim()) <= DateTime.Now && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                        {
                            filename = "silver_amc";
                        }
                        else if (ddlamctype.SelectedItem.Text.ToLower().Contains("silver") && Convert.ToDateTime(txtamcvalidupto.Text.Trim()) >= DateTime.Now && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                        {
                            filename = "silver_amc_exp";
                        }
                        else if (ddlamctype.SelectedItem.Text.ToLower().Contains("platinum") && Convert.ToDateTime(txtamcvalidupto.Text.Trim()) <= DateTime.Now && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                        {
                            filename = "platinum_amc";
                        }
                        else if (ddlamctype.SelectedItem.Text.ToLower().Contains("platinum") && Convert.ToDateTime(txtamcvalidupto.Text.Trim()) >= DateTime.Now && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                        {
                            filename = "platinum_amc_exp";
                        }
                        else if (ddlamctype.SelectedItem.Text.ToLower().Contains("none") && ddlWarrantee.SelectedValue.ToLower().Contains("yes"))
                        {

                            var varwarrntydate = _repo.Get_WeightMachineMasterById_ss(Session["PlantID"].ToString(), Session["WBID"].ToString());
                            if (Convert.ToDateTime(varwarrntydate.WarrentyUpto) >= DateTime.Now)
                            {
                                filename = "gold_warranty_yes";
                            }
                            else
                            {
                                filename = "gold_warranty_no";
                            }
                        }
                        else if (ddlamctype.SelectedItem.Text.ToLower().Contains("none") && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                        {
                             filename = "gold_warranty_no";
                 
                        }

                        filename = filename + ".png";
                        System.IO.File.Delete(Server.MapPath("~/images/login/gold_yes_warrantee_yes_guarantee.png"));
                        System.IO.File.Copy(Server.MapPath("~/images/amc/" + filename), Server.MapPath("~/images/login/gold_yes_warrantee_yes_guarantee.png"));
                    }
                    catch { }
                    //end
                    if (smrepo.Add_ServiceMaster(sm))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Save Successfully');", true);
                        HtmlMeta meta = new HtmlMeta();
                        meta.HttpEquiv = "Refresh";
                        meta.Content = "1.30;url=AddEdit.aspx";
                        this.Page.Controls.Add(meta);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex.Message.ToString() + "');", true);
            }
        }

        protected void Get_ServiceMaster()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                int id = Convert.ToInt32(Request.QueryString["Id"]);
                ServiceMaster sm = smrepo.Get_ServiceMasterById(id);
                if (sm != null)
                {
                    ddlamctype.SelectedValue = sm.AMCType;
                    txtamccontactnumber.Text = sm.AMCContactNo.ToString();
                    txtamcreminder.Text = sm.AMCReminder.ToString();
                    //DateTime? validupto = sm.AMCValidUpto;
                    //string AMCValidUpto = String.Format("{0:dd/MM/yyyy}", validupto);
                    //DateTime? stampingdate = sm.StampingDate;
                    //string Stampingdate = string.Format("{0:dd/MM/yyyy}", stampingdate);
                    if (sm.AMCValidUpto != null)
                    {
                        txtamcvalidupto.Text = sm.AMCValidUpto.Value.ToString();
                    }
                    txtstampingdate.Text = sm.StampingDate.Value.ToString();
                    txtstampingreminder.Text = sm.StampingReminder.ToString();
                    ddlWarrantee.SelectedValue = sm.Warrantee;
                    ddlGarrantee.SelectedValue = sm.Garrantee;
                }
            }
        }

        protected void Update()
        {
            //if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            //{
            //    RegexRepository r = new RegexRepository();
            int id = 1;  // Convert.ToInt32(Request.QueryString["Id"]);
            ServiceMaster sm = db.ServiceMasters.FirstOrDefault(x => x.Id == 1 && x.IsDeleted == false);
            if (sm != null)
            {
                sm.AMCType = ddlamctype.SelectedValue;
                sm.AMCContactNo = txtamccontactnumber.Text.Trim().ToString();
                //DateTime AMCValidUpto = DateTime.ParseExact(txtamcvalidupto.Text, "dd/MM/yyyy", new CultureInfo("en-GB"));
                if (txtamcvalidupto.Text.Trim().Length != 0)
                {
                    DateTime AMCValidUpto = Convert.ToDateTime(txtamcvalidupto.Text.Trim());  //, "dd/MM/yyyy", new CultureInfo("en-GB"));
                    sm.AMCValidUpto = AMCValidUpto;
                }
                else
                {
                    sm.AMCValidUpto = null;
                }
                sm.AMCReminder = Convert.ToInt32(txtamcreminder.Text.Trim());
                sm.StampingReminder = Convert.ToInt32(txtstampingreminder.Text.Trim());
                //DateTime Stampingdate = DateTime.ParseExact(txtstampingdate.Text, "dd/MM/yyyy", new CultureInfo("en-GB"));
                DateTime Stampingdate = Convert.ToDateTime(txtstampingdate.Text.Trim());  //, "dd/MM/yyyy", new CultureInfo("en-GB"));
                sm.StampingDate = Stampingdate;
                sm.Warrantee = ddlWarrantee.SelectedValue;
                sm.Garrantee = ddlGarrantee.SelectedValue;
                db.SubmitChanges();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Update Successfully');", true);
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "1.30;url=AddEdit.aspx?id=" + id;
                this.Page.Controls.Add(meta);
                try
                {
                    string filename = string.Empty;
                    //if (ddlamctype.SelectedItem.Text.ToLower().Contains("gold"))
                    //{
                    //    filename = "gold";
                    //}
                    //else if (ddlamctype.SelectedItem.Text.ToLower().Contains("silver"))
                    //{
                    //    filename = "silver";
                    //}
                    //else if (ddlamctype.SelectedItem.Text.ToLower().Contains("platinum"))
                    //{
                    //    filename = "platinum";
                    //}
                    //if (ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                    //{
                    //    filename = filename + "_no_warrantee";
                    //}
                    //else if (ddlWarrantee.SelectedValue.ToLower().Contains("yes"))
                    //{
                    //    filename = filename + "_yes_warrantee";
                    //}
                    //if (ddlGarrantee.SelectedValue.ToLower().Contains("no"))
                    //{
                    //    filename = filename + "_no_guarantee";
                    //}
                    //else if (ddlGarrantee.SelectedValue.ToLower().Contains("yes"))
                    //{
                    //    filename = filename + "_yes_guarantee";
                    //}

                    if (ddlamctype.SelectedItem.Text.ToLower().Contains("gold") && Convert.ToDateTime(txtamcvalidupto.Text.Trim()) <= DateTime.Now && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                    {
                        filename = "gold_amc";
                    }
                    else if (ddlamctype.SelectedItem.Text.ToLower().Contains("gold") && Convert.ToDateTime(txtamcvalidupto.Text.Trim()) >= DateTime.Now && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                    {
                        filename = "gold_amc_exp";
                    }
                    else if (ddlamctype.SelectedItem.Text.ToLower().Contains("silver") && Convert.ToDateTime(txtamcvalidupto.Text.Trim()) <= DateTime.Now && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                    {
                        filename = "silver_amc";
                    }
                    else if (ddlamctype.SelectedItem.Text.ToLower().Contains("silver") && Convert.ToDateTime(txtamcvalidupto.Text.Trim()) >= DateTime.Now && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                    {
                        filename = "silver_amc_exp";
                    }
                    else if (ddlamctype.SelectedItem.Text.ToLower().Contains("platinum") && Convert.ToDateTime(txtamcvalidupto.Text.Trim()) <= DateTime.Now && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                    {
                        filename = "platinum_amc";
                    }
                    else if (ddlamctype.SelectedItem.Text.ToLower().Contains("platinum") && Convert.ToDateTime(txtamcvalidupto.Text.Trim()) >= DateTime.Now && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                    {
                        filename = "platinum_amc_exp";
                    }
                    else if (ddlamctype.SelectedItem.Text.ToLower().Contains("none") &&  ddlWarrantee.SelectedValue.ToLower().Contains("yes"))
                    {
                        
                        var varwarrntydate=_repo.Get_WeightMachineMasterById_ss(Session["PlantID"].ToString(), Session["WBID"].ToString());
                        if ( Convert.ToDateTime(varwarrntydate.WarrentyUpto)>=DateTime.Now )
                        {
                            filename = "gold_warranty_yes";
                        }
                        else
                        {
                            filename = "gold_warranty_no";
                        }
                    }
                    else if (ddlamctype.SelectedItem.Text.ToLower().Contains("none") && ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                    {
                        filename = "gold_warranty_no";

                    }

                    //Session["WBID"] = _WBID;
                    //Session["UserId"] = um.Id;
                    //Session["PlantID"] = _PlantID;

                    filename = filename + ".png";
                    System.IO.File.Delete(Server.MapPath("~/images/login/gold_yes_warrantee_yes_guarantee.png"));
                    System.IO.File.Copy(Server.MapPath("~/images/amc/" + filename), Server.MapPath("~/images/login/gold_yes_warrantee_yes_guarantee.png"));
                }
                catch { }
            }
            //}
        }

        protected void AutoCheckInLoginTime()
        {
          
            int id = 1;  // Convert.ToInt32(Request.QueryString["Id"]);
            ServiceMaster sm = db.ServiceMasters.FirstOrDefault(x => x.Id == 1 && x.IsDeleted == false);
            if (sm != null)
            {
                ////sm.AMCType = ddlamctype.SelectedValue;
                ////sm.AMCContactNo = txtamccontactnumber.Text.Trim().ToString();
                //////DateTime AMCValidUpto = DateTime.ParseExact(txtamcvalidupto.Text, "dd/MM/yyyy", new CultureInfo("en-GB"));
                ////if (txtamcvalidupto.Text.Trim().Length != 0)
                ////{
                ////    DateTime AMCValidUpto = Convert.ToDateTime(txtamcvalidupto.Text.Trim());  //, "dd/MM/yyyy", new CultureInfo("en-GB"));
                ////    sm.AMCValidUpto = AMCValidUpto;
                ////}
                ////else
                ////{
                ////    sm.AMCValidUpto = null;
                ////}
                ////sm.AMCReminder = Convert.ToInt32(txtamcreminder.Text.Trim());
                ////sm.StampingReminder = Convert.ToInt32(txtstampingreminder.Text.Trim());
                //////DateTime Stampingdate = DateTime.ParseExact(txtstampingdate.Text, "dd/MM/yyyy", new CultureInfo("en-GB"));
                ////DateTime Stampingdate = Convert.ToDateTime(txtstampingdate.Text.Trim());  //, "dd/MM/yyyy", new CultureInfo("en-GB"));
                ////sm.StampingDate = Stampingdate;
                ////sm.Warrantee = ddlWarrantee.SelectedValue;
                ////sm.Garrantee = ddlGarrantee.SelectedValue;
                ////db.SubmitChanges();
                ////ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Update Successfully');", true);
                ////HtmlMeta meta = new HtmlMeta();
                ////meta.HttpEquiv = "Refresh";
                ////meta.Content = "1.30;url=AddEdit.aspx?id=" + id;
                ////this.Page.Controls.Add(meta);
                try
                {
                    string filename = string.Empty;
                    //if (ddlamctype.SelectedItem.Text.ToLower().Contains("gold"))
                    //{
                    //    filename = "gold";
                    //}
                    //else if (ddlamctype.SelectedItem.Text.ToLower().Contains("silver"))
                    //{
                    //    filename = "silver";
                    //}
                    //else if (ddlamctype.SelectedItem.Text.ToLower().Contains("platinum"))
                    //{
                    //    filename = "platinum";
                    //}
                    //if (ddlWarrantee.SelectedValue.ToLower().Contains("no"))
                    //{
                    //    filename = filename + "_no_warrantee";
                    //}
                    //else if (ddlWarrantee.SelectedValue.ToLower().Contains("yes"))
                    //{
                    //    filename = filename + "_yes_warrantee";
                    //}
                    //if (ddlGarrantee.SelectedValue.ToLower().Contains("no"))
                    //{
                    //    filename = filename + "_no_guarantee";
                    //}
                    //else if (ddlGarrantee.SelectedValue.ToLower().Contains("yes"))
                    //{
                    //    filename = filename + "_yes_guarantee";
                    //}

                    if (sm.AMCType.ToLower().Contains("gold") && Convert.ToDateTime(sm.AMCValidUpto) <= DateTime.Now && sm.Warrantee.ToLower().Contains("no"))
                    {
                        filename = "gold_amc";
                    }
                    else if (sm.AMCType.ToLower().Contains("gold") && Convert.ToDateTime(sm.AMCValidUpto) >= DateTime.Now && sm.Warrantee.ToLower().Contains("no"))
                    {
                        filename = "gold_amc_exp";
                    }
                    else if (sm.AMCType.ToLower().Contains("silver") && Convert.ToDateTime(sm.AMCValidUpto) <= DateTime.Now && sm.Warrantee.ToLower().Contains("no"))
                    {
                        filename = "silver_amc";
                    }
                    else if (sm.AMCType.ToLower().Contains("silver") && Convert.ToDateTime(sm.AMCValidUpto) >= DateTime.Now && sm.Warrantee.ToLower().Contains("no"))
                    {
                        filename = "silver_amc_exp";
                    }
                    else if (ddlamctype.SelectedItem.Text.ToLower().Contains("platinum") && Convert.ToDateTime(sm.AMCValidUpto) <= DateTime.Now && sm.Warrantee.ToLower().Contains("no"))
                    {
                        filename = "platinum_amc";
                    }
                    else if (sm.AMCType.ToLower().Contains("platinum") && Convert.ToDateTime(sm.AMCValidUpto) >= DateTime.Now && sm.Warrantee.ToLower().Contains("no"))
                    {
                        filename = "platinum_amc_exp";
                    }
                    else if (sm.AMCType.ToLower().Contains("none") && sm.Warrantee.ToLower().Contains("yes"))
                    {

                        var varwarrntydate = _repo.Get_WeightMachineMasterById_ss(Session["PlantID"].ToString(), Session["WBID"].ToString());
                        if (Convert.ToDateTime(varwarrntydate.WarrentyUpto) >= DateTime.Now)
                        {
                            filename = "gold_warranty_yes";
                        }
                        else
                        {
                            filename = "gold_warranty_no";
                        }
                    }
                    else if (sm.AMCType.ToLower().Contains("none") && sm.Warrantee.ToLower().Contains("no"))
                    {
                        filename = "gold_warranty_no";

                    }

                    //Session["WBID"] = _WBID;
                    //Session["UserId"] = um.Id;
                    //Session["PlantID"] = _PlantID;

                    filename = filename + ".png";
                    System.IO.File.Delete(Server.MapPath("~/images/login/gold_yes_warrantee_yes_guarantee.png"));
                    System.IO.File.Copy(Server.MapPath("~/images/amc/" + filename), Server.MapPath("~/images/login/gold_yes_warrantee_yes_guarantee.png"));
                }
                catch { }
            }
            //}
        }

        protected void ddlWarrantee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWarrantee.SelectedValue.ToLower().Contains("yes"))
            {
                ddlamctype.Text = "None";
                txtamcvalidupto.Text = string.Empty;
            }
        }

        protected void ddlWarrantee_TextChanged(object sender, EventArgs e)
        {
            if (ddlWarrantee.SelectedValue.ToLower().Contains("yes"))
            {
                ddlamctype.Text = "None";
                txtamcvalidupto.Text = string.Empty;
            }
        }

        protected void ddlamctype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlamctype.SelectedItem.Text.ToLower().Contains("none"))
            {
                ddlWarrantee.Text = "Yes";
            }
            else
            {
                ddlWarrantee.Text = "No";
            }
        }
    }
}