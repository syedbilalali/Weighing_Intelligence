using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;

namespace Avery_Weigh
{
    public partial class PlantInfo : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        PlantmasterRepository repo = new PlantmasterRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_PlantInfo_ForEdit();
            }
        }      
        protected void Save_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Update_PlantInfo();
            }
            else
            {
                Add_PlantInfo();
            }
        }

        //:Add New Plant Master Record
        private void Add_PlantInfo()
        {
            PlantMaster _master = repo.Get_PlantMaster_By_PlantCode(txtPlantCOde.Text.Trim());
            if (_master != null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "toastr", "toastr.error('Same Plant code is already exists.');", true);
            }
            else
            {
                _master = new PlantMaster();
                _master.ContactEmail = txtContactEmail.Text;
                _master.ContactMobile = txtContactMobile.Text;
                _master.Designation = txtDesignation.Text;
                try
                {
                    _master.NoOfMachine = Convert.ToInt32(txtMachineCount.Text);
                }
                catch { _master.NoOfMachine = 0; }
                _master.PlantAddress1 = txtAddress1.Text;
                _master.PlantAddress2 = txtAddress2.Text;
                _master.PlantCode = txtPlantCOde.Text;
                _master.PlantContactPerson = txtContactPerson.Text;
                _master.PlantName = txtPlantName.Text;
                _master.IsDeleted = false;
                if(repo.Add_PlantMaster(_master))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Plant info saved successfully.');", true);
                    HtmlMeta meta = new HtmlMeta
                    {
                        HttpEquiv = "Refresh",
                        Content = "1;url=PlantInfo.aspx"
                    };
                    this.Page.Controls.Add(meta);
                }
            }
        }

        //Update:Plant Master Record
        private void Update_PlantInfo()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                int Id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                //PlantMaster _master = repo.Get_PlantMaster_by_Id(Id);
                PlantMaster _master = db.PlantMasters.FirstOrDefault(x => x.Id == Id && x.IsDeleted == false);
                if (_master != null)
                {
                    PlantMaster _plant = repo.Get_PlantList().Where(x => x.PlantCode == txtPlantCOde.Text && x.Id != Id && x.IsDeleted == false).FirstOrDefault();
                    if(_plant != null)
                    {
                        ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('Same Plant Code Exist! Please Try Again.');",true);
                    }
                    else
                    {                      
                        _master.ContactEmail = txtContactEmail.Text;
                        _master.ContactMobile = txtContactMobile.Text;
                        _master.Designation = txtDesignation.Text;
                        _master.NoOfMachine = Convert.ToInt32(txtMachineCount.Text);
                        _master.PlantAddress1 = txtAddress1.Text;
                        _master.PlantAddress2 = txtAddress2.Text;
                        _master.PlantCode = txtPlantCOde.Text;
                        _master.PlantContactPerson = txtContactPerson.Text;
                        _master.PlantName = txtPlantName.Text.Trim().ToString();
                        db.SubmitChanges();
                        ClientScript.RegisterStartupScript(this.GetType(), "toastr", "toastr.success('Plant info updated successfully.');", true);
                        HtmlMeta meta = new HtmlMeta
                        {
                            HttpEquiv = "Refresh",
                            Content = "1;url=PlantInfo.aspx?id="+Id
                        };
                        this.Page.Controls.Add(meta);
                    }                  
                }               
            }           
        }

        //:Get Plant Master For Update
        private void Get_PlantInfo_ForEdit()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                divoptions.Style.Add("display", "block");
                txtPlantCOde.Enabled = false;
                int id = Convert.ToInt32(Request.QueryString["Id"]);
                PlantMaster _pm = repo.Get_PlantMaster_by_Id(id);
                if (_pm != null)
                {
                    txtPlantCOde.Text = _pm.PlantCode.ToString();
                    txtPlantName.Text = _pm.PlantName.ToString();
                    txtDesignation.Text = _pm.Designation.ToString();
                    txtContactPerson.Text = _pm.PlantContactPerson.ToString();
                    txtContactMobile.Text = _pm.ContactMobile.ToString();
                    txtContactEmail.Text = _pm.ContactEmail.ToString();
                    txtAddress1.Text = _pm.PlantAddress1.ToString();
                    txtAddress2.Text = _pm.PlantAddress2.ToString();
                    txtMachineCount.Text = _pm.NoOfMachine.ToString();                   
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('Plant code not found.please try again');",true);
                }
            }
        }
      
        //:Get First Record From Plant Master
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = repo.Get_PlantList().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("PlantInfo.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //:Get Previous Record From Plant Master
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            PlantMaster next = null;
            try
            {
                next = repo.Get_PlantList().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("PlantInfo.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //:Get Next Record 
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = repo.Get_PlantList().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("PlantInfo.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //:Get Last Record from plant master
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = repo.Get_PlantList().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("PlantInfo.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

       
    }
}