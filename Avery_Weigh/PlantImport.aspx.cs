using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;

namespace Avery_Weigh
{
    public partial class PlantImport : System.Web.UI.Page
    {
        PlantmasterRepository _repo = new PlantmasterRepository();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //:Upload Excel File
        protected void LnkSave_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                if (FileUpload1.PostedFile.ContentType == "application/vnd.ms-excel" || FileUpload1.PostedFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    try
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
                        string message = _repo.SaveDataToServer(ds);  //Pass DataSet to the method which is going to save the data to the sql server.
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.info('" + message.ToString() + "');", true);
                        HtmlMeta meta = new HtmlMeta
                        {
                            HttpEquiv = "Refresh",
                            Content = "3;url=PlantList.aspx"
                        };
                        this.Page.Controls.Add(meta);
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('File format not valid');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('Please Select File');",true);
            }
        }
    }
}