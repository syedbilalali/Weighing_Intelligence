using Avery_Weigh.Repository;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Avery_Weigh.Supplier
{
    public partial class Import : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SupplierRepository _supplierrepo = new SupplierRepository();
        SystemLogRepository logRepo = new SystemLogRepository();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //Upload Excel File to the server
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (fileupload1.HasFile)
            {
                if (fileupload1.PostedFile.ContentType == "application/vnd.ms-excel" || fileupload1.PostedFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    try
                    {
                        string filename = Path.Combine(Server.MapPath("~/Uploads"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
                        fileupload1.PostedFile.SaveAs(filename);
                        string ConsString = "";
                        string extension = Path.GetExtension(fileupload1.PostedFile.FileName);
                        if (extension.ToLower() == ".xls")
                        {
                            ConsString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties = \"Excel 8.0;HDR=Yes;IMEX=2\""; ;
                        }
                        else if (extension.ToLower() == ".xlsx")
                        {
                            ConsString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties = \"Excel 12.0;HDR=Yes;IMEX=2\""; ;
                        }
                        OleDbConnection con = new OleDbConnection(ConsString);
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        string sheet1 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                        string query = "select *from [" + sheet1 + "]";
                        OleDbCommand cmd = new OleDbCommand(query, con);
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        da.Dispose();
                        con.Close();
                        con.Dispose();
                        string message = _supplierrepo.SaveDataToServer(ds);
                        ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.info('"+message+"');",true);
                        HtmlMeta meta = new HtmlMeta();
                        meta.HttpEquiv = "Refresh";
                        meta.Content = "2;url=List.aspx";
                        this.Page.Controls.Add(meta);
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
                        //#region Add log to table
                        //Model_SystemLog log = new Model_SystemLog();
                        //log.LogDate = DateTime.Now;
                        //log.LogDescription = ex.InnerException.ToString();
                        //log.LogTitle = ex.Message.ToString();
                        //log.URL = HttpContext.Current.Request.Url.AbsoluteUri;
                        //logRepo.SaveSystemLog(log);
                        //#endregion
                        //Response.Write(ex.Message.ToString());
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('File Not Supported.');",true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Please Select a file first');", true);
            }
        }
    }
}