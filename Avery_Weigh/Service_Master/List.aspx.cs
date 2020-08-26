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

namespace Avery_Weigh.Service_Master
{
    public partial class List : System.Web.UI.Page
    {
        ServiceMasterRepository smrepo = new ServiceMasterRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_ServiceMasterList();
            }
        }

        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.FindControl("lblindex") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }

        protected void Get_ServiceMasterList()
        {
            var data = smrepo.Get_ServiceMasterList();
            if(data.Count() <= 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                tblNone.Visible = false;
                dbMain.Style.Add("display", "block");
                rptList.DataSource = data;
                rptList.DataBind();
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
            {
                int id = Convert.ToInt32(RecordId.Value);
                if (smrepo.Delete_ServiceMaster(id))
                {
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.success('Delete Successfully');",true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "1;url=List.aspx";
                    this.Page.Controls.Add(meta);
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

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = smrepo.Get_ServiceMaster_DataTable();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "ServiceMaster");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=ServiceMaster.xlsx");
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