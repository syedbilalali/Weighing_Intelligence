using Avery_Weigh.Model;
using Avery_Weigh.Repository;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Advantech.Adam;
//using System.Linq;
using DIOWebService;
using System.Threading;
using NetSDK;
using PlaySDK;
using System.Drawing;
using System.Linq;
using System.Globalization;
//using System.Drawing;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using System.Web;

namespace Avery_Weigh
{
    public partial class Manual_Weighment : System.Web.UI.Page
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DataClasses1DataContext db = new DataClasses1DataContext();
        MaterialRepository _materialrepo = new MaterialRepository();
        SupplierRepository _supplierrepo = new SupplierRepository();
        TransporterRepository _transrepo = new TransporterRepository();
        PackingRepository _Packingrepo = new PackingRepository();
        TransactionRepository _transactionRepo = new TransactionRepository();
        UserMasterRepository umRepo = new UserMasterRepository();
        DataClasses1DataContext db1 = new DataClasses1DataContext();
        SystemLogRepository logRepo = new SystemLogRepository();
        private Socket s3;

       
        bool Flag_FirstWt_Records = false;
        bool Flag_SecondWt_Records = false;

        string checkdisplayMessage = string.Empty;

        public string mySrc2;
        public string mySrc1;
        public string mySrc3;

      
        protected void Page_Load(object sender, EventArgs e)
        {
            txtTripId.Attributes.Add("onKeyDown", "doCheck();");
            checkSessionExpired();
            if (!IsPostBack)
            {
                    bindFormLabels();
                
                    getuserAccess();
                    timer1.Interval = 100;
                    timer1.Enabled = true;
                    lblUnit.Text = Session["WEIGHINGUNIT"].ToString();
                    lblUnit1.Text = Session["WEIGHINGUNIT"].ToString();
                    lblUnit2.Text = Session["WEIGHINGUNIT"].ToString();
                    lblUnit3.Text = Session["WEIGHINGUNIT"].ToString();
                    CheckAndBindTripId();
                    BindMaterialDropDown();
                    BindMaterialClassificationDropdown();
                    BindSupplierDropdown();
                    BindTransporterDropdown();
                    BindPackingDropDown();
                    Session["blZeroRangeVisibleWB1"] = "1";
                    Session["blWeightInterlock"] = "1";



                //try
                //    {
                //        UserControl UC;
                //        System.Web.UI.WebControls.Image imgUC;
                //        UC = (UserControl)Page.FindControl("Manual_Weighment");
                //        imgUC = (System.Web.UI.WebControls.Image)UC.FindControl("Image5");
                //        imgUC.ImageUrl = "~/images/type1/configure_disable.png";
                //    }
                //    catch { }

                //FillPendingTruck();


            }

           
            



        }


