using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Avery_Weigh.Repository;

namespace Avery_Weigh.SensorMaster
{
    public partial class Import : System.Web.UI.Page
    {
        SensorMasterRepository repo = new SensorMasterRepository();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.HasFile)
                {
                    if (FileUpload1.PostedFile.ContentType == "application/vnd.ms-excel" || FileUpload1.PostedFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        string filename = Path.Combine(Server.MapPath("~/Uploads"), Guid.NewGuid().ToString() + Path.GetExtension(FileUpload1.PostedFile.FileName));
                        FileUpload1.PostedFile.SaveAs(filename);
                        string ConsString = "";
                        string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
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
                        string message = repo.SaveDataToServer(ds);//Save Excel Data To Server
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.info('" + message + "');", true);
                        HtmlMeta meta = new HtmlMeta
                        {
                            HttpEquiv = "Refresh",
                            Content = "3;url=List.aspx"
                        };
                        this.Page.Controls.Add(meta);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('File Format Is Not Valid!');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Please Choose a File');", true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('"+ex.Message.ToString()+"');",true);
            }
        }
    }
}