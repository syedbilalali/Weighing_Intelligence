using Avery_Weigh.Repository;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Avery_Weigh.Machine
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        MachineParametersRepository _machineRepo = new MachineParametersRepository();
        SystemLogRepository logRepo = new SystemLogRepository();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Get_WorkingParameter_List();
            }
        }

        //Get:MachineWorkingParameter List
        private void Get_WorkingParameter_List()
        {
            IEnumerable<tblMachineWorkingParameter> tblSuppliers = _machineRepo.GetMachineWorkingParameters_List();
            if (tblSuppliers.Count() == 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                tblNone.Visible = false;
                dbMain.Style.Add("display", "block");
            }
            rptList.DataSource = tblSuppliers;
            rptList.DataBind();
        }

        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Label l = e.Item.FindControl("lblid") as Label;
            if (l != null)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            if (RecordId.Value != "")
            {
                Response.Redirect("AddEdit.aspx?id=" + RecordId.Value);
            }
            else
            {
                Response.Redirect("AddEdit.aspx");
            }
        }

        //Delete:MachineWorkingParameter Record By Id
        protected void Delete_Click(object sender, EventArgs e)
        {
            if (RecordId.Value != "")
            {
                int id = Convert.ToInt32(RecordId.Value);
                if (_machineRepo.Delete_WorkingParameter(id))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Delete Successfully');", true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "0.30;url=List.aspx";
                    this.Page.Controls.Add(meta);
                }
            }
        }
      
        //Export:Data into Excel File
        private void ExportToExcel()
        {
            DataTable dt = _machineRepo.GetMachineDataTable();
            if (dt.Columns.Count != 0)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "MachineWorkingParameter");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=MachineWorkingParameters.xlsx");
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

        }

        protected void Export_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }      
    }
}