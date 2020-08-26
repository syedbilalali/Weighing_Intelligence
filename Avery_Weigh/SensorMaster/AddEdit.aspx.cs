using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;

namespace Avery_Weigh.SensorMaster
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        PlantmasterRepository plantrepo = new PlantmasterRepository();
        WeightMachinMasterRepository wmrepo = new WeightMachinMasterRepository();
        SensorMasterRepository sensrepo = new SensorMasterRepository();
        RegexRepository regex = new RegexRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_PlantCode();
                Get_SensorForEdit();
            }
        }


        protected void Btnsave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Update_Sensor();
            }
            else
            {
                Add_Sensor();
            }
        }

        //Get:First Record From Sensor Master
        protected void First_Record_Click(object sender, EventArgs e)
        {

        }

        //Get:Previos Record From Sensor Master
        protected void Previous_Record_Click(object sender, EventArgs e)
        {

        }

        //Get:Next Record From Sensor Master
        protected void Next_Record_Click(object sender, EventArgs e)
        {

        }

        //Get:Last Record From Sensor Master
        protected void Last_Record_Click(object sender, EventArgs e)
        {

        }

        //Get:Plant Code From Plant Master
        public void Get_PlantCode()
        {
            var data = plantrepo.Get_PlantCodeId();
            ddlplantcode.DataTextField = "PlantName";
            ddlplantcode.DataValueField = "PlantCode";
            ddlplantcode.DataSource = data;
            ddlplantcode.DataBind();
            ddlplantcode.Items.Insert(0, new ListItem("Select", ""));
        }

        //Get Machine Id from Weight Machine Master by Plantcode
        public void Get_MachineId_By_PlantCode(string code)
        {
            var data = wmrepo.Get_MachineIdBy_PlantCode(code);
            ddlmachineid.DataTextField = "MachineId";
            ddlmachineid.DataValueField = "MachineId";
            ddlmachineid.DataSource = data;
            ddlmachineid.DataBind();
        }

        protected void ddlplantcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlplantcode.SelectedValue))
            {
                var data = wmrepo.Get_MachineIdBy_PlantCode(ddlplantcode.SelectedValue);
                if (data.Count() > 0)
                {
                    ddlmachineid.DataTextField = "MachineId";
                    ddlmachineid.DataValueField = "MachineId";
                    ddlmachineid.DataSource = data;
                    ddlmachineid.DataBind();
                    ddlmachineid.Items.Insert(0, new ListItem("Select Machine Id", ""));
                }
                else
                {
                    ddlmachineid.Items.Clear();
                    ddlmachineid.Items.Insert(0, new ListItem("Not Available", ""));
                }
            }
        }

        //Add:New Sensor 
        protected void Add_Sensor()
        {
            try
            {
                var data = sensrepo.Get_Sensor_List().FirstOrDefault(x => x.PlantCode == ddlplantcode.SelectedValue && x.MachineId == ddlmachineid.SelectedValue && x.IsDeleted == false);
                var data2 = sensrepo.Get_Sensor_List().FirstOrDefault(x => x.PlantCode == ddlplantcode.SelectedValue && x.SensorIP == txtip.Text.Trim() && x.IsDeleted == false);
                if (data != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Machine Configuration Exist! Please Try Again')", true);
                }
                else if (data2 != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Sensor IP Exist');", true);
                }
                else
                {
                    tblSensorMaster sensor = new tblSensorMaster();
                    sensor.PlantCode = ddlplantcode.SelectedValue.ToString();
                    sensor.MachineId = ddlmachineid.SelectedValue.ToString();
                    sensor.SensorIdentification = txtidentification.Text.Trim();
                    if (regex.CheckIPAddress(txtip.Text.Trim()))
                    {
                        sensor.SensorIP = txtip.Text.Trim();
                        sensor.SensorPort = txtport.Text.Trim();
                        sensor.IsDeleted = false;
                        if (sensrepo.Add_Sensor(sensor))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Save Successfully');", true);
                            HtmlMeta meta = new HtmlMeta();
                            meta.HttpEquiv = "Refresh";
                            meta.Content = "0.30;url=AddEdit.aspx";
                            this.Page.Controls.Add(meta);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Invalid IP Address');", true);
                    }                                    
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
            }
        }

        //Update:Sensor Record by Id
        protected void Update_Sensor()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                try
                {
                    int id = Convert.ToInt32(Request.QueryString["Id"]);
                    var data = sensrepo.Get_Sensor_List().FirstOrDefault(x => x.PlantCode == ddlplantcode.SelectedValue && x.MachineId == ddlmachineid.SelectedValue && x.Id != id && x.IsDeleted == false);
                    var data2 = sensrepo.Get_Sensor_List().FirstOrDefault(x => x.PlantCode == ddlplantcode.SelectedValue && x.Id != id && x.IsDeleted == false);
                    if (data != null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Machine Configuration Already Exist! Please try again');", true);
                    }
                    else if (data2 != null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Sensor IP Already Exist! Plese try again');", true);
                    }
                    else
                    {
                        tblSensorMaster sensor = db.tblSensorMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
                        sensor.PlantCode = ddlplantcode.SelectedValue;
                        sensor.MachineId = ddlmachineid.SelectedValue;
                        sensor.SensorIdentification = txtidentification.Text.Trim().ToString();
                        if (regex.CheckIPAddress(txtip.Text.Trim()))
                        {
                            sensor.SensorIP = txtip.Text.Trim().ToString();
                            sensor.SensorPort = txtport.Text.Trim().ToString();
                            db.SubmitChanges();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Update Successfully');", true);
                            HtmlMeta meta = new HtmlMeta();
                            meta.HttpEquiv = "Refresh";
                            meta.Content = "1;url = AddEdit.aspx?id=" + id;
                            this.Page.Controls.Add(meta);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Invalid IP Address');", true);
                        }
                       
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex.Message.ToString() + "');", true);
                }
            }
        }

        //Get:Sensor Master for Edit
        protected void Get_SensorForEdit()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                try
                {
                    int id = Convert.ToInt32(Request.QueryString["Id"]);
                    tblSensorMaster sen = sensrepo.Get_Sensor_by_Id(id);
                    if (sen != null)
                    {
                        ddlplantcode.SelectedValue = sen.PlantCode;
                        Get_MachineId_By_PlantCode(ddlplantcode.SelectedValue);
                        ddlmachineid.SelectedValue = sen.MachineId;
                        txtidentification.Text = sen.SensorIdentification;
                        txtip.Text = sen.SensorIP;
                        txtport.Text = sen.SensorPort;
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex.Message.ToString() + "');", true);
                }
            }
        }
    }
}