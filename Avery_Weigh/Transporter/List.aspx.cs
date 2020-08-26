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
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;
using ClosedXML.Excel;
using Avery_Weigh.Model;
using System.Web.UI.HtmlControls;

namespace Avery_Weigh.Transporter
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        TransporterRepository _trans = new TransporterRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_TransporterList();
            }
        }

        //Get:Transporter List
        private void Get_TransporterList()
        {
             IEnumerable<Model_Transporter> _Transporters =  _trans.GetTransporters_List();
            if (_Transporters.Count() == 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                tblNone.Visible = false;
                dbMain.Style.Add("display", "block");
            }
            rptList.DataSource = _Transporters;
            rptList.DataBind();
        }

        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.FindControl("lblid") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }
          
       
        //Delete:Transporter By Id
        protected void Delete_Click(object sender, EventArgs e)
        {
            if (RecordId.Value!="")
            {
                int id = Convert.ToInt32(RecordId.Value);
                if (_trans.Delete_TransporterById(id))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record deleted successfully');", true);
                    HtmlMeta meta = new HtmlMeta
                    {
                        HttpEquiv = "Refresh",
                        Content = "1;url=List.aspx"
                    };
                    this.Page.Controls.Add(meta);
                }
            }
        }  

        //Export:Transporter Data To Excel
        private void ExportToExcel()
        {
            DataTable dt = _trans.GetTransporterDataTable();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Transporter");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Transporter.xlsx");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }             

        protected void DownloadExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        protected void AddEdit_Click(object sender, EventArgs e)
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
    }
}