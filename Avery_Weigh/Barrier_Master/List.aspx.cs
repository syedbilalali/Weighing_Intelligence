using Avery_Weigh.Repository;
using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Avery_Weigh.Barrier_Master
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        BarrierMasterRepository _repo = new BarrierMasterRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_BarrierList();
            }
        }

        //Get:Barrier Master List
        private void Get_BarrierList()
        {
            var data = _repo.GetAllBarrierMasterList();
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
       
        //Delete:BarrierMaster record by Id
        protected void Delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
            {
                int id = Convert.ToInt32(RecordId.Value);
                if (_repo.Delete_BarrierMaster(id))
                {
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.success('Delete Successfully');",true);
                }
            }
        }

        //Export:Data Into Excel File
        private void ExportToExcel()
        {
            DataTable dt = _repo.GetBarrierMasterDataTable();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Barrier Master");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=BarrierMaster.xlsx");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        //Download:BarrierMaster data in Excel Format
        protected void BtnDownloadExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
     
        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.FindControl("lblindex") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 +"";
            }
        }
    }
}
