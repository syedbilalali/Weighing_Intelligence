using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Avery_Weigh.Repository;
using System.Data.OleDb;
using System.Configuration;
using Avery_Weigh.Model;

namespace Avery_Weigh.Records.Transactions
{
    public partial class List : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        TransactionRepository _transRepo = new TransactionRepository();
        UserMasterRepository umRepo = new UserMasterRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getuserAccess();
                GetRecord();
            }
        }

        private void getuserAccess()
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/Login");

            int userid = Convert.ToInt32(User.Identity.Name);
            UserClassification uc = umRepo.GetUserAuthorization(userid);
            if (uc != null)
            {
                #region check Weighing page access
                if (uc.TransactionDeletion == false)
                {

                    Response.Redirect(Request.UrlReferrer.ToString());
                }

                #endregion
            }


        }

        private void GetRecord()
        {

            // get Pending Transaction record from database
            IList<Model_Records> transRecords = _transRepo.GetTransactions(2);  //1 - means Transactions record

            if (transRecords.Count() == 0)
            {
                tblNone.Visible = true;
                dbMain.Style.Add("display", "none");
            }
            else
            {
                tblNone.Visible = false;
                dbMain.Style.Add("display", "block");
            }
            rptList.DataSource = transRecords;
            rptList.DataBind();
        }

        protected void AddEdit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RecordId.Value))
            {
                Response.Redirect("AddEdit.aspx?id=" + RecordId.Value);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Please select record to view.');", true);
            }
        }

        protected void BtnDownloadExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void ExportToExcel()
        {
            //DataTable dt = _repo.GetBarrierMasterDataTable();
            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    wb.Worksheets.Add(dt, "Camera Master");
            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.Charset = "";
            //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.AddHeader("content-disposition", "attachment;filename=PendingRecords.xlsx");
            //    using (MemoryStream memoryStream = new MemoryStream())
            //    {
            //        wb.SaveAs(memoryStream);
            //        memoryStream.WriteTo(Response.OutputStream);
            //        Response.Flush();
            //        Response.End();
            //    }
            //}
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            Delete_Record();
        }

        private void Delete_Record()
        {
            try
            {
                if (!string.IsNullOrEmpty(RecordId.Value))
                {
                    int id = Convert.ToInt32(RecordId.Value);
                    bool IsDeleted = _transRepo.DeleteTransactionRecord(id);
                    if (IsDeleted)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record Deleted Successfully');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Record Not Found!');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Please select record to delete.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('" + ex.Message.ToString() + "');", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.FindControl("lblindex") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }
    }
}