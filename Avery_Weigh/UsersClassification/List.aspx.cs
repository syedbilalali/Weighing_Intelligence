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

namespace Avery_Weigh.UsersClassification
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        UserClassificationRepository _repo = new UserClassificationRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_UserClassification_List();
            }
        }

        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.FindControl("lblid") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }

        //:Delete UserClassification
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
            {
                int id = Convert.ToInt32(RecordId.Value);                
                if (_repo.Delete_UserClassification(id))
                {                    
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.success('Record Deleted Successfully');",true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "0.30;url = List.aspx";
                    this.Page.Controls.Add(meta);
                }
            }
        }

        protected void BtnAddEdit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
                Response.Redirect("AddEdit.aspx?Id=" + RecordId.Value);
            else
                Response.Redirect("AddEdit.aspx");
        }

        protected void BtnExportToExcel_Click(object sender, EventArgs e)
        {
            ExpoToExcel();
        }

        //Export Data To Excel File.
        private void ExpoToExcel()
        {
            DataTable dt = _repo.GetDataTable_UserClassification();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "User Classification");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=UserClassification.xlsx");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        //Get:UserClassification List
        private void Get_UserClassification_List()
        {
            IEnumerable<UserClassification> list = _repo.Get_ActiveUserClassificationList();
            if (list.Count() == 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                dbMain.Style.Add("display", "block");
                tblNone.Visible = false;
            }

            rptList.DataSource = list;
            rptList.DataBind();
        }
    }
}