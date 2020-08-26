using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;
using ClosedXML.Excel;

namespace Avery_Weigh
{
    public partial class PlantList : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        PlantmasterRepository _repo = new PlantmasterRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_PlantMasterList();
            }
        }
        //:Get PlantMaster DataList
        private void Get_PlantMasterList()
        {
            IEnumerable<PlantMaster> List = _repo.Get_PlantList();
            if (List.Count() == 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                dbMain.Style.Add("display", "block");
                tblNone.Visible = false;
            }

            rptList.DataSource = List;
            rptList.DataBind();
        }

        protected void LnkAddEdit_Click1(object sender, EventArgs e)
        {
            if (RecordId.Value != "")
            {
                Response.Redirect("PlantInfo.aspx?id=" + RecordId.Value);
            }
            else
            {
                Response.Redirect("PlantInfo.aspx");
            }
        }

        //Delete:PlantMaster by Plant Id
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
            {
                int id = Convert.ToInt32(RecordId.Value);
                if (_repo.Delete_PlantById(id))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Deleted Successfully');", true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "2;url=PlantList.aspx";
                    this.Page.Controls.Add(meta);
                }

            }
        }

        //:Export Data to Excel File
        protected void ExportToExcel()
        {
            DataTable dt = _repo.GetDataTable_PlantMaster(); //:Get PlantMaster Datatable
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "PlantMaster");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=PlantMaster.xlsx");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void BtnLinkExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label l = e.Item.FindControl("lblid") as Label;
            if (l != null)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }
    }
}