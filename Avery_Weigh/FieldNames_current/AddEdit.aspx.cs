using Avery_Weigh.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Avery_Weigh.FieldNames
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        PlantmasterRepository _repo = new PlantmasterRepository();
        DynamicFieldRepository _fieldRepo = new DynamicFieldRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_PlantCode();
                if(Request.QueryString["plantid"]!=null && Request.QueryString["machineid"]!= null)
                {
                    ddlplantid.SelectedValue = Request.QueryString["plantid"].ToString();
                    bindMachineId(Request.QueryString["plantid"].ToString());
                    ddlmachinid.SelectedValue = Request.QueryString["machineid"].ToString();
                    BindRecords();
                }
            }
        }

        protected void Get_PlantCode()
        {
            var data = _repo.Get_PlantList();
            if (data != null)
            {
                ddlplantid.DataTextField = "PlantName";
                ddlplantid.DataValueField = "PlantCode";
                ddlplantid.DataSource = data;
                ddlplantid.DataBind();
                ddlplantid.Items.Insert(0, new ListItem("Select", ""));
            }
        }

        protected void ddlplantid_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindMachineId(ddlplantid.SelectedItem.Value);
        }

        private void bindMachineId(string plantid)
        {
            var data = (from t in db.WeightMachineMasters
                        where t.PlantCodeId == plantid && t.IsDeleted == false
                        select t).ToList();
            if (data.Count > 0)
            {
                ddlmachinid.DataTextField = "MachineId";
                ddlmachinid.DataValueField = "MachineId";
                ddlmachinid.DataSource = data;
                ddlmachinid.DataBind();
                ddlmachinid.Items.Insert(0, new ListItem("Select", ""));
            }
            else
            {
                ddlmachinid.Items.Clear();
                ddlmachinid.Items.Insert(0, new ListItem("Not Available", ""));
            }
        }

        protected void ddlmachinid_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRecords();
        }

        private void BindRecords()
        {
            string plantCode = ddlplantid.SelectedItem.Value;
            string machineId = ddlmachinid.SelectedItem.Value;
            IList<DynamicFieldName> fieldList = _fieldRepo.getFieldsUsingMachineId(plantCode, machineId).ToList();
            if (fieldList.Count() == 0)
            {
                rptList.DataSource = InitializeDynamicField();
                rptList.DataBind();
            }
            else
            {
                rptList.DataSource = fieldList;
                rptList.DataBind();
            }
        }

        private DataTable InitializeDynamicField()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FieldName");
            dt.Columns.Add("FieldValue");
            dt.Columns.Add("IsMandatory1");
            dt.Columns.Add("IsMandatory2");

            DataRow row = dt.NewRow();
            row[0] = "Trip Id";
            row[1] = "Trip Id";
            row[2] = "true";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Weighing Type";
            row[1] = "Weighing Type";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Multi Product";
            row[1] = "Multi Product";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Gate Entry No";
            row[1] = "Gate Entry No";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Truck No";
            row[1] = "Truck No";
            row[2] = "true";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Material";
            row[1] = "Material";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Material Classification";
            row[1] = "Material Classification";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Supplier/customer";
            row[1] = "Supplier/customer";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Transporter";
            row[1] = "Transporter";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Packing";
            row[1] = "Packing";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Packing qty";
            row[1] = "Packing qty";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Challan/Invoice no";
            row[1] = "Challan/Invoice no";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Challan/Invoice  Date";
            row[1] = "Challan/Invoice  Date";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Challan weight";
            row[1] = "Challan weight";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "PO /SO/DO no";
            row[1] = "PO /SO/DO no";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "PO/SO/DO date";
            row[1] = "PO/SO/DO date";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "PO/SO/DO materials";
            row[1] = "PO/SO/DO materials";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Remarks";
            row[1] = "Remarks";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "1st weight";
            row[1] = "1st weight";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "2nd weight";
            row[1] = "2nd weight";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Net weight";
            row[1] = "Net weight";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Gate Pass no";
            row[1] = "Gate Pass no";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Security name";
            row[1] = "Security name";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row[0] = "Security Remarks";
            row[1] = "Security Remarks";
            dt.Rows.Add(row);

            return dt;
        }

        protected void Btnsave_Click(object sender, EventArgs e)
        {
            string plantCode = ddlplantid.SelectedItem.Value;
            string machineId = ddlmachinid.SelectedItem.Value;
            bool isvalidated = true;
            foreach (RepeaterItem item in rptList.Items)
            {
                TextBox txtValue = (TextBox)item.FindControl("txtValue");
                if (string.IsNullOrEmpty(txtValue.Text))
                {
                    isvalidated = false;
                }
            }
            if (isvalidated)
            {
                foreach (RepeaterItem item in rptList.Items)
                {
                    Label lblname = (Label)item.FindControl("lblname");
                    TextBox txtValue = (TextBox)item.FindControl("txtValue");
                    CheckBox chk1 = (CheckBox)item.FindControl("chk1");
                    CheckBox chk2 = (CheckBox)item.FindControl("chk2");

                    _fieldRepo.InsertFieldNames(plantCode, machineId, lblname.Text, txtValue.Text, chk1.Checked, chk2.Checked);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record saved successfully.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('All field values are mandatory.');", true);
            }
        }
    }
}