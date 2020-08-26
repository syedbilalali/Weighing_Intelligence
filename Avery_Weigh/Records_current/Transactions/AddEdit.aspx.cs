using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Avery_Weigh.Repository;
using System.Web.UI.WebControls;

namespace Avery_Weigh.Records.Transactions
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
                    tblTransaction trans = _transRepo.getTransactionById(RecordId, 2);
                    if (trans != null)
                    {
                        txtChallanDate.Text = trans.ChallanDate.Value.ToString("dd/MM/yyyy");
                        txtChallanNo.Text = trans.ChallanNo;
                        txtChallanWeight.Text = trans.ChallanWeight;
                        txtCreateDate.Text = trans.CreateDate.Value.ToString();
                        txtGateEntryNo.Text = trans.GateEntryNo;
                        txtRemarks.Text = trans.Remarks;
                        txtTripID.Text = trans.TripId.ToString();
                        txtTruckNo.Text = trans.TruckNo;
                        txtWeighBridgeID.Text = trans.WeighbridgeId;
                        ddlPacking.SelectedValue = trans.PackingCode.Trim();
                        ddlSupplier.SelectedValue = trans.SupplierCode.Trim();
                        ddlTransport.SelectedValue = trans.TransporterCode.Trim();
                        ddlTripType.SelectedValue = trans.TripType.ToString();
                        txtCreatedBy.Text = trans.CreatedBy;
                        txtWeighBridgeID.Text = trans.WeighbridgeId;
                        txtShift.Text = trans.Shift;
                        //IList<tblTransactionWeight> weights = _transRepo.getWeighingByTransactionId(RecordId);
                        //rptList.DataSource = weights;
                        //rptList.DataBind();

                        IList<tblTransactionMaterial> materials = _transRepo.getmaterialsByTransactionId(RecordId);
                        rptMaterials.DataSource = materials;
                        rptMaterials.DataBind();

                    }
                }
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
            ddlSupplier.DataValueField = "Id";
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
            ddlPacking.DataValueField = "Id";
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
    }
}