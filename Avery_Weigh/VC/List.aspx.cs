using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Model;
using Avery_Weigh.Repository;
using ClosedXML.Excel;

namespace Avery_Weigh.VC
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        VehicleClassificationRepository _vcrepo = new VehicleClassificationRepository();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Get_VCList();
            }
        }

        //Get:VehicleClassification List
        private void Get_VCList()
        {
            IEnumerable<Model_VehicleClassification> _vc = _vcrepo.Get_Model_VehicleClassificationsList();
            if (_vc.Count() == 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                tblNone.Visible = false;
                dbMain.Style.Add("display", "block");
            }
            rptList.DataSource = _vc;
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
                Response.Redirect("Add.aspx?id=" + RecordId.Value);
            }
            else
            {
                Response.Redirect("Add.aspx");
            }
        }
      
        //Delete:VehicleClassification Record
        protected void Delete_Click(object sender, EventArgs e)
        {
            if (RecordId.Value != "")
            {
                int id = Convert.ToInt32(RecordId.Value);
                if (_vcrepo.Delete_vc(id))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Delete Successfully.');", true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "1;url=List.aspx";
                    this.Page.Controls.Add(meta);
                }
            }
        }

        //Export VehicleClassification data into excel file
        private void ExportToExcel()
        {
            DataTable dt = _vcrepo.Get_VCDataTable();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Vehicle Classification");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=VehicleClassification.xlsx");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void LnkExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
    }
}