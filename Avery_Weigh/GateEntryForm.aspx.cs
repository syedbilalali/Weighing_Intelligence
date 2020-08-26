using Avery_Weigh.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Avery_Weigh
{
    public partial class GateEntryForm : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        GateEntryRepository gateRepo = new GateEntryRepository();
        TransactionRepository _transRepo = new TransactionRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int GateNo = gateRepo.GetGateEntryNo();
                txtgatePassNo.Text = GateNo.ToString();
                txtSecurity.Text = Session["UserName"].ToString();
            }
        }

        protected void Add1_Click(object sender, EventArgs e)
        {

        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            if (!_transRepo.checkTruckIsPendingOrNot(txtTruckNo.Text))
            {
                string truckNo = txtTruckNo.Text;
                string securityName = txtSecurity.Text;
                string remarks = txtRemarks.Text;
                tblGateEntryRecord record = new tblGateEntryRecord();
                record.GatePassNo = Convert.ToInt32(txtgatePassNo.Text);
                record.EntryDate = DateTime.Now;
                record.SecurityMarks = txtRemarks.Text;
                record.SecurityName = txtSecurity.Text;
                record.TruckNo = txtTruckNo.Text;
                db.tblGateEntryRecords.InsertOnSubmit(record);
                db.SubmitChanges();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Truck Entry Record added Successfully')", true);
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "2;url=GateEntryForm";
                this.Page.Controls.Add(meta);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Truck No is already in pending records.')", true);
            }
        }
    }
}