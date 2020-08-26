using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;
using ClosedXML.Excel;

namespace Avery_Weigh.Alpha_DisplayMaster
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        AlphaDisplayRepository _repo = new AlphaDisplayRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAlphaDisplayDataList();
            }
        }

        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.FindControl("lblindex") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }

        //Delete:AlphaDisplayMaster Record by Id
        protected void Delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
            {
                int id = Convert.ToInt32(RecordId.Value);
                if (_repo.Delete_AlphaDisplayById(id))
                {
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.success('Delete Successfully');",true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "0.30;url=List.aspx";
                    this.Page.Controls.Add(meta);
                }
            }
        }

        //DownLoad:Excel File
        protected void BtnDownloadExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        protected void AddEdit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
            {
                Response.Redirect("AddEdit.aspx?id=" + RecordId.Value);
            }
            else
            {
                Response.Redirect("AddEdit.aspx");
            }
        }

        //Get:AlphaDisplayMaster List
        private void GetAlphaDisplayDataList()
        {
            var data = _repo.GetAllAlphaDisplayMasterList();
            if(data.Count() == 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                tblNone.Visible = false;
                dbMain.Style.Add("display", "block");
            }
            rptList.DataSource = data;
            rptList.DataBind();
        }
       
        //Export:Data into Excel File
        private void ExportToExcel()
        {
            DataTable dt = _repo.GetAlphaDisplayMasterDataTable();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Alpha Display Master");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=AlphaDisplayMaster.xlsx");
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
}