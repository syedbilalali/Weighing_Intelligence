using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;
using ClosedXML.Excel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web.UI.HtmlControls;

namespace Avery_Weigh.Material_Classification
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        MaterialClassificationRepository _matclarepo = new MaterialClassificationRepository();
        

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GetMaterialClassificationList();
            }
        }

        //Get:Material Classification List
        private void GetMaterialClassificationList()
        {
            IEnumerable<MaterialClassification> materialslist = _matclarepo.Get_MaterialClassification_List();
            if (materialslist.Count() == 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                tblNone.Visible = false;
                dbMain.Style.Add("display", "block");
            }
            rptList.DataSource = materialslist;
            rptList.DataBind();
        }

        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Label label = e.Item.FindControl("lblindex") as Label;
            if (label != null)
            {
                label.Text = e.Item.ItemIndex + 1 + "";
            }
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
            {
                Response.Redirect("AddEdit.aspx?Id=" + RecordId.Value);
            }
            else
            {
                Response.Redirect("AddEdit.aspx");
            }
        }

        //Delete:Record by Id
        protected void Delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
            {
                int id = Convert.ToInt32(RecordId.Value);
                if (_matclarepo.Delete_MaterialClassification(id))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Deleted Successfully');", true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "0.30;url=List.aspx";
                    this.Page.Controls.Add(meta);
                }
            }
        }
        
        //Export:Material Classification Data Into Excel File
        private void ExportToExcel()
        {
            DataTable dt = _matclarepo.Get_MaterialClassification_DataTable();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Material Classification");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=materialclassification.xlsx");
                using(MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }  

        protected void btnImport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }      
    }

}