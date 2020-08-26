using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;
using Avery_Weigh.Model;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using System.Data.OleDb;
using System.Configuration;
using System.Web.UI.HtmlControls;

namespace Avery_Weigh.WeightMachinMaster
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        WeightMachinMasterRepository _repo = new WeightMachinMasterRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetWeightMachinMasterList();
            }
        }

        protected void RptrWeightMachinMaster_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.FindControl("lblindex") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }

        //Get:Weight Machine Master List
        private void GetWeightMachinMasterList()
        {
            IEnumerable<WeightMachineMaster> _List = _repo.GetMachineMasters_List();
            if (_List.Count() == 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                tblNone.Visible = false;
                dbMain.Style.Add("display", "block");
            }
            RptrWeightMachinMaster.DataSource = _List;
            RptrWeightMachinMaster.DataBind();
        }

        protected void AddAndEdit_Click(object sender, EventArgs e)
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
        
        //Delete:Weight Machine Master
        protected void Delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
            {
                int id = Convert.ToInt32(RecordId.Value);
                if (_repo.Delete_WeightMachineMaster(id))
                {
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.success('Delete Successfully');",true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "0.30;url=List.aspx";
                    this.Page.Controls.Add(meta);
                }
            }
        }

        //Export data into excel file
        private void ExportToExcel()
        {
            DataTable dt = _repo.GetWeightMachinMasterDataTable();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Weight Machin Master");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=WeightMachinMaster.xlsx");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void ExpToExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
    }
}
