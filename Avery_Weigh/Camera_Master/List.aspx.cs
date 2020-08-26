using Avery_Weigh.Repository;
using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Avery_Weigh.Camera_Master
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        CameraMasterRepository _repo = new CameraMasterRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_CameraMasterList();
            }
        }

        protected void Get_CameraMasterList()
        {
            var data = _repo.GetAllCameraMasterList();
            if (data.Count() == 0)
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

        protected void ExportToExcel()
        {
            DataTable dt = _repo.Get_CameraMaster_DataTable();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Camera Master");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=CameraMaster.xlsx");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
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

        protected void BtnDownloadExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
            {
                int id = Convert.ToInt32(RecordId.Value);
                if (_repo.Delete_CameraMaster(id))
                {
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.success('Delete Successfully');",true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "1;url=List.aspx";
                    this.Page.Controls.Add(meta);
                }
            }
        }

        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.FindControl("lblindex") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }
    }
}
