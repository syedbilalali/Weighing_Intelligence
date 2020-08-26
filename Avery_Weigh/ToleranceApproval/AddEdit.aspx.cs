using Avery_Weigh.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Avery_Weigh.ToleranceApproval
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        TransactionRepository _transRepo = new TransactionRepository();
        MaterialRepository _materialrepo = new MaterialRepository();
        SupplierRepository _supplierrepo = new SupplierRepository();
        TransporterRepository _transrepo = new TransporterRepository();
        PackingRepository _Packingrepo = new PackingRepository();
        UserMasterRepository umRepo = new UserMasterRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getuserAccess();
                BindPackingDropDown();
                BindTransporterDropdown();
                BindSupplierDropdown();


                if (Request.QueryString["Id"] != null)
                {
                    int RecordId = 0;
                    try
                    {
                        RecordId = Convert.ToInt32(Request.QueryString["Id"].ToString());
                    }
                    catch { }
                    tblTransaction trans = _transRepo.getTransactionById(RecordId, 1);
                    if(trans != null)
                    {
                        txtChallanDate.Text = trans.ChallanDate == null ? "" : trans.ChallanDate.Value.ToString();
                        txtChallanNo.Text = trans.ChallanNo;
                        txtChallanWeight.Text = trans.ChallanWeight;
                        txtCreateDate.Text = trans.CreateDate == null ? "" : trans.CreateDate.Value.ToString();
                        //txtGateEntryNo.Text = trans.GateEntryNo;
                        txtRemarks.Text = trans.Remarks;
                        txtTripID.Text = trans.TripId.ToString();
                        txtTruckNo.Text = trans.TruckNo;
                        txtWeighBridgeID.Text = trans.WeighbridgeId;
                        ddlPacking.SelectedValue = trans.PackingCode == null ? "" : trans.PackingCode.Trim();
                        ddlSupplier.SelectedValue = trans.SupplierCode== null? "": trans.SupplierCode.Trim();
                        ddlTransport.SelectedValue = trans.TransporterCode == null ? "" : trans.TransporterCode.Trim();
                        ddlTripType.SelectedValue = trans.TripType == null ? "" : trans.TripType.ToString();
                        txtCreatedBy.Text = trans.CreatedBy;
                        txtWeighBridgeID.Text = trans.WeighbridgeId;
                        txtShift.Text = trans.Shift;
                        //IList<tblTransactionWeight> weights = _transRepo.getWeighingByTransactionId(RecordId);
                        //rptList.DataSource = weights;
                        //rptList.DataBind();
                        lblFirstWt.Text = trans.FirstWeight.ToString();
                        lblFirstWtDateTime.Text = trans.FirstWtDateTime.ToString();
                        //lblSecondWeight.Text = trans.SecondWeight.ToString();
                        //lblSecondWeightDateTime.Text = trans.SecondWtDateTime.ToString();
                        //lblNetWeight.Text = trans.NetWeight.ToString();

                        IList<tblTransactionMaterial> materials = _transRepo.getmaterialsByTransactionId(RecordId);
                        rptMaterials.DataSource = materials;
                        rptMaterials.DataBind();

                    }
                }
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
                if (uc.PendingRecordDeletion == false)
                {

                    Response.Redirect(Request.UrlReferrer.ToString());
                }

                #endregion
            }


        }
        private void BindMaterialDropDown()
        {
            //ddlmaterial.DataTextField = "Name";
            //ddlmaterial.DataValueField = "MaterialCode";
            //ddlmaterial.DataSource = _materialrepo.GetModelMaterials();
            //ddlmaterial.DataBind();
            //ddlmaterial.Items.Insert(0, new ListItem("Select", ""));

        }
        private void BindMaterialClassificationDropdown()
        {
            //MaterialClassificationRepository repository = new MaterialClassificationRepository();
            //ddlmc.DataTextField = "Name";
            //ddlmc.DataValueField = "Code";
            //ddlmc.DataSource = repository.GetMaterialClassifications();
            //ddlmc.DataBind();
            //ddlmc.Items.Insert(0, new ListItem("Select", ""));
        }
        private void BindSupplierDropdown()
        {

            ddlSupplier.DataTextField = "Name";
            ddlSupplier.DataValueField = "Code";
            ddlSupplier.DataSource = _supplierrepo.Get_SupplierCode();
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select", ""));
        }
        private void BindTransporterDropdown()
        {
            ddlTransport.DataTextField = "Name";
            ddlTransport.DataValueField = "ddCode";
            ddlTransport.DataSource = _transrepo.Get_Transporters();
            ddlTransport.DataBind();
            ddlTransport.Items.Insert(0, new ListItem("Select", ""));
        }
        private void BindPackingDropDown()
        {
            ddlPacking.DataTextField = "Name";
            ddlPacking.DataValueField = "PackingCode";
            ddlPacking.DataSource = _Packingrepo.Get_PackingCode();
            ddlPacking.DataBind();
            ddlPacking.Items.Insert(0, new ListItem("Select", ""));
        }
        protected void Btnsave_Click(object sender, EventArgs e)
        {

        }

        protected void rptMaterials_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.FindControl("lblindex") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }

        protected void rptList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.FindControl("lblindex") is Label l)
            {
                l.Text = e.Item.ItemIndex + 1 + "";
            }
        }

        protected void txtApproved_Click(object sender, EventArgs e)
        {
            int RecordId = 0;
            try
            {
                RecordId = Convert.ToInt32(txtTripID.Text.ToString());
            }
            catch { }
            //tblTransaction _trans = _transRepo.getTransactionByTripId(RecordId);
            tblTransaction _trans1 = db.tblTransactions.FirstOrDefault(x => x.TripId == RecordId);
            _trans1.TARETOLERANCESTATUS = "Yes".ToString();
            //db.tblTransactions.(trans);
            db.SubmitChanges();
        }

        protected void txtReject_Click(object sender, EventArgs e)
        {
            int RecordId = 0;
            try
            {
                RecordId = Convert.ToInt32(txtTripID.Text.ToString());
            }
            catch { }
            //tblTransaction _trans = _transRepo.getTransactionByTripId(RecordId);
            tblTransaction _trans1 = db.tblTransactions.FirstOrDefault(x => x.TripId == RecordId);
            //_trans1.TARETOLERANCESTATUS = "Yes".ToString();
            db.tblTransactions.DeleteOnSubmit(_trans1);
            db.SubmitChanges();
        }


    }
}