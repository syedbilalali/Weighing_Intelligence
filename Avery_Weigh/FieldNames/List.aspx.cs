using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Model;
using Avery_Weigh.Repository;
using ClosedXML.Excel;


namespace Avery_Weigh.FieldNames
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        DynamicFieldRepository _dynamicRepo = new DynamicFieldRepository();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindDynamicField();
            }
        }
        private void BindDynamicField()
        {
            IEnumerable<DynamicFieldModel> tblList = _dynamicRepo.GetDynamicFieldsList().ToList();
            if (tblList.Count() == 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                tblNone.Visible = false;
                dbMain.Style.Add("display", "block");
            }
            rptList.DataSource = tblList;
            rptList.DataBind();
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            if (RecordId.Value != "")
            {
                Response.Redirect("AddEdit.aspx?plantId=" + RecordId.Value.Split('#')[0]+"&machineId="+ RecordId.Value.Split('#')[1]);
            }
            else
            {
                Response.Redirect("AddEdit.aspx");
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            if (RecordId.Value != "")
            {
                string strPlantCode= RecordId.Value.Split('#')[0].ToString();
                string strMachineId=  RecordId.Value.Split('#')[1].ToString();
                if (_dynamicRepo.Delete_DynamicFieldsData(strPlantCode, strMachineId))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record deleted successfully.');", true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "1;url = List.aspx";
                    this.Page.Controls.Add(meta);
                }
            }
        }
    }
}