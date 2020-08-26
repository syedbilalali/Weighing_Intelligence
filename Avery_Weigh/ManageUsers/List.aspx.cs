using Avery_Weigh.Model;
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

namespace Avery_Weigh.ManageUsers
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        UserMasterRepository _repo = new UserMasterRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_UserMasterList();
            }
        }

        //Get:Active user list
        protected void Get_UserMasterList()
        {
            IEnumerable<Model_UserMasters> list = _repo.Get_Users(); // return all active user list
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

        protected void id1_Click(object sender, EventArgs e)
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

        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.FindControl("lblindex") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }

        //Delete:User Master by Id
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
            {
                try
                {
                    int id = Convert.ToInt32(RecordId.Value);
                    if (_repo.Delete_UserMasterById(id))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Deleted Successfully');", true);
                        HtmlMeta meta = new HtmlMeta();
                        meta.HttpEquiv = "Refresh";
                        meta.Content = "0.30;url=List.aspx";
                        this.Page.Controls.Add(meta);
                    }
                }
                catch(Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('"+ex.Message.ToString()+"');",true);
                }
            }
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        // Export Data Into Excel File
        private void ExportToExcel()
        {
            DataTable dt = _repo.GetDataTable_UserMaster();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "UserMaster");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=UserMaster.xlsx");
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