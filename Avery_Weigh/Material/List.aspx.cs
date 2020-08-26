using Avery_Weigh.Repository;
using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Model;
using System.Collections.Generic;

namespace Avery_Weigh.Material
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        MaterialRepository _mat = new MaterialRepository(); 

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GetMaterialList();
            }
        }  

        //Get:MaterialList
        private void GetMaterialList()
        {           
            IEnumerable<Model_Materials> list =  _mat.Get_Model_MaterialList();
            if (list.Count() == 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                tblNone.Visible = false;
                dbMain.Style.Add("display", "block");
            }
            rptList.DataSource = list;
            rptList.DataBind();
        }

        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.FindControl("lblmaterialid") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            if (RecordId.Value != "")
            {
                Response.Redirect("AddEdit.aspx?Id=" + RecordId.Value);
            }
            else
            {
                Response.Redirect("AddEdit.aspx");
            }
        }   

        //Delete:Material Record by Material Id
        protected void Delete_Click(object sender, EventArgs e)
        {
            if (RecordId.Value != "")
            {
                int id = Convert.ToInt32(RecordId.Value);
                if (_mat.Delete_Material(id))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record deleted successfully.');", true);
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "1;url = List.aspx";
                    this.Page.Controls.Add(meta);
                }
            }
        }  

       //Export:Material into excel file
        private void ExportToExcel()
        {
            DataTable dt = _mat.GetMaterialDataTable();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Material");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Material.xlsx");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }  

        protected void ExportExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
       
    }
}