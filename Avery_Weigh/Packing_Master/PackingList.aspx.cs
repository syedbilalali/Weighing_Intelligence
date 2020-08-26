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

namespace Avery_Weigh.Packing_Master
{
    public partial class Packing_Master : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        PackingRepository _pack = new PackingRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetPackingMasterList();
            }
        }

        //: return packing master data list
        private void GetPackingMasterList()
        {
            IEnumerable<PackingMaster> packingsList = _pack.GetPackingMasters_List();
            if (packingsList.Count() == 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                tblNone.Visible = false;
                dbMain.Style.Add("display", "block");
            }
            rptList.DataSource = packingsList;
            rptList.DataBind();
        }

        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.FindControl("lblindex") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }

        protected void id1_Click(object sender, EventArgs e)
        {
            if(RecordId.Value!= "")
            {
                Response.Redirect("AddEdit.aspx?id=" + RecordId.Value);
            }
            else
            {
                Response.Redirect("AddEdit.aspx");
            }
        }

        //Delete:PackingMaster Record By Id
        protected void Delete_Click(object sender, EventArgs e)
        {
            if (RecordId.Value != "")
            {
                int id = Convert.ToInt32(RecordId.Value);
                if (_pack.Delete_PackingMaster(id))
                {
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.success('Delete Successfully');",true);
                    HtmlMeta meta = new HtmlMeta
                    {
                        HttpEquiv = "Refresh",
                        Content = "0.30;url=PackingList.aspx"
                    };
                    this.Page.Controls.Add(meta);
                }
            }
        }
              
        //Export: Packing Master Data into Excel File
        private void ExportToExcel()
        {
            DataTable dt =  _pack.GetPackingDataTable();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Packing Master");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=PackingMaster.xlsx");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        
        protected void Export_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
    }
}