        private void checkSessionExpired()
        {
            if (Context.Session != null)
            {
                if (Session.IsNewSession)
                {
                    string cookieHeader = Request.Headers["Cookie"];
                    if ((null != cookieHeader) && (cookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        Response.Redirect("/Login");
                    }
                }
            }
        }

        private void FillPendingTruck()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                   
                    using (SqlCommand cmd = new SqlCommand(@"SELECT TOP 15 ROW_NUMBER() OVER (ORDER BY TRIPID) AS SINO,TRUCKNO,FirstWeight FROM [tblTransactions] where TransactionStatus=1", con))
                    {
                        cmd.CommandType = CommandType.Text;

                        //cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                        //cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                        //cmd.Parameters.AddWithValue("@Option", "DateWise");

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {

                                da.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    GridView1.DataSource = dt;
                                    GridView1.DataBind();

                                }
                                else
                                {
                                    GridView1.DataSource = null;
                                    GridView1.DataBind();
                                }

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex + "');", true);
            }

        }

        private void bindFormLabels()
        {
            try

            {
                IList<DynamicFieldName> model = _transrepo.GetFieldNameByMachine(Session["WBID"].ToString(), Session["PlantID"].ToString());
                if (model.Count > 0)
                {
                    foreach (DynamicFieldName names in model)
                    {
                        switch (names.FieldName)
                        {
                            case "Trip Id":
                                lblTripId.InnerText = names.FieldValue;
                               
                                break;

                            case "Weighing Type":
                                lblWeighingType.InnerText = names.FieldValue;
                                break;
                            case "Multi Product":
                                lblMultiProduct.InnerText = names.FieldValue;
                                if (names.IsRequired == true)
                                {
                                    lblMultiProduct.Visible = false;
                                    checkmultiproduct.Visible = false;
                                }
                                break;
                            case "Truck No":
                                lbltruckno.InnerText = names.FieldValue;
                                break;
                            case "Material":
                                lblMaterial.InnerText = names.FieldValue;
                                if (names.IsRequired == true)
                                {
                                    lblMaterial.Visible = false;
                                    ddlmaterial.Visible = false;
                                }
                                break;
                            case "Material Classification":
                                lblMC.InnerText = names.FieldValue;
                                if (names.IsRequired == true)
                                {
                                    lblMC.Visible = false;
                                    ddlmc.Visible = false;
                                }
                                break;
                            case "Supplier/customer":
                                lblsupplier.InnerText = names.FieldValue;
                                if (names.IsRequired == true)
                                {
                                    lblsupplier.Visible = false;
                                    ddlsupplier.Visible = false;
                                }
                                break;
                            case "Transporter":
                                lblTransporter.InnerText = names.FieldValue;
                                if (names.IsRequired == true)
                                {
                                    lblTransporter.Visible = false;
                                    ddltransporter.Visible = false;
                                }
                                break;
                            case "Packing":
                                lblPacking.InnerText = names.FieldValue;
                                if (names.IsRequired == true)
                                {
                                    lblPacking.Visible = false;
                                    ddlpacking.Visible = false;
                                }
                                break;
                            case "Packing qty":
                                lblPackingQty.InnerText = names.FieldValue;
                                if (names.IsRequired == true)
                                {
                                    lblPackingQty.Visible = false;
                                    txtpackingqty.Visible = false;
                                }
                                break;
                            case "Challan/Invoice no":
                                lblChallanNo.InnerText = names.FieldValue;
                                if (names.IsRequired == true)
                                {
                                    lblChallanNo.Visible = false;
                                    txtInvoiceNo.Visible = false;
                                }
                                break;
                            case "Challan weight":
                                lblChallanwt.InnerText = names.FieldValue;
                                if (names.IsRequired == true)
                                {
                                    lblChallanwt.Visible = false;
                                    txtChallanWeight.Visible = false;
                                }
                                break;
                            case "PO /SO/DO no":
                                lblPOSODONo.InnerText = names.FieldValue;
                                if (names.IsRequired == true)
                                {
                                    lblPOSODONo.Visible = false;
                                    txtPONo.Visible = false;
                                    txtPODate.Visible = false;
                                    lblPODate.Visible = false;
                                    
                                }
                                break;
                            case "Remarks":
                                lblRemrks.InnerText = names.FieldValue;
                                break;
                            case "1st weight":
                                lblFirstWt.InnerText = names.FieldValue;
                                break;
                            case "2nd weight":
                                lbl2ndWt.InnerText = names.FieldValue;
                                break;
                            case "Net weight":
                                lblNetWt.InnerText = names.FieldValue;
                                break;
                            case "Security name":
                                //lblTripId.InnerText = names.FieldValue;
                                break;
                            case "Security Remarks":
                                //lblTripId.InnerText = names.FieldValue;
                                break;
                        }
                    }
                }
            }
            catch { }
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
                if (uc.Weighment == false)
                {
                    WeighMenu.Style.Add("display", "none");
                    //else if (uc. == false)
                    //     ManageMasters.Style.Add("display", "none");
                    Response.Redirect(Request.UrlReferrer.ToString());
                }
                else
                    WeighMenu.Style.Add("display", "block");
                #endregion
                #region check Configuration page access
                if (uc.Weighment == false)
                    configurationMenu.Style.Add("display", "none");
                else
                    configurationMenu.Style.Add("display", "block");
                #endregion
                #region check Configuration page access
                if (uc.Weighment == false)
                    configurationMenu.Style.Add("display", "none");
                else
                    configurationMenu.Style.Add("display", "block");
                #endregion
            }

           
        }

        private void CheckAndBindTripId()
        {
            txtTripId.Text = _transactionRepo.GetTripId().ToString();
        }

        public tblMachineWorkingParameter getLoggedInUserWeigh()
        {
                string machineId = string.Empty;
                try
                {
                    if (Session["WBID"] !=null)
                    machineId = Session["WBID"].ToString();
                }
                catch { }
                tblMachineWorkingParameter mwparameter = _transactionRepo.getMachineWorkingParameters(machineId);
                return mwparameter;
            
        }

        
        public string GetWeightFromTCPIP_SS()
        {
            string output = "No weight";
            // for inicator ip address and port no
            string strIndicatorIPAddress = string.Empty;
            string strIndicatorPortNo = string.Empty;
            tblMachineWorkingParameter tbl = getLoggedInUserWeigh();
            try
            {
                if (tbl != null)
                {
                    strIndicatorPortNo = tbl.PortNo;
                    strIndicatorIPAddress = tbl.IPPort;
                }

            }
            catch { }

            s3 = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp);
           
                IPAddress hostadd = IPAddress.Parse(strIndicatorIPAddress);
           
                IPEndPoint EPhost = new IPEndPoint(hostadd, Convert.ToInt32(strIndicatorPortNo));
        
            string weight = "";
            s3.ReceiveBufferSize = 0;
            s3.SendBufferSize = 0;
            s3.ReceiveTimeout = 500;
            try
            {

                s3.Connect(EPhost);

                if (s3.Connected)
                {

                    if (tbl.ModeOfComs == "1")
                    {
                        try
                        {
                            Byte[] sbyte1 = new Byte[] { 0x5 };

                            s3.Send(sbyte1);
                            System.Threading.Thread.Sleep(500);
                        }
                        catch { }
                    }

                    Byte[] receive = new Byte[37];


                    int ret = s3.Receive(receive, receive.Length, 0);
                    if (ret > 0)
                    {
                        string tmp = null;

                        tmp = System.Text.Encoding.ASCII.GetString(receive);
                        if (tmp.Length > 0)
                        {
                            weight = tmp.Substring(tmp.IndexOf("") + "".Length, 7).Trim();

                        }
                    }


                    s3.Disconnect(true);
                }
            }
            catch (Exception e1)
            {


                return output;

            }
           

            return weight;
          
        }

       
        protected void timer1_Tick(object sender, EventArgs e)
        {

           


            //Enable this code if weight machine connected on local client machine 
            //string Weight = GetWeightFromIP();  //comented by ss and added new code

            string Weight =  GetWeightFromTCPIP_SS();
            
            //Enable this code if code is run on public server 
            //string Weight = GetIPfromServerPort();
            string TruckNo = txtTruckNo.Text.Trim().ToUpper();
            RuntimeWeight.Text = Weight.Trim();
            //FillPendingTruck();

            if (string.IsNullOrEmpty(txtTruckNo.Text.Trim()))
            {
                Session["SECONDWT_RCD"] = "0";
                FirstWeight.Text = Weight.Trim();
                DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                FirstDate.Value = indianTime.ToString("dd/MM/yyyy");
                FirstTime.Value = indianTime.ToString("HH:mm:ss tt");

                FirstWeight.BackColor = Color.White;
                FirstWeight.BorderColor = Color.DarkBlue;
                SecondWeight.Style["background-color"] = "white";

                SecondDate.Value = string.Empty;
                SecondTime.Value = string.Empty;
                SecondWeight.Value = string.Empty;
                txtFinalWeight.Value = string.Empty;
                Session["blWeightInterlock"] = "0";
            }
            //if (string.IsNullOrEmpty(txtTruckNo.Text.Trim()) && SecondWeight.Value.ToString().Length != 0)
            //{
            //    SecondDate.Value = string.Empty;
            //    SecondTime.Value = string.Empty;
            //    SecondWeight.Value = string.Empty;
            //    txtFinalWeight.Value = string.Empty;

            //    ddlmaterial.SelectedItem.Value = "0";
            //    ddlmc.SelectedItem.Value = "0";
            //    ddlsupplier.SelectedItem.Value = "0";
            //    ddltransporter.SelectedItem.Value = "0";
            //    ddlpacking.SelectedItem.Value = "0";
            //    txtpackingqty.Text = string.Empty;
            //    txtInvoiceNo.Text = string.Empty;
            //    txtInvoiceDate.Text = string.Empty;
            //    txtChallanWeight.Text = string.Empty;
            //    txtPONo.Text = string.Empty;
            //    txtPODate.Text = string.Empty;
            //    txtremarks.Text = string.Empty;


            //}

            if (!string.IsNullOrEmpty(txtTruckNo.Text.Trim()))
            {
                //check truck is already in pending record or not
                if (!_transactionRepo.checkTruckIsPendingOrNot(TruckNo))
                {
                    //check Truck trip is saved under transaction file and id checked first weight and date time will bot update.
                    if (!_transactionRepo.checkTruckTripClosed(TruckNo))
                    {
                        if (Session["SECONDWT_RCD"].ToString() == "0")
                        {
                            FirstWeight.Text = Weight.Trim();
                            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                            FirstDate.Value = indianTime.ToString("dd/MM/yyyy");
                            FirstTime.Value = indianTime.ToString("HH:mm:ss tt");

                            FirstWeight.BackColor = Color.White;
                            FirstWeight.BorderColor = Color.DarkBlue;

                            SecondDate.Value = string.Empty;
                            SecondTime.Value = string.Empty;
                            SecondWeight.Value = string.Empty;
                            txtFinalWeight.Value = string.Empty;
                            Session["blWeightInterlock"] = "0";
                        }
                        if (Session["SECONDWT_RCD"].ToString() == "1")
                        {
                            SecondWeight.Style["background-color"] = "LightGray";

                        }
                    }
                }
                else
                {
                    tblTransaction tbltrans = _transactionRepo.getPendingTransactionById(TruckNo);
                    if (tbltrans != null)
                    {
                        FirstWeight.Text = tbltrans.FirstWeight.Value.ToString("0");
                        FirstDate.Value = tbltrans.FirstWtDateTime.Value.ToString("dd/MM/yyyy");
                        FirstTime.Value = tbltrans.FirstWtDateTime.Value.ToString("HH:mm:ss tt");
                        FirstWeight.BackColor = Color.LightGray;
                        FirstWeight.BorderColor = Color.Red;
                        Session["blWeightInterlock"] = "1";

                    }

                    //If truck in on pending record and need to take next weight
                    SecondWeight.Value = Weight;
                    DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    SecondDate.Value = indianTime.ToString("dd/MM/yyyy");
                    SecondTime.Value = indianTime.ToString("HH:mm:ss tt");
                    if (ddlinoutna.SelectedItem.Value == "0" || ddlinoutna.SelectedItem.Value == "2")
                    {
                        try
                        {
                            decimal FinalWeight = Math.Abs(Convert.ToDecimal(SecondWeight.Value) - Convert.ToDecimal(FirstWeight.Text));
                            txtFinalWeight.Value = FinalWeight.ToString("0");
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            decimal FinalWeight = Convert.ToDecimal(FirstWeight.Text) - Convert.ToDecimal(SecondWeight.Value);
                            txtFinalWeight.Value = FinalWeight.ToString("0");
                        }
                        catch { }
                    }
                }
            }

            this.ReadweightWB1();



        }

        protected void RuntimeWeight_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
            {
                FirstWeight.Text = RuntimeWeight.Text;
                FirstDate.Value = DateTime.Now.ToShortDateString();
                FirstTime.Value = DateTime.Now.TimeOfDay.ToString();
            }
        }

        private void ReadweightWB1()
        {
            try
            {
                double _dblZeroRangeWB1 = 0.00;
                this.lblRangeLockWB1.Visible = false;

                tblMachineWorkingParameter tbl = getLoggedInUserWeigh();
                if (tbl.ZeroInterlock == 1)
                {
                    if (lblUnit.Text == "t")
                    {
                        _dblZeroRangeWB1 = Convert.ToDouble(tbl.ZeroInterlockRange) / 1000;
                    }
                    else
                    {
                        _dblZeroRangeWB1 = Convert.ToDouble(tbl.ZeroInterlockRange);
                    }
                    if ((double.Parse(this.RuntimeWeight.Text.Trim() == "No weight" ? "0" : this.RuntimeWeight.Text) > _dblZeroRangeWB1 || this.RuntimeWeight.Text.Trim() == "No weight") && Session["blZeroRangeVisibleWB1"].ToString() == "1")
                    {
                        //blWeightInterlock = false;
                        //this.RuntimeWeight.Enabled = false;
                        this.lblRangeLockWB1.Visible = true;
                        if (this.RuntimeWeight.Text.Trim() == "No weight")
                        {
                            this.lblRangeLockWB1.Text = "Load the Weighbridge";
                        }
                        else
                        {
                            this.lblRangeLockWB1.Text = "Weight is not in zero range on Weighbridge";
                        }
                        this.lblRangeLockWB1.BackColor = Color.Red;
                        this.lblRangeLockWB1.ForeColor = Color.White;
                        return;
                    }
                    else
                    {
                        //this.blZeroRangeVisibleWB1 = false;
                        Session["blZeroRangeVisibleWB1"] = "0";
                        //this.RuntimeWeight.Enabled = true;
                    }
                }

                if (Math.Abs(double.Parse(string.IsNullOrEmpty(this.FirstWeight.Text.Trim()) ? "0" : this.FirstWeight.Text.Trim()) -
                    double.Parse(string.IsNullOrEmpty(this.RuntimeWeight.Text.Trim()) ? "0" : this.RuntimeWeight.Text.Trim()))
                    <= Convert.ToDouble(tbl.WeightInterlockRange))
                {

                    if (tbl.WeightInterlock== 1 && Session["blWeightInterlock"].ToString() == "1")
                    {
                        //blWeightInterlock == true;
                        this.RuntimeWeight.Enabled = false;
                        this.lblRangeLockWB1.Visible = true;
                        this.lblRangeLockWB1.Text = "Weight is not in weight range on Weighbridge";
                        this.lblRangeLockWB1.BackColor = Color.Red;
                        this.lblRangeLockWB1.ForeColor = Color.White;
                    }
                    else
                    {
                        //blWeightInterlock = false;
                        this.RuntimeWeight.Enabled = true;
                        Session["blWeightInterlock"] = "0";
                    }
                }

            }
            catch
            {
                //btnWeight1.Text = "-1";
            }
        }

        private void takeweight()
        {
            if (string.IsNullOrEmpty(txtTruckNo.Text.Trim()) && SecondWeight.Value.ToString().Length != 0)
            {
                SecondDate.Value = string.Empty;
                SecondTime.Value = string.Empty;
                SecondWeight.Value = string.Empty;
                txtFinalWeight.Value = string.Empty;
            }

            //check truck is already in pending record or not
            if (!_transactionRepo.checkTruckIsPendingOrNot(txtTruckNo.Text.Trim().ToUpper()))
            {
                //check Truck trip is saved under transaction file and id checked first weight and date time will bot update.
                if (_transactionRepo.checkTruckIsPendingOrNot(txtTruckNo.Text.Trim().ToUpper()))
                {
                    FirstWeight.Text = RuntimeWeight.Text.Trim();
                    DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    FirstDate.Value = indianTime.ToString("dd/MM/yyyy");
                    FirstTime.Value = indianTime.ToString("HH:mm:ss tt");
                    
                    
                }
            }
            else
            {
                tblTransaction tbltrans = _transactionRepo.getPendingTransactionById(txtTruckNo.Text.Trim().ToUpper());
                if (tbltrans != null)
                {
                    FirstWeight.Text = tbltrans.FirstWeight.Value.ToString("0");
                    FirstDate.Value  = tbltrans.FirstWtDateTime.Value.ToString("dd/MM/yyyy");
                    FirstTime.Value  = tbltrans.FirstWtDateTime.Value.ToString("HH:mm:ss tt");
                    FirstWeight.Enabled = false;
                    FirstWeight.BackColor = Color.Gray;


                }

                //If truck in on pending record and need to take next weight
                SecondWeight.Value = RuntimeWeight.Text.Trim();
                DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                SecondDate.Value = indianTime.ToString("dd/MM/yyyy");
                SecondTime.Value = indianTime.ToString("HH:mm:ss tt");
                if (ddlinoutna.SelectedItem.Value == "0" || ddlinoutna.SelectedItem.Value == "2")
                {
                    try
                    {
                        decimal FinalWeight = Math.Abs(Convert.ToDecimal(SecondWeight.Value) - Convert.ToDecimal(FirstWeight.Text));
                        txtFinalWeight.Value = FinalWeight.ToString("0");
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        decimal FinalWeight = Convert.ToDecimal(FirstWeight.Text) - Convert.ToDecimal(SecondWeight.Value);
                        txtFinalWeight.Value = FinalWeight.ToString("0");
                    }
                    catch { }
                }
            }
        }

        private void BindMaterialDropDown()
        {
            ddlmaterial.DataTextField = "Name";
            ddlmaterial.DataValueField = "MaterialCode";
            ddlmaterial.DataSource = _materialrepo.GetModelMaterials();
            ddlmaterial.DataBind();
            ddlmaterial.Items.Insert(0, new ListItem("Select", ""));
        }

        private void BindMaterialClassificationDropdown()
        {
            MaterialClassificationRepository repository = new MaterialClassificationRepository();
            ddlmc.DataTextField = "Name";
            ddlmc.DataValueField = "Code";
            ddlmc.DataSource = repository.GetMaterialClassifications_Code();
            ddlmc.DataBind();
            ddlmc.Items.Insert(0, new ListItem("Select", "0"));
        }

        private void BindSupplierDropdown()
        {
            ddlsupplier.DataTextField = "Name";
            ddlsupplier.DataValueField = "Code";
            ddlsupplier.DataSource = _supplierrepo.Get_SupplierCode();
            ddlsupplier.DataBind();
            ddlsupplier.Items.Insert(0, new ListItem("Select", "0"));
        }
        private void BindTransporterDropdown()
        {
            ddltransporter.DataTextField = "Name";
            ddltransporter.DataValueField = "ddCode";
            ddltransporter.DataSource = _transrepo.Get_Transporters();
            ddltransporter.DataBind();
            ddltransporter.Items.Insert(0, new ListItem("Select", "0"));
        }
        private void BindPackingDropDown()
        {
            ddlpacking.DataTextField = "Name";
            ddlpacking.DataValueField = "PackingCode";
            ddlpacking.DataSource = _Packingrepo.Get_PackingCode();
            ddlpacking.DataBind();
            ddlpacking.Items.Insert(0, new ListItem("Select", "0"));
        }

        protected void save_Click(object sender, EventArgs e)
        {
            //takeweight();

            //DialogResult result = MessageBox.Show("Are you sure to save the weight ?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            //if (result == DialogResult.No)
            //{
            //    return;
            //}
            //else if (result == DialogResult.No)
            //{
            //    MessageBox.Show("You chose No.");
            //}

            if (RuntimeWeight.Text.Trim()== "No weight")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('No Weight on digitizer');", true);
                adderrorlog("No Weight on digitizer", "Indicator");
                return;
            }

            if (txtInvoiceDate.Text.Trim().Length != 0)
            {
                if (Convert.ToDateTime(txtInvoiceDate.Text) > DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Challan Date/Invoice date should not be greater than current date. ');", true);
                    adderrorlog("Challan Date/Invoice date should not be greater than current date.", "Invoice Date");
                    return;
                }
            }

            if (txtPODate.Text.Trim().Length != 0)
            {
                if (Convert.ToDateTime(txtPODate.Text) > DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('PO/SO date should not be greater than current date. ');", true);
                    return;
                }
            }

            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "No")
            {
            //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
            //}
            //else
            //{
                //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
                return;
            }

            save.Enabled = false;
            tblMachineWorkingParameter tbl = getLoggedInUserWeigh();
            if (tbl.ZeroInterlock == 1 && Session["blZeroRangeVisibleWB1"].ToString() == "1")  // || GlobalVariable.blWeightInterLock == true)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Weight is not in zero range on Weighbridge');", true);
                adderrorlog("Weight is not in zero range on Weighbridge.", "Zero Interlock");
                save.Enabled = true;
                return;
            }
            if (tbl.ZeroInterlock==1)  // || GlobalVariable.blWeightInterLock == true)
            {
                Session["blZeroRangeVisibleWB1"] = "1";
            }

            if (tbl.WeightInterlock == 1 && Session["blWeightInterlock"].ToString() == "1")  // || GlobalVariable.blWeightInterLock == true)
            {
                if (this.lblRangeLockWB1.Text.Trim() == "Weight is not in weight range on Weighbridge" && this.lblRangeLockWB1.Visible == true)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Weight is not in weight range on Weighbridge');", true);
                    adderrorlog("Weight is not in weight range on Weighbridge", "Weight Interlock");
                    save.Enabled = true;
                    return;
                }
            }

            if (FirstWeight.Text.Trim().Length != 0 && SecondWeight.Value.Trim().Length != 0)
            {
                tblMachineWorkingParameter tbl1 = getLoggedInUserWeigh();
                if ( Convert.ToDecimal(txtFinalWeight.Value) < tbl1.NetWeightLimit)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Net Weight should me greater than net weight limit.');", true);
                    adderrorlog("Net Weight should me greater than net weight limit.", "Net Weight Limit");
                    save.Enabled = true;
                    return;
                }
                
            }

           

            //if (tbl.WeightInterlock == 1 && Session["blWeightInterlock"].ToString() == "1")  // || GlobalVariable.blWeightInterLock == true)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Weight is not in weight range on Weighbridge');", true);
            //    save.Enabled = true;
            //    return;
            //}
            //if (tbl.WeightInterlock == 1)  // || GlobalVariable.blWeightInterLock == true)
            //{
            //    Session["blZeroRangeVisibleWB1"] = "1";
            //}

            if (Session["TareCheck"].ToString() == "1")
            {
                if (_transactionRepo.checkTruckTolerance(txtTruckNo.Text.Trim().ToUpper()) == "No")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Tare weight is out of tolerance limit.');", true);
                    adderrorlog("Tare weight is out of tolerance limit.", "Tare Weight Check");
                    save.Enabled = true;
                    return;
                }
            }

            bool SecondWt = false;
            if (RuntimeWeight.Text == "No weight" || RuntimeWeight.Text == "0" || txtTruckNo.Text.Trim().Length ==0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record cannot be captured. No Weight comes.');", true);
                adderrorlog("Record cannot be captured. No Weight comes.", "No Weight");
            }
            else
            {
                //if (string.IsNullOrEmpty(txtTruckNo.Text))
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Truck no cannot be leave blank.');", true);
                //else
                //{

                if (checkmultiproduct.Checked)
                {
                    if (ddlmaterial.SelectedItem.Value != "")
                    {
                        if (CheckSameMaterialExist(this.txtTripId.Text.Trim(), ddlmaterial.SelectedItem.Value.Trim())=="1")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('This Material is already exists.');", true);
                            adderrorlog("This Material is already exists.", "Multi Weighing");
                            save.Enabled = true;
                            return;
                        }
                    }
                }
                string res = "";
                if (!_transactionRepo.checkTruckIsPendingOrNot(txtTruckNo.Text))
                {
                    res = CheckValidation();
                }
                else
                {
                    res = CheckSecondValidation();
                    
                }
                if (res.Split(':')[0] == "1")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Field " + res.Split(':')[1].ToString() + " Is Mandatory.');", true);
                    adderrorlog("Field " + res.Split(':')[1].ToString() + " Is Mandatory.", "Manadatory");
                    save.Enabled = true;
                }
                else
                {
                    bool IsFirstWeight = false;
                    decimal LiveWeight = 0;
                    bool isInbound = false;
                    try
                    {
                        LiveWeight = Convert.ToDecimal(RuntimeWeight.Text);
                    }
                    catch { }

                    if (LiveWeight > 0)
                    {
                        decimal diff = 0;
                        //if (timer1.Enabled == true)
                        //{
                            string TruckNo = txtTruckNo.Text.Trim().ToUpper();
                            string varTareToleranceStatus = string.Empty;
                            if (!_transactionRepo.checkTruckIsPendingOrNot(TruckNo))
                            {
                                IsFirstWeight = true;
                                FirstWeight.Text = RuntimeWeight.Text;
                                FirstDate.Value = DateTime.Now.ToShortDateString();
                                FirstTime.Value = DateTime.Now.TimeOfDay.ToString();
                                this.Flag_FirstWt_Records = true;
                                this.Flag_SecondWt_Records = false;
                                Session["FIRSTWT_RCD"] = "1";
                                Session["SECONDWT_RCD"] = "0";
                                txtTripId.Text = _transactionRepo.GetTripId().ToString();
                                if (ddlinoutna.SelectedItem.Value=="2" && Session["TareCheck"].ToString() == "1")
                                {
                                    varTareToleranceStatus = CheckTareWeightToleranceLimit();
                                }
                            }
                            else
                            {
                                SecondWeight.Value = RuntimeWeight.Text;
                                SecondDate.Value = DateTime.Now.ToShortDateString();
                                SecondTime.Value = DateTime.Now.TimeOfDay.ToString();
                                this.Flag_FirstWt_Records = false;
                                this.Flag_SecondWt_Records = true;
                                Session["FIRSTWT_RCD"] = "0";
                                Session["SECONDWT_RCD"] = "1";

                                if (ddlinoutna.SelectedItem.Value == "1" && Session["TareCheck"].ToString() == "1")
                                {
                                    varTareToleranceStatus = CheckTareWeightToleranceLimit();
                                }

                            }
                        //}
                        if (ddlinoutna.SelectedItem.Value == "1")
                        {
                            try
                            {
                                diff = Convert.ToDecimal(FirstWeight.Text) - Convert.ToDecimal(SecondWeight.Value);
                            }
                            catch { }
                        }
                        else if (ddlinoutna.SelectedItem.Value == "2")
                        {
                            try
                            {
                                diff = Convert.ToDecimal(SecondWeight.Value) - Convert.ToDecimal(FirstWeight.Text);
                            }
                            catch { }
                        }
                        else
                        {
                            if (checkmultiproduct.Checked)
                            {
                                int transid = 0;
                                try
                                {
                                    transid = Convert.ToInt32(txtTripId.Text);
                                }
                                catch { }
                                IList<tblTransactionMaterial> tbltransWei = _transactionRepo.getmaterialsByTransactionId(transid);
                                try
                                {
                                    diff = Convert.ToDecimal(FirstWeight.Text) - Convert.ToDecimal(SecondWeight.Value);
                                }
                                catch { }
                                if (tbltransWei.Count == 1)
                                {
                                    if (diff > 0)
                                        isInbound = true;

                                    else
                                    {
                                        diff = diff * -1;
                                        isInbound = false;
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    diff = Convert.ToDecimal(FirstWeight.Text) - Convert.ToDecimal(SecondWeight.Value);
                                }
                                catch { }
                                if (diff > 0)
                                    isInbound = true;
                                else
                                {
                                    diff = diff * -1;
                                    isInbound = false;
                                }
                            }
                        }
                        
                        if (diff > 0  || IsFirstWeight )
                        {
                            Model_ManualWeight model = new Model_ManualWeight();
                            model.trans = new tblTransaction();
                            try
                            {
                                //DateTime vchallandate = DateTime.ParseExact(txtInvoiceDate.Text, "dd/MM/yyyy", new CultureInfo("en-GB"));
                                model.trans.ChallanDate = Convert.ToDateTime(txtInvoiceDate.Text);
                            }
                            catch { }
                            model.trans.TARETOLERANCESTATUS = varTareToleranceStatus;
                            model.trans.ChallanNo = txtInvoiceNo.Text;
                            model.trans.ChallanWeight = txtChallanWeight.Text;
                            model.trans.CreateDate = DateTime.Now;
                            model.trans.GateEntryNo = txtgateentryno.Text;
                            model.trans.IsMultiProduct = checkmultiproduct.Checked;
                            if (ddlmc.SelectedItem.Value == "0")
                            {
                                model.trans.MaterialCalssificationCode = ddlmc.SelectedItem.Value;
                                model.trans.MaterialClassificationName = "";
                            }
                            else
                            {
                                model.trans.MaterialCalssificationCode = ddlmc.SelectedItem.Value;
                                model.trans.MaterialClassificationName = ddlmc.SelectedItem.Text.Split('(')[0].Replace("(", "");
                            }

                            try
                            {
                                if (ddlmaterial.SelectedItem.Value == "0")
                                {
                                    model.trans.MaterialCode = ddlmaterial.SelectedItem.Value;
                                    model.trans.MaterialName = ""; ;
                                }
                                else
                                {
                                    model.trans.MaterialCode = ddlmaterial.SelectedItem.Value;
                                    model.trans.MaterialName = ddlmaterial.SelectedItem.Text.Split('(')[0].Replace("(", "");
                                }
                            }
                            catch { }
                            if (ddlpacking.SelectedItem.Value == "0")
                            {
                                model.trans.PackingCode = ddlpacking.SelectedItem.Value;
                                model.trans.PackingName = "";
                            }
                            else
                            {
                                model.trans.PackingCode = ddlpacking.SelectedItem.Value;
                                model.trans.PackingName = ddlpacking.SelectedItem.Text.Split('(')[0].Replace("(", "");
                            }
                            try
                            {
                                model.trans.PackingQty = Convert.ToInt32(txtpackingqty.Text);
                            }
                            catch { }
                            try
                            {
                                //DateTime vpodatedate = DateTime.ParseExact(txtPODate.Text, "dd/MM/yyyy", new CultureInfo("en-GB"));
                                model.trans.PODate =  Convert.ToDateTime(txtPODate.Text);
                            }
                            catch { }
                            model.trans.PONo = txtPONo.Text;
                            model.trans.Remarks = txtremarks.Text;
                            if (ddlsupplier.SelectedItem.Value == "0")
                            {
                                model.trans.SupplierCode = ddlsupplier.SelectedItem.Value;
                                model.trans.SupplierName = "";
                            }
                            else
                            {
                               
                                model.trans.SupplierCode = ddlsupplier.SelectedItem.Value;
                                model.trans.SupplierName = ddlsupplier.SelectedItem.Text.Split('(')[0].Replace("(", "");
                            }

                            model.trans.TransactionStatus = 1;
                            if (ddltransporter.SelectedItem.Value == "0")
                            {
                                model.trans.TransporterCode = ddltransporter.SelectedItem.Value;
                                model.trans.TransporterName = "";
                            }
                            else
                            {
                                model.trans.TransporterCode = ddltransporter.SelectedItem.Value;
                                model.trans.TransporterName = ddltransporter.SelectedItem.Text.Split('(')[0].Replace("(", "");
                            }

                            model.trans.TripId = Convert.ToInt32(txtTripId.Text);
                            model.trans.TripType = ddlinoutna.SelectedItem.Value != "0" || IsFirstWeight ? Convert.ToInt32(ddlinoutna.SelectedItem.Value) : isInbound == true ? 1 : 2;
                            ddlinoutna.SelectedValue = ddlinoutna.SelectedItem.Value != "0" || IsFirstWeight ? ddlinoutna.SelectedItem.Value : isInbound == true ? "1" : "2";
                            model.trans.TruckNo = txtTruckNo.Text.ToUpper();

                            
                            if (!_transactionRepo.checkTruckIsPendingOrNot(txtTruckNo.Text.ToUpper()))
                            {
                                model.trans.FirstWeight = Convert.ToDecimal(FirstWeight.Text);
                                model.trans.FirstWtDateTime = Convert.ToDateTime(FirstTime.Value);
                                SecondWt = false;
                                this.Flag_FirstWt_Records = true;
                                this.Flag_SecondWt_Records = false;
                                Session["FIRSTWT_RCD"] = "1";
                                Session["SECONDWT_RCD"] = "0";
                                
                            }
                            else
                            {
                                model.trans.SecondWeight = Convert.ToDecimal(SecondWeight.Value);
                                model.trans.SecondWtDateTime = Convert.ToDateTime(SecondTime.Value);
                                string FinalWeightCaptured = txtFinalWeight.Value;
                                model.trans.NetWeight = Convert.ToDecimal(FinalWeightCaptured);
                                model.trans.Shift= Shift(DateTime.Now).Substring(0, 1);
                                model.trans.SHIFTDATE= Convert.ToDateTime(Shift(DateTime.Now).Substring(2));
                                SecondWt = true;
                                this.Flag_FirstWt_Records = false;
                                this.Flag_SecondWt_Records = true;
                                Session["FIRSTWT_RCD"] = "0";
                                Session["SECONDWT_RCD"] = "1";
                                
                               
                            }
                            if (checkmultiproduct.Checked)
                            {
                                tblTransaction trans = _transactionRepo.getPendingTransactionById(txtTruckNo.Text);
                                if (trans  != null)
                                //if (!string.IsNullOrEmpty(trans.FirstWeight.ToString()))
                                {
                                    tblTransactionMaterial mat = new tblTransactionMaterial();
                                    mat.CreteDate = DateTime.Now;
                                    try
                                    {
                                        mat.MaterialCode = ddlmaterial.SelectedItem.Value;
                                    }
                                    catch { }
                                    try
                                    {
                                        mat.MaterialName = ddlmaterial.SelectedItem.Text.Split('(')[0].Replace("(", "");
                                    }
                                    catch { }
                                    mat.TransactionId = trans.TripId;   // Convert.ToInt32(txtTripId.Text);
                                    string varTotalMatWeight = TotalMaterialWt(trans.TripId.ToString());
                                    if (trans.TripType == 1)
                                    {
                                        if (trans.SecondWeight == null && varTotalMatWeight=="0")
                                        {
                                            mat.Weight = (Convert.ToDecimal(FirstWeight.Text) - Convert.ToDecimal(SecondWeight.Value));
                                        }
                                        else
                                        {
                                            mat.Weight = ((Convert.ToDecimal(FirstWeight.Text) - Convert.ToDecimal(varTotalMatWeight)) - Convert.ToDecimal(SecondWeight.Value));
                                        }
                                        
                                    }
                                    else
                                    {
                                        if (trans.SecondWeight == null && varTotalMatWeight == "0")
                                        {
                                            mat.Weight = (Convert.ToDecimal(SecondWeight.Value) - Convert.ToDecimal(FirstWeight.Text));
                                        }
                                        else
                                        {
                                            mat.Weight = ((Convert.ToDecimal(SecondWeight.Value) - Convert.ToDecimal(varTotalMatWeight)) - Convert.ToDecimal(FirstWeight.Text));
                                        }
                                    }

                                    model.material = mat;
                                }
                                
                            }
                            model.UserName = Session["UserName"].ToString();
                            model.WeibridgeId = Session["WBID"].ToString();
                            model.trans.PlantCode = Session["PlantID"].ToString();
                            model.trans.WeighingUnit= Session["WEIGHINGUNIT"].ToString();
                            model.trans.CompanyCode = Session["CompanyCode"].ToString();  // "com1";
                            model.trans.WeighbridgeId  = Session["WBID"].ToString();
                            model.plantCode = Session["PlantID"].ToString();
                            
                            model.companyCode = Session["CompanyCode"].ToString();  // "com1";
                            model.trans.PRINT_TICKET = "N";

                            // Save Transactio record in database
                            _transactionRepo.saveTransactionRecord(model);
                           
                            
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record added successfully.');", true);
                            if (SecondWt)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "COnfirmClick();", true);
                            }
                        }
                        else
                        {
                            
                            if (ddlinoutna.SelectedItem.Value == "1")
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Weight should me less than First weight.');", true);
                                adderrorlog("Weight should me less than First weight.", " + txtTruckNo.Text.ToUpper() + ");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Weight should be more than First Weight.');", true);
                                adderrorlog("Weight should be more than First Weight.", " + txtTruckNo.Text.ToUpper() + ");
                            }
                            
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Weight comes 0. Weight Cannot be captured');", true);
                        adderrorlog("Weight comes 0. Weight Cannot be captured", "no weight");
                    }
                }
            }
            save.Enabled = true;
        }

        public void StoredTareWeight()
        {
            //TruckTareWeight _trucktarewt = db.TruckTareWeights.FirstOrDefault();
            //_trucktarewt = new TruckTareWeight();
            //if (ddlinoutna.SelectedItem.Value == "1")
            //{
            //    _trucktarewt.TruckNo = txtTruckNo.Text.Trim().ToUpper();
            //    _trucktarewt.TareWeight = Convert.ToDecimal(SecondWeight.Value);
            //    _trucktarewt.TareWtDateTime = Convert.ToDateTime(SecondTime.Value);
            //}
            //else
            //{
            //    _trucktarewt.TruckNo = txtTruckNo.Text.Trim().ToUpper();
            //    _trucktarewt.TareWeight = Convert.ToDecimal(SecondWeight.Value);
            //    _trucktarewt.TareWtDateTime = Convert.ToDateTime(SecondTime.Value);
            //}
            //db.TruckTareWeights.InsertOnSubmit(_trucktarewt);
            //db.SubmitChanges();
        }

        public void OnConfirm(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            int transId = Convert.ToInt32(txtTripId.Text);
            if (confirmValue == "Yes")
            {
                _transactionRepo.CloseTicket(transId);
            }
            else
            {
                
            }
        }

        public string CheckTareWeightToleranceLimit()
        {
            tblMachineWorkingParameter _tmachine = db.tblMachineWorkingParameters.Where(x => x.PlantCode == Session["PlantID"].ToString() && x.MachineId == Session["WBID"].ToString()).FirstOrDefault();
            var query1 = "Yes";
            if (_tmachine.TareCheck == 1 && _tmachine.TareScheme != "")
            {
                var query = string.Empty;
               
                double dbl_uppertolerance = 0;
                double dbl_lowertolerance = 0;

                if (_tmachine.TareScheme == "Average(All)")
                {
                    query = (from q in db.TruckTareWeights where q.TruckNo == txtTruckNo.Text.Trim() select q.TareWeight).Average().ToString();
                    if (query != "")
                    {
                        dbl_uppertolerance = Convert.ToDouble(query) + Convert.ToDouble(_tmachine.TareWeightValue);
                        dbl_lowertolerance = Convert.ToDouble(query) - Convert.ToDouble(_tmachine.TareWeightValue);

                        if (Convert.ToDouble(RuntimeWeight.Text) > dbl_uppertolerance || Convert.ToDouble(RuntimeWeight.Text) < dbl_lowertolerance)
                        {
                            query1 = "No";

                        }
                    }
                }
                if (_tmachine.TareScheme == "Tolerance(%)")
                {
                    //query = (from q in db.TruckTareWeights where q.TruckNo == txtTruckNo.Text.Trim() orderby q.TareWtDateTime descending select q.TareWeight).First().ToString();
                    query = (from q in db.TruckTareWeights where q.TruckNo == txtTruckNo.Text.Trim() select q.TareWeight).Average().ToString();
                    if (query != "")
                    {
                        dbl_uppertolerance = Convert.ToDouble(query) + (Convert.ToDouble(query) * Convert.ToDouble(_tmachine.TareWeightValue)) / 100;
                        dbl_lowertolerance = Convert.ToDouble(query) - (Convert.ToDouble(query) * Convert.ToDouble(_tmachine.TareWeightValue)) / 100;

                        if (Convert.ToDouble(RuntimeWeight.Text) > dbl_uppertolerance || Convert.ToDouble(RuntimeWeight.Text) < dbl_lowertolerance)
                        {
                            query1 = "No";

                        }
                    }
                }

                if (_tmachine.TareScheme == "Tolerance(kg)")
                {
                    //query = (from q in db.TruckTareWeights where q.TruckNo == txtTruckNo.Text.Trim() orderby q.TareWtDateTime descending select q.TareWeight).First().ToString();
                    query = (from q in db.TruckTareWeights where q.TruckNo == txtTruckNo.Text.Trim() select q.TareWeight).Average().ToString();
                    if (query != "")
                    {
                        dbl_uppertolerance = Convert.ToDouble(query) + Convert.ToDouble(_tmachine.TareWeightValue);
                        dbl_lowertolerance = Convert.ToDouble(query) - Convert.ToDouble(_tmachine.TareWeightValue);

                        if (Convert.ToDouble(RuntimeWeight.Text) > dbl_uppertolerance || Convert.ToDouble(RuntimeWeight.Text) < dbl_lowertolerance)
                        {
                            query1 = "No";

                        }
                    }
                }
            }
            return query1;
        }


        public string TotalMaterialWt(string vartripid)
        {
            tblTransactionMaterial  m_trans = db.tblTransactionMaterials.FirstOrDefault(x => x.TransactionId  == Convert.ToInt32(vartripid));
            if (m_trans != null)
            {
                if (db.tblTransactionMaterials.Count() > 0)
                {
                   
                    var max_mweight = (from a in db.tblTransactionMaterials
                                      where a.TransactionId == Convert.ToInt32(vartripid)
                                       select a).Sum(a => a.Weight);

                    return max_mweight.ToString();
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }

        }

        public string CheckSameMaterialExist(string vartripid,string varMatCode)
        {
            tblTransactionMaterial m_trans = db.tblTransactionMaterials.FirstOrDefault(x => x.TransactionId == Convert.ToInt32(vartripid) && x.MaterialCode==varMatCode);
            if (m_trans != null)
            {
                if (db.tblTransactionMaterials.Count() > 0)
                {

                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }

        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            string strTripId = string.Empty;

            if (!string.IsNullOrEmpty(this.txtTruckNo.Text.Trim()))
            {
                //strTripId = _transactionRepo.GetTripId_new(Session["WBID"].ToString(),this.txtTruckNo.Text.Trim()).ToString();
                strTripId = _transactionRepo.GetTripId_SS(Session["WBID"].ToString(),this.txtTripId.Text.Trim(), this.txtTruckNo.Text.Trim()).ToString();
                //strTripId = this.txtTripId.Text.Trim();
                if (strTripId == "0")
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('No Records');", true);
                else
                {
                    if (File.Exists(Server.MapPath("~/pdfs/" + strTripId + ".pdf")))
                    {
                        File.Delete(Server.MapPath("~/pdfs/" + strTripId + ".pdf"));
                    }

                    Ticket ts = new Ticket();
                    ts.GetTicket(Convert.ToInt32(strTripId));
                    //lnkPrint.OnClientClick = "target='_blank'";
                    //Response.Redirect("~/pdfs/" + strTripId + ".pdf");
                    //lnkPrint.Attributes.Add("href",String.Format("/pdfs/" + strTripId + ".pdf"));
                    //lnkPrint.Attributes.Add("target","_blank");
                    var varurl = "/pdfs/" + strTripId + ".pdf";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('" + varurl + "','_newtab');", true);

                    //if (File.Exists(Server.MapPath("~/pdfs/" + strTripId + ".pdf")))
                    //{
                    //    File.Delete(Server.MapPath("~/pdfs/" + strTripId + ".pdf"));
                    //}

                    //new line added for duplicate print

                    tblTransaction _trantkt = db.tblTransactions.FirstOrDefault(x => x.TripId == Convert.ToInt32(strTripId) && x.WeighbridgeId == Session["WBID"].ToString());

                    if (_trantkt != null && _trantkt.SecondWeight !=null )
                    {
                        // _trantkt.print_ticket = "Y";
                        _trantkt.PRINT_TICKET = "Y";
                        db.SubmitChanges();
                    }

                    //end of duplicate

                }
            }
            
        }

        //protected void timer2_Tick(object sender, EventArgs e)
        //{

            
            
        //}

        //protected void timer3_Tick(object sender, EventArgs e)
        //{

            


        //}

        protected  string Shift(DateTime dtCurrent)
        {
            string _strShift = string.Empty;
           
            SHIFTTIME _shift = db.SHIFTTIMEs.FirstOrDefault(x => x.Id == 1);
            //DataTable _dtblShift = ModuleClasses.ClsDatabase.GetFieldValues("SHIFTTIME", "*");
            //OleDbDataAdapter oleAdpt = new OleDbDataAdapter("Select * from SHIFTTIME", GlobalConnection.oleGlobalConnection); //GlobalConnection.GlobalConnectionString); //objConnection.oleCon);
            //oleAdpt.Fill(_dtblShift);
            if (_shift != null)
            {
                string _STA = _shift.STA.ToString();
                string _EDA = _shift.EDA.ToString();
                string _STB = _shift.STB.ToString();
                string _EDB = _shift.EDB.ToString();
                string _STC = _shift.STC.ToString();
                string _EDC = _shift.EDC.ToString();
                switch (_shift.NOOFSHIFTS.ToString())
                {
                    case "ONE":
                        if (DateTime.Parse(_EDA).TimeOfDay < DateTime.Parse(_STA).TimeOfDay)
                        {
                            if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDA).TimeOfDay)
                                _strShift = "A," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse("23:59:59").TimeOfDay)
                                _strShift = "A," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay > DateTime.Parse(_EDA).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                        }
                        else if (DateTime.Parse(_EDA).TimeOfDay > DateTime.Parse(_STA).TimeOfDay)
                        {
                            if (dtCurrent.TimeOfDay >= DateTime.Parse(_STA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDA).TimeOfDay)
                                _strShift = "A," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_EDA).TimeOfDay)
                                _strShift = "G," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STA).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                        }
                        else if (DateTime.Parse(_EDA).TimeOfDay == DateTime.Parse(_STA).TimeOfDay)
                        {
                            if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDA).TimeOfDay)
                                _strShift = "A," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STA).TimeOfDay && dtCurrent.TimeOfDay <= DateTime.Parse("23:59:59").TimeOfDay)
                                _strShift = "A," + DateTime.Now.ToShortDateString();
                        }

                        break;
                    case "TWO":
                        if (DateTime.Parse(_EDA).TimeOfDay < DateTime.Parse(_STA).TimeOfDay)
                        {
                            if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDA).TimeOfDay)
                                _strShift = "A," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STA).TimeOfDay && dtCurrent.TimeOfDay <= DateTime.Parse("23:59:59").TimeOfDay)
                                _strShift = "A," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STB).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDB).TimeOfDay)
                                _strShift = "B," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_EDA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STB).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_EDB).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STA).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                        }
                        else if (DateTime.Parse(_EDB).TimeOfDay < DateTime.Parse(_STB).TimeOfDay)
                        {
                            if (dtCurrent.TimeOfDay >= DateTime.Parse(_STA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDA).TimeOfDay)
                                _strShift = "A," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STB).TimeOfDay && dtCurrent.TimeOfDay <= DateTime.Parse("23:59:59").TimeOfDay)
                                _strShift = "B," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDB).TimeOfDay)
                                _strShift = "B," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_EDB).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STA).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_EDA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STB).TimeOfDay)
                                _strShift = "G," + DateTime.Now.ToShortDateString();
                        }
                        else if (DateTime.Parse(_EDB).TimeOfDay > DateTime.Parse(_STB).TimeOfDay)
                        {
                            if (dtCurrent.TimeOfDay >= DateTime.Parse(_STA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDA).TimeOfDay)
                                _strShift = "A," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STB).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDB).TimeOfDay)
                                _strShift = "B," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STA).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_EDA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STB).TimeOfDay)
                                _strShift = "G," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_EDB).TimeOfDay && dtCurrent.TimeOfDay <= DateTime.Parse("23:59:59").TimeOfDay)
                                _strShift = "G," + DateTime.Now.ToShortDateString();
                        }
                        break;
                    case "THREE":
                        if (DateTime.Parse(_EDA).TimeOfDay < DateTime.Parse(_STA).TimeOfDay)
                        {
                            if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDA).TimeOfDay)
                                _strShift = "A," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STA).TimeOfDay && dtCurrent.TimeOfDay <= DateTime.Parse("23:59:59").TimeOfDay)
                                _strShift = "A," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STB).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDB).TimeOfDay)
                                _strShift = "B," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STC).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDC).TimeOfDay)
                                _strShift = "C," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_EDA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STB).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_EDB).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STC).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_EDC).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STA).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                        }
                        else if (DateTime.Parse(_EDB).TimeOfDay < DateTime.Parse(_STB).TimeOfDay)
                        {
                            if (dtCurrent.TimeOfDay >= DateTime.Parse(_STA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDA).TimeOfDay)
                                _strShift = "A," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STB).TimeOfDay && dtCurrent.TimeOfDay <= DateTime.Parse("23:59:59").TimeOfDay)
                                _strShift = "B," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDB).TimeOfDay)
                                _strShift = "B," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_STC).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDC).TimeOfDay)
                                _strShift = "C," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_EDA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STB).TimeOfDay)
                                _strShift = "G," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_EDB).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STC).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_EDC).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STA).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                        }
                        else if (DateTime.Parse(_EDC).TimeOfDay < DateTime.Parse(_STC).TimeOfDay)
                        {
                            if (dtCurrent.TimeOfDay >= DateTime.Parse(_STA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDA).TimeOfDay)
                                _strShift = "A," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STB).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDB).TimeOfDay)
                                _strShift = "B," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STC).TimeOfDay && dtCurrent.TimeOfDay <= DateTime.Parse("23:59:59").TimeOfDay)
                                _strShift = "C," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDC).TimeOfDay)
                                _strShift = "C," + ShiftDate().ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_EDA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STB).TimeOfDay)
                                _strShift = "G," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_EDB).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STC).TimeOfDay)
                                _strShift = "G," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay >= DateTime.Parse(_EDC).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STA).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                        }
                        else if (DateTime.Parse(_EDC).TimeOfDay > DateTime.Parse(_STC).TimeOfDay)
                        {
                            if (dtCurrent.TimeOfDay >= DateTime.Parse(_STA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDA).TimeOfDay)
                                _strShift = "A," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STB).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDB).TimeOfDay)
                                _strShift = "B," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_STC).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_EDC).TimeOfDay)
                                _strShift = "C," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_EDA).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STB).TimeOfDay)
                                _strShift = "G," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_EDB).TimeOfDay && dtCurrent.TimeOfDay < DateTime.Parse(_STC).TimeOfDay)
                                _strShift = "G," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse(_EDC).TimeOfDay && dtCurrent.TimeOfDay <= DateTime.Parse("23:59:59").TimeOfDay)
                                _strShift = "G," + DateTime.Now.ToShortDateString();
                            else if (dtCurrent.TimeOfDay >= DateTime.Parse("00:00:00").TimeOfDay && dtCurrent.TimeOfDay <= DateTime.Parse(_STA).TimeOfDay)
                                _strShift = "G," + ShiftDate().ToShortDateString();
                        }
                        break;
                }
            }
            return _strShift;
        }

        private static DateTime ShiftDate()
        {
           

            System.DateTime dtPreDate = DateTime.Today.AddDays(-1);

            return dtPreDate;
        }

      

        private string CheckValidation()
        {
            bool check = false;
            string _FieldName = "";
            IList<DynamicFieldName> model = _transrepo.GetFieldNameByMachine(Session["WBID"].ToString(), Session["PlantID"].ToString());
            if (model.Count > 0)
            {
                foreach (DynamicFieldName names in model)
                {

                    if (names.FieldName == "Trip Id")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (string.IsNullOrEmpty(txtTripId.Text))
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }

                    //if (names.FieldName == "Weighing Type")
                    //{
                    //    if (names.IsMandatory1 == true)
                    //    {
                    //        if (ddlinoutna.SelectedItem.Value == "0")
                    //        {
                    //            check = true;
                    //            _FieldName = _FieldName + names.FieldValue + ",";
                    //            break;
                    //        }
                    //    }

                    //}
                    //if (names.FieldName == "Multi Product")
                    //{
                    //    if (names.IsMandatory1 == true)
                    //    {
                    //        if (!checkmultiproduct.Checked)
                    //        {
                    //            check = true;
                    //            _FieldName = _FieldName + names.FieldValue + ",";
                    //            break;
                    //        }
                    //    }

                    //}
                    if (names.FieldName == "Truck No")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (string.IsNullOrEmpty(txtTruckNo.Text))
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Material")
                    {
                        if (names.IsMandatory1 == true && checkmultiproduct.Checked==false )
                        {
                            if (ddlmaterial.SelectedItem.Value == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Material Classification")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (ddlmc.SelectedItem.Value == "0")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Supplier/customer")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (ddlsupplier.SelectedItem.Value == "0")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Transporter")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (ddltransporter.SelectedItem.Value == "0")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Packing")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (ddlpacking.SelectedItem.Value == "0")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Packing qty")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (txtpackingqty.Text.Trim() == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Challan/Invoice no")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (txtInvoiceNo.Text.Trim() == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Challan weight")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (txtChallanWeight.Text == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "PO /SO/DO no")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (txtPONo.Text == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Remarks")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (txtremarks.Text == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "1st weight")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (FirstWeight.Text == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "2nd weight")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (SecondWeight.Value == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Net weight")
                    {
                        if (names.IsMandatory1 == true)
                        {
                            if (lblNetWt.InnerText == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                }
            }
            if (_FieldName.Length > 0)
            {
                _FieldName = _FieldName.Remove(_FieldName.Length - 1, 1);
            }
            return check == false ? "0" : "1:" + _FieldName;
        }
        private string CheckSecondValidation()
        {
            bool check = false;
            string _FieldName = "";
            IList<DynamicFieldName> model = _transrepo.GetFieldNameByMachine(Session["WBID"].ToString(), Session["PlantID"].ToString());
            if (model.Count > 0)
            {
                foreach (DynamicFieldName names in model)
                {

                    if (names.FieldName == "Trip Id")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (string.IsNullOrEmpty(txtTripId.Text))
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }

                    //if (names.FieldName == "Weighing Type")
                    //{
                    //    if (names.IsMandatory1 == true)
                    //    {
                    //        if (ddlinoutna.SelectedItem.Value == "0")
                    //        {
                    //            check = true;
                    //            _FieldName = _FieldName + names.FieldValue + ",";
                    //            break;
                    //        }
                    //    }

                    //}
                    //if (names.FieldName == "Multi Product")
                    //{
                    //    if (names.IsMandatory1 == true)
                    //    {
                    //        if (!checkmultiproduct.Checked)
                    //        {
                    //            check = true;
                    //            _FieldName = _FieldName + names.FieldValue + ",";
                    //            break;
                    //        }
                    //    }

                    //}
                    if (names.FieldName == "Truck No")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (string.IsNullOrEmpty(txtTruckNo.Text))
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Material")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (ddlmaterial.SelectedItem.Value == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Material Classification")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (ddlmc.SelectedItem.Value == "0")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Supplier/customer")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (ddlsupplier.SelectedItem.Value == "0")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Transporter")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (ddltransporter.SelectedItem.Value == "0")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Packing")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (ddlpacking.SelectedItem.Value == "0")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Packing qty")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (txtpackingqty.Text.Trim() == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Challan/Invoice no")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (txtInvoiceNo.Text.Trim() == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Challan weight")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (txtChallanWeight.Text == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "PO /SO/DO no")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (txtPONo.Text == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Remarks")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (txtremarks.Text == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "1st weight")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (FirstWeight.Text == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "2nd weight")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (SecondWeight.Value == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                    if (names.FieldName == "Net weight")
                    {
                        if (names.IsMandatory2 == true)
                        {
                            if (lblNetWt.InnerText == "")
                            {
                                check = true;
                                _FieldName = _FieldName + names.FieldValue + ",";
                                break;
                            }
                        }

                    }
                }
            }
            if (_FieldName.Length > 0)
            {
                _FieldName = _FieldName.Remove(_FieldName.Length - 1, 1);
            }
            return check == false ? "0" : "1:" + _FieldName;
        }

        protected void ddlpacking_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlpacking.SelectedItem.Value != "0")
                {
                    var vardata = _Packingrepo.Get_PackingByCode(ddlpacking.SelectedItem.Value.ToString());
                    if (vardata != null)
                    {
                        if (vardata.PackingWT != null)
                        {
                            txtpackingqty.Text = vardata.PackingWT.ToString();
                        }
                        if (vardata.PackingUOM != null)
                        {
                            lblPackingUnit.Text = vardata.PackingUOM.ToString();
                        }
                    }
                    else
                    {
                        txtpackingqty.Text = string.Empty;
                        lblPackingUnit.Text = string.Empty;

                    }

                }
            }
            catch { }
            
        }

        protected void ddlpacking_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (ddlpacking.SelectedItem.Value != "0")
            //    {
            //        var vardata = _Packingrepo.Get_PackingByCode(ddlpacking.SelectedItem.Value.ToString());

            //        if (vardata.PackingWT != null)
            //        {
            //            txtpackingqty.Text = vardata.PackingWT.ToString();
            //        }
            //        if (vardata.PackingUOM != null)
            //        {
            //            lblPackingUnit.Text = vardata.PackingUOM.ToString();
            //        }

            //    }
            //}
            //catch { }
        }

        protected void btnLoadUnload_Click(object sender, EventArgs e)
        {
            tblTransaction _trantkt = db.tblTransactions.FirstOrDefault(x => x.TripId == Convert.ToInt32(this.txtTripId.Text.Trim()) && x.WeighbridgeId == Session["WBID"].ToString());

            if (_trantkt != null && _trantkt.SecondWeight != null)
            {
                // _trantkt.print_ticket = "Y";
                _trantkt.TransactionStatus = 2;
                db.SubmitChanges();
            }
        }

        protected void ddlmaterial_SelectedIndexChanged(object sender, EventArgs e)
        {//ddlmc.SelectedValue = "0";
            try
            {
               // BindMaterialClassificationDropdown();
                if (ddlmaterial.SelectedItem.Value != "0")
                {
                    
                    var varDataMatPacking = _materialrepo.Get_tblMaterialByCode(ddlmaterial.SelectedItem.Value.ToString());

                    if (varDataMatPacking.MaterialClassificationCodeId != "")
                    {
                        //ddlmc.Text = ""; 
                        
                        string test= varDataMatPacking.MaterialClassificationCodeId;


                        //ddlmc.Items.FindByValue(test).Selected = true;
                       // ddlmc.SelectedItem.Text  =test;

                        ddlmc.SelectedValue   = test;

                    }
                    else
                    {
                        ddlmc.SelectedValue = "0";
                    }

                    if (varDataMatPacking.PackingCodeId != "")
                    {
                        var vardata = _Packingrepo.Get_PackingByCode(varDataMatPacking.PackingCodeId);
                        ddlpacking.Text = varDataMatPacking.PackingCodeId;
                        if (vardata != null)
                        {
                            if (vardata.PackingWT != null)
                            {
                                txtpackingqty.Text = vardata.PackingWT.ToString();
                            }
                            if (vardata.PackingUOM != null)
                            {
                                lblPackingUnit.Text = vardata.PackingUOM.ToString();
                            }
                        }
                        else
                        {
                            txtpackingqty.Text = string.Empty;
                            lblPackingUnit.Text = string.Empty;
                            ddlpacking.Text = string.Empty;

                        }
                    }
                    else
                    {
                        txtpackingqty.Text = string.Empty;
                        lblPackingUnit.Text = string.Empty;
                        ddlpacking.SelectedValue ="0";
                    }

                }
            }
            catch(Exception ex) {
                ddlmc.SelectedValue = "0";
            }

        }

        protected void txtTruckNo_TextChanged(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(txtTruckNo.Text.Trim()) || txtTruckNo.Text.Trim().Length==0) // && SecondWeight.Value.ToString().Length != 0)
            {
                //SecondDate.Value = string.Empty;
                //SecondTime.Value = string.Empty;
                //SecondWeight.Value = string.Empty;
                //txtFinalWeight.Value = string.Empty;

                //ddlmaterial.SelectedItem.Value = "0";
                //ddlmc.SelectedItem.Value = "0";
                //ddlsupplier.SelectedItem.Value = "0";
                //ddltransporter.SelectedItem.Value = "0";
                //ddlpacking.SelectedItem.Value = "0";
                //txtpackingqty.Text = string.Empty;
                //txtInvoiceNo.Text = string.Empty;
                //txtInvoiceDate.Text = string.Empty;
                //txtChallanWeight.Text = string.Empty;
                //txtPONo.Text = string.Empty;
                //txtPODate.Text = string.Empty;
                //txtremarks.Text = string.Empty;
                Session["SECONDWT_RCD"] = "0";
                Response.Redirect("Manual_Weighment.aspx");
            }
           // takeweight();
        }

        private void adderrorlog(string varerrdesc,string varerrortitle)
        {
            #region Add log to table
            Model_SystemLog log = new Model_SystemLog();
            log.UserId = Session["UserName"].ToString();
            log.LogDate = DateTime.Now;
            log.LogDescription = varerrdesc;
            log.LogTitle = varerrortitle.ToString();
            log.URL = HttpContext.Current.Request.Url.AbsoluteUri;
            logRepo.SaveSystemLog(log);
            #endregion
        }

    }
}
