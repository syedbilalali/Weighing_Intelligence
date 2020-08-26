using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Avery_Weigh
{
    public partial class Database_Backup : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                    Response.Redirect("/login.aspx");
                //bindData();
            }
        }

        private void bindData()
        {
           
                //SHIFTTIME  _shift = db.SHIFTTIMEs.FirstOrDefault(x => x.Id == 1);
                //if (_shift != null)
                //{
                //    ddlShiftId.Text = _shift.NOOFSHIFTS;
                //    dtStartTimeA.Text = _shift.STA;
                //    dtEndTimeA.Text = _shift.EDA;
                //    dtStartTimeB.Text = _shift.STB;
                //    dtEndTimeB.Text = _shift.EDB;
                //    dtStartTimeC.Text = _shift.STC;
                //    dtEndTimeC.Text = _shift.EDC;
                    
                //}
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    SHIFTTIME _shift = db.SHIFTTIMEs.FirstOrDefault(x => x.Id == 1);
            //    if (_shift == null)
            //    {
            //        _shift = new SHIFTTIME();
            //         _shift.NOOFSHIFTS= ddlShiftId.Text.Trim();
            //        _shift.STA=dtStartTimeA.Text.Trim();
            //        _shift.EDA=dtEndTimeA.Text.Trim();
            //        _shift.STB=dtStartTimeB.Text.Trim();
            //        _shift.EDB=dtEndTimeB.Text.Trim();
            //        _shift.STC=dtStartTimeC.Text.Trim();
            //        _shift.EDC=dtEndTimeC.Text.Trim();
                    
            //        db.SHIFTTIMEs.InsertOnSubmit(_shift);
            //        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Shift record added successfully.');", true);
            //    }
            //    else
            //    {
            //        _shift.NOOFSHIFTS = ddlShiftId.Text.Trim();
            //        _shift.STA = dtStartTimeA.Text.Trim();
            //        _shift.EDA = dtEndTimeA.Text.Trim();
            //        _shift.STB = dtStartTimeB.Text.Trim();
            //        _shift.EDB = dtEndTimeB.Text.Trim();
            //        _shift.STC = dtStartTimeC.Text.Trim();
            //        _shift.EDC = dtEndTimeC.Text.Trim();
            //        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Company record updated successfully.');", true);
            //    }
            //    db.SubmitChanges();
            //    bindData();
            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('" + ex.Message.ToString() + "');", true);
            //}
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    SHIFTTIME _shift = db.SHIFTTIMEs.FirstOrDefault(x => x.Id == 1);
            //    if (_shift == null)
            //    {
            //        _shift = new SHIFTTIME();
            //        _shift.NOOFSHIFTS = ddlShiftId.Text.Trim();
            //        _shift.STA = dtStartTimeA.Text.Trim();
            //        _shift.EDA = dtEndTimeA.Text.Trim();
            //        _shift.STB = dtStartTimeB.Text.Trim();
            //        _shift.EDB = dtEndTimeB.Text.Trim();
            //        _shift.STC = dtStartTimeC.Text.Trim();
            //        _shift.EDC = dtEndTimeC.Text.Trim();

            //        db.SHIFTTIMEs.InsertOnSubmit(_shift);
            //        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Shift record added successfully.');", true);
            //    }
            //    else
            //    {
            //        _shift.NOOFSHIFTS = ddlShiftId.Text.Trim();
            //        _shift.STA = dtStartTimeA.Text.Trim();
            //        _shift.EDA = dtEndTimeA.Text.Trim();
            //        _shift.STB = dtStartTimeB.Text.Trim();
            //        _shift.EDB = dtEndTimeB.Text.Trim();
            //        _shift.STC = dtStartTimeC.Text.Trim();
            //        _shift.EDC = dtEndTimeC.Text.Trim();
            //        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.success('Shift record updated successfully.');", true);
            //    }
            //    db.SubmitChanges();
            //    bindData();
            //}
            //catch (Exception ex)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('" + ex.Message.ToString() + "');", true);
            //}
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            ////dialog.ShowDialog();
            //dialog.InitialDirectory = Server.MapPath("~/images/");   // "C:\\";
            //dialog.IsFolderPicker = true;
            //if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            //{
            //    txtFilePath.Text = dialog.FileName;
            //}

            //string[] Dir = upload1.FileName.Split('\\');
            //string Path = "";
            //for (int i = 0; i < Dir.Length; i++)
            //    Path += Dir[i] + "\\";

            //txtFilePath.Text = Path;

            string newconnectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString;

            

            SqlConnectionStringBuilder con = new SqlConnectionStringBuilder(newconnectionstring);
            string myUser = con.UserID;
            string myPass = con.Password;
            string mydatabase = con.InitialCatalog;
            string myServerName = con.DataSource;

           

            string  strFilePath = Server.MapPath("~/DatabaseBackup/SQLbackup.bat");

            if (File.Exists(strFilePath) == false)
            {
                try
                {
                    FileStream fs = File.Create(strFilePath);
                    fs.Close();

                }
                catch
                {
                    //MessageBox.Show("Export file path is not Accessible.Please change your path setting from Text/Excel File Setting", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            StringBuilder strFieldsToWrite = new StringBuilder();
            TextWriter _twTransactionOnTruck = new StreamWriter(strFilePath);

            string varWriteData = string.Empty;
            string varWriteData1 = string.Empty;
            string varWriteData2 = string.Empty;



            varWriteData = "@echo off";
            varWriteData1 = "SQLCMD -U  " + "\"" + myUser + "\"" + " -P " + "\"" + myPass  + "\"" + " -S " + myServerName + " -Q " + "\"" + "EXEC SP_BACKUPDATABASES @databaseName='" + mydatabase + "' , @backupType='F',@backupLocation='" + txtFilePath.Text.Trim() + "'" + "\"";
            //varWriteData1 = "exp " + GlobalVariable.sUsername + "/" + GlobalVariable.sPassword + " file=" + txtpath.Text.Trim().ToString()  + " owner=" + GlobalVariable.sUsername;
            //varWriteData2 = "exp " + GlobalVariable.sUsername + "/" + GlobalVariable.sPassword + "@" + GlobalVariable.sServerName + " file=" + txtpath.Text.Trim().ToString() + " owner=" + GlobalVariable.sUsername;

            _twTransactionOnTruck.WriteLine(varWriteData.ToString());
            varWriteData.Remove(0, varWriteData.Length);

            //if (GlobalVariable.sServerName.Length == 0)
            //{
            _twTransactionOnTruck.WriteLine(varWriteData1.ToString());
            varWriteData1.Remove(0, varWriteData1.Length);
            //}
            //else
            //{
            //    _twTransactionOnTruck.WriteLine(varWriteData2.ToString());
            //    varWriteData2.Remove(0, varWriteData2.Length);
            //}

            _twTransactionOnTruck.Close();
            _twTransactionOnTruck.Dispose();


            try
            {
                var p = new Process();
                p.StartInfo.FileName = strFilePath;  // Application.StartupPath + "\\SQLbackup.bat"; ;  // just for example, you can use yours.
                p.Start();
            }
            catch { }

            

            // Application.DoEvents();

        }
    }
}