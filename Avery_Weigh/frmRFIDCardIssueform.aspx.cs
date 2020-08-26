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
//using System.Drawing;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Avery_Weigh
{
    public partial class frmRFIDCardIssueform : System.Web.UI.Page
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DataClasses1DataContext db = new DataClasses1DataContext();
        MaterialRepository _materialrepo = new MaterialRepository();
        SupplierRepository _supplierrepo = new SupplierRepository();
        TransporterRepository _transrepo = new TransporterRepository();
        PackingRepository _Packingrepo = new PackingRepository();
        TransactionRepository _transactionRepo = new TransactionRepository();
        UserMasterRepository umRepo = new UserMasterRepository();
        private Socket s3;
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        bool Flag_FirstWt_Records = false;
        bool Flag_SecondWt_Records = false;

        string checkdisplayMessage = string.Empty;

        public string mySrc2;
        public string mySrc1;
        public string mySrc3;



        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["DIOIP"] != null && Session["SENSORACTIVE"] != null && Session["ALPHADISPLAYIP1"] != null && Session["ALPHADISPLAYACTIVE"] != null)
            {
                if (!IsPostBack)
                {
                   
                    bindFormLabels();

                    getuserAccess();
                   
                    CheckAndBindTripId();
                    BindMaterialDropDown();
                    BindMaterialClassificationDropdown();
                    BindSupplierDropdown();
                    BindTransporterDropdown();
                    BindPackingDropDown();

                    FillGrid();
                    //livevideoofcamera1();
                    //this.txtMsgInfo.Text = "Vehicle is not proper position at Weighbridge.";
                }

            }
            else
            {
                Response.Redirect("~/Login.aspx");
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
                                break;
                            case "Truck No":
                                lbltruckno.InnerText = names.FieldValue;
                                break;
                            case "Material":
                                lblMaterial.InnerText = names.FieldValue;
                                break;
                            case "Material Classification":
                                lblMC.InnerText = names.FieldValue;
                                break;
                            case "Supplier/customer":
                                lblsupplier.InnerText = names.FieldValue;
                                break;
                            case "Transporter":
                                lblTransporter.InnerText = names.FieldValue;
                                break;
                            case "Packing":
                                lblPacking.InnerText = names.FieldValue;
                                break;
                            case "Packing qty":
                                lblPackingQty.InnerText = names.FieldValue;
                                break;
                            case "Challan/Invoice no":
                                lblChallanNo.InnerText = names.FieldValue;
                                break;
                            case "Challan weight":
                                lblChallanwt.InnerText = names.FieldValue;
                                break;
                            case "PO /SO/DO no":
                                lblPOSODONo.InnerText = names.FieldValue;
                                break;
                            case "Remarks":
                                lblRemrks.InnerText = names.FieldValue;
                                break;
                            //case "1st weight":
                            //    lblFirstWt.InnerText = names.FieldValue;
                            //    break;
                            //case "2nd weight":
                            //    lbl2ndWt.InnerText = names.FieldValue;
                            //    break;
                            //case "Net weight":
                            //    lblNetWt.InnerText = names.FieldValue;
                            //    break;
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
                    WeighMenu.Style.Add("display", "none");
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
            Int32 varMaxNo = 0;

            var VARMAX2 = _transactionRepo.GetTripId().ToString();

            string constr = ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT max(tripid) as TRIPID FROM tblRFIDCARDDATA"))
                {
                    cmd.CommandType = CommandType.Text;
                    //cmd.Parameters.AddWithValue("@RFIDCARDNO", txtRFIDCARDNO.Text);
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())

                    {
                        varMaxNo =Convert.ToInt32(dr["TRIPID"].ToString());
                    }
                    else
                    {
                        varMaxNo = 0;
                    }
                    con.Close();
                }
            }

            if (varMaxNo > Convert.ToInt32(VARMAX2))
                this.txtTripId.Text = (varMaxNo + 1).ToString();
            else
                this.txtTripId.Text = (Convert.ToInt32(VARMAX2) + 1).ToString();

        }

        public tblMachineWorkingParameter getLoggedInUserWeigh()
        {
            string machineId = Session["WBID"].ToString();
            tblMachineWorkingParameter mwparameter = _transactionRepo.getMachineWorkingParameters(machineId);
            return mwparameter;
        }

        public string GetIPfromServerPort()
        {
            int portno = 0;
            tblMachineWorkingParameter tbl = getLoggedInUserWeigh();
            try
            {
                portno = Convert.ToInt32(tbl.PortNo);

            }
            catch { }
            StringBuilder sb = new StringBuilder();
            IPAddress ip = IPAddress.Parse("172.31.40.159");
            TcpListener server = new TcpListener(ip, portno);
            TcpClient client = default(TcpClient);
            string msg = "0";
            try
            {
                server.Start();
                //lblname.Text = sb.ToString();
                client = server.AcceptTcpClient();
                byte[] receivedBuffer = new byte[1024];
                NetworkStream stream = client.GetStream();
                stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                msg = Encoding.ASCII.GetString(receivedBuffer, 0, receivedBuffer.Length);
                Console.Read();
                server.Stop();
            }
            catch (Exception ex)
            {
                server.Stop();
            }

            return msg;
        }
        public string GetWeightFromIP()
        {
            string output = "No weight";
            byte[] data = new byte[10];

            //IPHostEntry iphostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAdress = IPAddress.Parse("192.168.1.50");
            IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, 1);

            Socket client = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client.Connect(ipEndpoint);

                //Console.WriteLine("Socket created to {0}", client.RemoteEndPoint.ToString());
                //byte[] sendmsg = Encoding.ASCII.GetBytes("This is from Client\n");
                //int n = client.Send(sendmsg);

                int m = client.Receive(data);
                string _output = Encoding.ASCII.GetString(data);
                output = _output.Trim().Split(' ')[_output.Trim().Split(' ').Length - 2];/* + _output.Trim().Split(' ')[_output.Trim().Split(' ').Length - 1]*/;
                m = client.Receive(data);

                client.Shutdown(SocketShutdown.Both);
                client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return output;
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
                strIndicatorPortNo = tbl.PortNo;
                strIndicatorIPAddress = tbl.IPPort;

            }
            catch { }

            s3 = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp);

            IPAddress hostadd = IPAddress.Parse(strIndicatorIPAddress);
            IPEndPoint EPhost = new IPEndPoint(hostadd, Convert.ToInt32(strIndicatorPortNo));
            string weight = "";
            s3.ReceiveTimeout = 500;
            try
            {

                s3.Connect(EPhost);

                if (s3.Connected)
                {
                    try
                    {
                        Byte[] sbyte1 = new Byte[] { 0x5 };

                        s3.Send(sbyte1);
                        System.Threading.Thread.Sleep(500);
                    }
                    catch { }

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


        //protected void timer1_Tick(object sender, EventArgs e)
        //{
        //    //Header.FindControl["Image5"]
        //    // livevideoofcamera1();

        //    //UserControl uc = this.Page.FindControl("Header") as UserControl;
        //    //string url = (uc.FindControl("Image5") as Image).ImageUrl;
        //    ////(uc.FindControl("Image5") as Image).ImageUrl = "";
        //    //(uc.FindControl("Image5") as Image).ImageUrl = "~/images/type6/reader_grey.png";


        //    //this.timer2_Tick(null, null);

        //    //UserControl UC;
        //    //System.Web.UI.WebControls.Image imgUC;
        //    //UC = (UserControl)Page.FindControl("Header");
        //    //imgUC = (System.Web.UI.WebControls.Image)UC.FindControl("Image5");
        //    //imgUC.ImageUrl = "~/images/type6/reader_grey.png";

        //    //((this.Master.FindControl("Header") as UserControl).FindControl("Image5") as Image).ImageUrl = "~/images/type6/reader_grey.png";


        //    //Enable this code if weight machine connected on local client machine 
        //    //string Weight = GetWeightFromIP();  //comented by ss and added new code

        //    string Weight = GetWeightFromTCPIP_SS();

        //    //Enable this code if code is run on public server 
        //    //string Weight = GetIPfromServerPort();
        //    string TruckNo = txtTruckNo.Text.Trim().ToUpper();
        //    RuntimeWeight.Text = Weight.Trim();
        //    //check truck is already in pending record or not
        //    if (!_transactionRepo.checkTruckIsPendingOrNot(TruckNo))
        //    {
        //        //check Truck trip is saved under transaction file and id checked first weight and date time will bot update.
        //        if (!_transactionRepo.checkTruckTripClosed(TruckNo))
        //        {
        //            FirstWeight.Text = Weight.Trim();
        //            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        //            FirstDate.Value = indianTime.ToString("MM/dd/yyyy");
        //            FirstTime.Value = indianTime.ToString("HH:mm:ss tt");
        //        }
        //    }
        //    else
        //    {
        //        tblTransaction tbltrans = _transactionRepo.getPendingTransactionById(TruckNo);
        //        if (tbltrans != null)
        //        {
        //            FirstWeight.Text = tbltrans.FirstWeight.ToString();
        //            FirstDate.Value = tbltrans.FirstWtDateTime.Value.ToString("MM/dd/yyyy");
        //            FirstTime.Value = tbltrans.FirstWtDateTime.Value.ToString("HH:mm:ss tt");
        //        }

        //        //If truck in on pending record and need to take next weight
        //        SecondWeight.Value = Weight;
        //        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        //        SecondDate.Value = indianTime.ToString("MM/dd/yyyy");
        //        SecondTime.Value = indianTime.ToString("HH:mm:ss tt");
        //        if (ddlinoutna.SelectedItem.Value == "0" || ddlinoutna.SelectedItem.Value == "2")
        //        {
        //            try
        //            {
        //                decimal FinalWeight = Math.Abs(Convert.ToDecimal(SecondWeight.Value) - Convert.ToDecimal(FirstWeight.Text));
        //                txtFinalWeight.Value = FinalWeight.ToString();
        //            }
        //            catch { }
        //        }
        //        else
        //        {
        //            try
        //            {
        //                decimal FinalWeight = Convert.ToDecimal(FirstWeight.Text) - Convert.ToDecimal(SecondWeight.Value);
        //                txtFinalWeight.Value = FinalWeight.ToString();
        //            }
        //            catch { }
        //        }
        //    }


        //    // for sensor add

        //    //if (Session["DIOIP"].ToString() != "0" && Session["SENSORACTIVE"].ToString() == "Y")
        //    //{

        //    //    var varnew = new Avery_Weigh.AveryService.adam();
        //    //    varnew.RefreshDIO(Session["DIOIP"].ToString(), Session["DIOPORTNO"].ToString());
        //    //    UserControl uc = this.Page.FindControl("Header") as UserControl;


        //    //    if (Session["SENSORSA"].ToString() == "0")
        //    //    {

        //    //        //((this.Master.FindControl("Header") as UserControl).FindControl("SensorImageBtn1") as ImageButton).ImageUrl = "~/images/type6/reader_grey.png";
        //    //        string url = (uc.FindControl("SensorImageBtn1") as ImageButton).ImageUrl;
        //    //        if (url != "~/images/type6/reader_red.png")
        //    //            (uc.FindControl("SensorImageBtn1") as ImageButton).ImageUrl = "~/images/type6/reader_red.png";

        //    //    }
        //    //    else if (Session["SENSORSA"].ToString() == "1")
        //    //    {

        //    //        //UserControl uc = this.Page.FindControl("Header") as UserControl;
        //    //        string url = (uc.FindControl("SensorImageBtn1") as ImageButton).ImageUrl;
        //    //        if (url != "~/images/type6/reader_green.png")
        //    //            (uc.FindControl("SensorImageBtn1") as ImageButton).ImageUrl = "~/images/type6/reader_green.png";

        //    //    }


        //    //    if (Session["SENSORSB"].ToString() == "0")
        //    //    {

        //    //        //UserControl uc = this.Page.FindControl("Header") as UserControl;
        //    //        string url = (uc.FindControl("SensorImageBtn2") as ImageButton).ImageUrl;
        //    //        if (url != "~/images/type6/reader_red.png")
        //    //            (uc.FindControl("SensorImageBtn2") as ImageButton).ImageUrl = "~/images/type6/reader_red.png";

        //    //    }
        //    //    else if (Session["SENSORSB"].ToString() == "1")
        //    //    {

        //    //        //UserControl uc = this.Page.FindControl("Header") as UserControl;
        //    //        string url = (uc.FindControl("SensorImageBtn2") as ImageButton).ImageUrl;
        //    //        if (url != "~/images/type6/reader_green.png")
        //    //            (uc.FindControl("SensorImageBtn2") as ImageButton).ImageUrl = "~/images/type6/reader_green.png";

        //    //    }



        //    //    if (Session["SENSORSC"].ToString() == "0")
        //    //    {

        //    //        //UserControl uc = this.Page.FindControl("Header") as UserControl;
        //    //        string url = (uc.FindControl("SensorImageBtn3") as ImageButton).ImageUrl;
        //    //        if (url != "~/images/type6/reader_red.png")
        //    //            (uc.FindControl("SensorImageBtn3") as ImageButton).ImageUrl = "~/images/type6/reader_red.png";

        //    //    }
        //    //    else if (Session["SENSORSC"].ToString() == "1")
        //    //    {

        //    //        //UserControl uc = this.Page.FindControl("Header") as UserControl;
        //    //        string url = (uc.FindControl("SensorImageBtn3") as ImageButton).ImageUrl;
        //    //        if (url != "~/images/type6/reader_green.png")
        //    //            (uc.FindControl("SensorImageBtn3") as ImageButton).ImageUrl = "~/images/type6/reader_green.png";

        //    //    }
        //    //}


        //    //this.timer2_Tick(null, null);



        //}

        //protected void RuntimeWeight_Click(object sender, EventArgs e)
        //{
        //    if (timer1.Enabled == true)
        //    {
        //        FirstWeight.Text = RuntimeWeight.Text;
        //        FirstDate.Value = DateTime.Now.ToShortDateString();
        //        FirstTime.Value = DateTime.Now.TimeOfDay.ToString();
        //    }
        //}

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
            ddlsupplier.DataValueField = "Id";
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
            //updateimage();
            AddRFIDData_USED_SP();
            //AddRFIDData();
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

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            string strTripId = _transactionRepo.GetTripId(Session["WBID"].ToString()).ToString();
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

            }
        }

        //protected void timer2_Tick(object sender, EventArgs e)
        //{

        //    this.txtMsgInfo.Text = "Timer Vehicle is not proper position at Weighbridge.";

        //    if (Session["DIOIP"].ToString() != "0" && Session["SENSORACTIVE"].ToString() == "Y")
        //    {



        //        // for sensor add


        //        var varnew = new Avery_Weigh.AveryService.adam();
        //        varnew.RefreshDIO(Session["DIOIP"].ToString(), Session["DIOPORTNO"].ToString());
        //        //this.RefreshDIO_New(Session["DIOIP"].ToString(), Session["DIOPORTNO"].ToString());
        //        UserControl uc = this.Page.FindControl("Header") as UserControl;


        //        if (Session["SENSORSA"].ToString() == "0")
        //        {

        //            //((this.Master.FindControl("Header") as UserControl).FindControl("SensorImageBtn1") as ImageButton).ImageUrl = "~/images/type6/reader_grey.png";
        //            string url = (uc.FindControl("SensorImageBtn1") as ImageButton).ImageUrl;
        //            if (url != "~/images/type6/reader_red.png")
        //                (uc.FindControl("SensorImageBtn1") as ImageButton).ImageUrl = "~/images/type6/reader_red.png";

        //        }
        //        else if (Session["SENSORSA"].ToString() == "1")
        //        {

        //            //UserControl uc = this.Page.FindControl("Header") as UserControl;
        //            string url = (uc.FindControl("SensorImageBtn1") as ImageButton).ImageUrl;
        //            if (url != "~/images/type6/reader_green.png")
        //                (uc.FindControl("SensorImageBtn1") as ImageButton).ImageUrl = "~/images/type6/reader_green.png";

        //        }


        //        if (Session["SENSORSB"].ToString() == "0")
        //        {

        //            //UserControl uc = this.Page.FindControl("Header") as UserControl;
        //            string url = (uc.FindControl("SensorImageBtn2") as ImageButton).ImageUrl;
        //            if (url != "~/images/type6/reader_red.png")
        //                (uc.FindControl("SensorImageBtn2") as ImageButton).ImageUrl = "~/images/type6/reader_red.png";

        //        }
        //        else if (Session["SENSORSB"].ToString() == "1")
        //        {

        //            //UserControl uc = this.Page.FindControl("Header") as UserControl;
        //            string url = (uc.FindControl("SensorImageBtn2") as ImageButton).ImageUrl;
        //            if (url != "~/images/type6/reader_green.png")
        //                (uc.FindControl("SensorImageBtn2") as ImageButton).ImageUrl = "~/images/type6/reader_green.png";

        //        }



        //        if (Session["SENSORSC"].ToString() == "0")
        //        {

        //            //UserControl uc = this.Page.FindControl("Header") as UserControl;
        //            string url = (uc.FindControl("SensorImageBtn3") as ImageButton).ImageUrl;
        //            if (url != "~/images/type6/reader_red.png")
        //                (uc.FindControl("SensorImageBtn3") as ImageButton).ImageUrl = "~/images/type6/reader_red.png";

        //        }
        //        else if (Session["SENSORSC"].ToString() == "1")
        //        {

        //            //UserControl uc = this.Page.FindControl("Header") as UserControl;
        //            string url = (uc.FindControl("SensorImageBtn3") as ImageButton).ImageUrl;
        //            if (url != "~/images/type6/reader_green.png")
        //                //(uc.FindControl("SensorImageBtn3") as ImageButton).ImageUrl = String.Empty;
        //                (uc.FindControl("SensorImageBtn3") as ImageButton).ImageUrl = "~/images/type6/reader_green.png";

        //        }



        //        //FOR DISPLAY MESSAGE
        //        //if (Session["ALPHADISPLAYIP1"].ToString() != "0" && Session["ALPHADISPLAYACTIVE"].ToString() == "Y")
        //        //{
        //        //    var varDisplay1 = new Avery_Weigh.AveryService.alphadisplay();
        //        //    if (Session["SENSORSA"].ToString() == "0" && Session["SENSORSB"].ToString() == "1" && Session["SENSORSC"].ToString() == "0") // ) this.varSA == false && this.varSB == true && this.varSC == false)
        //        //    {
        //        //         this.txtMsgInfo.Text = "Vehicle has reached the weighbridge.Take Weight.";
        //        //        if (this.Flag_FirstWt_Records == false && this.Flag_SecondWt_Records == false && Session["FIRSTWT_RCD"].ToString() == "0" && Session["SECONDWT_RCD"].ToString() == "0")
        //        //        {
        //        //            if (checkdisplayMessage == string.Empty || checkdisplayMessage != "STOPRT")
        //        //            {
        //        //                varDisplay1.DecimaltoHex("STOP", "RT");
        //        //                checkdisplayMessage = "STOPRT";
        //        //            }
        //        //            //this.PlayFile(Application.StartupPath + "\\WeighInProcess.mp3");
        //        //            //Session["FIRSTWT_RCD"] = "0";
        //        //            //Session["SECONDWT_RCD"] = "0";
        //        //        }
        //        //        if (this.Flag_FirstWt_Records == true || Session["FIRSTWT_RCD"].ToString() == "1")
        //        //        {
        //        //            if (checkdisplayMessage == string.Empty || checkdisplayMessage != this.FirstWeight.Text.Trim().ToString() + " EXIT"+"G")
        //        //            {
        //        //                varDisplay1.DecimaltoHex(this.FirstWeight.Text.Trim().ToString() + " EXIT", "G");
        //        //                checkdisplayMessage = this.FirstWeight.Text.Trim().ToString() + " EXIT" + "G";
        //        //            }
        //        //            //this.PlayFile(Application.StartupPath + "\\CanMoveOut.mp3");
        //        //        }
        //        //        else if (this.Flag_SecondWt_Records == true || Session["SECONDWT_RCD"].ToString() == "1")
        //        //        {
        //        //            if (checkdisplayMessage == string.Empty || checkdisplayMessage != this.SecondWeight.Value.Trim().ToString() + " EXIT" + "G")
        //        //            {
        //        //                varDisplay1.DecimaltoHex(this.SecondWeight.Value.Trim().ToString() + " EXIT", "G");
        //        //                checkdisplayMessage = this.SecondWeight.Value.Trim().ToString() + " EXIT" + "G";
        //        //            }
        //        //            //this.PlayFile(Application.StartupPath + "\\CanMoveOut.mp3");
        //        //        }

        //        //    }
        //        //    else if ((Session["SENSORSA"].ToString() == "1" && Session["SENSORSB"].ToString() == "1") || (Session["SENSORSC"].ToString() == "1" && Session["SENSORSB"].ToString() == "1") || Session["SENSORSA"].ToString() == "1" || Session["SENSORSC"].ToString() == "1")
        //        //    {
        //        //        this.txtMsgInfo.Text = "Vehicle is not proper position at Weighbridge.";

        //        //        if (checkdisplayMessage == string.Empty || checkdisplayMessage != "nullYT")
        //        //        {
        //        //            varDisplay1.DecimaltoHex(null, "YT");
        //        //            checkdisplayMessage = "nullYT";
        //        //        }


        //        //        //if (this.Flag_FirstWt_Records == false && this.Flag_SecondWt_Records == false)
        //        //        //    this.PlayFile(Application.StartupPath + "\\CanMove.mp3");
        //        //        }

        //        //    else if (Session["SENSORSA"].ToString() == "0" && Session["SENSORSB"].ToString() == "0" && Session["SENSORSC"].ToString() == "0")
        //        //    {

        //        //        if (RuntimeWeight.Text.ToString().ToUpper() != "NO WEIGHT")
        //        //        {
        //        //            this.txtMsgInfo.Text = "Waiting for Vehicle.";
        //        //            if (checkdisplayMessage == string.Empty || checkdisplayMessage != "nullGT")
        //        //            {
        //        //                varDisplay1.DecimaltoHex(null, "GT");
        //        //                checkdisplayMessage = "nullGT";
        //        //            }
        //        //            this.Flag_FirstWt_Records = false;
        //        //            this.Flag_SecondWt_Records = false;
        //        //            Session["FIRSTWT_RCD"] = "0";
        //        //            Session["SECONDWT_RCD"] = "0";
        //        //        }
        //        //        else if (RuntimeWeight.Text.ToString().ToUpper() == "NO WEIGHT")
        //        //        {
        //        //            this.txtMsgInfo.Text = "No Weight on Weighbridge.";
        //        //            if (checkdisplayMessage == string.Empty || checkdisplayMessage != "nullRT")
        //        //            {
        //        //                varDisplay1.DecimaltoHex(null, "RT");
        //        //                checkdisplayMessage = "nullRT";
        //        //            }

        //        //        }

        //        //    }
        //        //}
        //    }
        //    //timer2.Enabled = true;


        //}

        protected void timer3_Tick(object sender, EventArgs e)
        {



            //FOR DISPLAY MESSAGE
            //if (Session["ALPHADISPLAYIP1"].ToString() != "0" && Session["ALPHADISPLAYACTIVE"].ToString() == "Y")
            //{
            //    var varDisplay1 = new Avery_Weigh.AveryService.alphadisplay();
            //    if (Session["SENSORSA"].ToString() == "0" && Session["SENSORSB"].ToString() == "1" && Session["SENSORSC"].ToString() == "0") // ) this.varSA == false && this.varSB == true && this.varSC == false)
            //    {
            //        this.txtMsgInfo.Text = "Vehicle has reached the weighbridge.Take Weight.";
            //        if (this.Flag_FirstWt_Records == false && this.Flag_SecondWt_Records == false && Session["FIRSTWT_RCD"].ToString() == "0" && Session["SECONDWT_RCD"].ToString() == "0")
            //        {
            //            if (checkdisplayMessage == string.Empty || checkdisplayMessage != "STOPRT")
            //            {
            //                varDisplay1.DecimaltoHex("STOP", "RT");
            //                checkdisplayMessage = "STOPRT";
            //            }
            //            //this.PlayFile(Application.StartupPath + "\\WeighInProcess.mp3");
            //            //Session["FIRSTWT_RCD"] = "0";
            //            //Session["SECONDWT_RCD"] = "0";
            //        }
            //        if (this.Flag_FirstWt_Records == true || Session["FIRSTWT_RCD"].ToString() == "1")
            //        {
            //            if (checkdisplayMessage == string.Empty || checkdisplayMessage != this.FirstWeight.Text.Trim().ToString() + " EXIT" + "G")
            //            {
            //                varDisplay1.DecimaltoHex(this.FirstWeight.Text.Trim().ToString() + " EXIT", "G");
            //                checkdisplayMessage = this.FirstWeight.Text.Trim().ToString() + " EXIT" + "G";
            //            }
            //            //this.PlayFile(Application.StartupPath + "\\CanMoveOut.mp3");
            //        }
            //        else if (this.Flag_SecondWt_Records == true || Session["SECONDWT_RCD"].ToString() == "1")
            //        {
            //            if (checkdisplayMessage == string.Empty || checkdisplayMessage != this.SecondWeight.Value.Trim().ToString() + " EXIT" + "G")
            //            {
            //                varDisplay1.DecimaltoHex(this.SecondWeight.Value.Trim().ToString() + " EXIT", "G");
            //                checkdisplayMessage = this.SecondWeight.Value.Trim().ToString() + " EXIT" + "G";
            //            }
            //            //this.PlayFile(Application.StartupPath + "\\CanMoveOut.mp3");
            //        }

            //    }
            //    else if ((Session["SENSORSA"].ToString() == "1" && Session["SENSORSB"].ToString() == "1") || (Session["SENSORSC"].ToString() == "1" && Session["SENSORSB"].ToString() == "1") || Session["SENSORSA"].ToString() == "1" || Session["SENSORSC"].ToString() == "1")
            //    {
            //        this.txtMsgInfo.Text = "Vehicle is not proper position at Weighbridge.";

            //        if (checkdisplayMessage == string.Empty || checkdisplayMessage != "nullYT")
            //        {
            //            varDisplay1.DecimaltoHex(null, "YT");
            //            checkdisplayMessage = "nullYT";
            //        }


            //        //if (this.Flag_FirstWt_Records == false && this.Flag_SecondWt_Records == false)
            //        //    this.PlayFile(Application.StartupPath + "\\CanMove.mp3");
            //    }

            //    else if (Session["SENSORSA"].ToString() == "0" && Session["SENSORSB"].ToString() == "0" && Session["SENSORSC"].ToString() == "0")
            //    {

            //        if (RuntimeWeight.Text.ToString().ToUpper() != "NO WEIGHT")
            //        {
            //            this.txtMsgInfo.Text = "Waiting for Vehicle.";
            //            if (checkdisplayMessage == string.Empty || checkdisplayMessage != "nullGT")
            //            {
            //                varDisplay1.DecimaltoHex(null, "GT");
            //                checkdisplayMessage = "nullGT";
            //            }
            //            this.Flag_FirstWt_Records = false;
            //            this.Flag_SecondWt_Records = false;
            //            Session["FIRSTWT_RCD"] = "0";
            //            Session["SECONDWT_RCD"] = "0";
            //        }
            //        else if (RuntimeWeight.Text.ToString().ToUpper() == "NO WEIGHT")
            //        {
            //            this.txtMsgInfo.Text = "No Weight on Weighbridge.";
            //            if (checkdisplayMessage == string.Empty || checkdisplayMessage != "nullRT")
            //            {
            //                varDisplay1.DecimaltoHex(null, "RT");
            //                checkdisplayMessage = "nullRT";
            //            }

            //        }

            //    }
            //}



        }

        private void snapshotofcamera1(string varFileName)
        {
            // for snap shot
            if (Session["CAMERAIP1"].ToString() == "0" && Session["CAMERAACTIVE"].ToString() == "Y")
                return;

            if (Session["CAMERAIP1"].ToString() != "0" && Session["CAMERAACTIVE"].ToString() == "N")
                return;


            byte[] buffer = new byte[1000000];
            int read, total = 0;
            byte[] m_Bitmap = null;

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://" + Session["CAMERAIP1"] + "/cgi-bin/snapshot.cgi?channel=1");
                req.Method = "POST";
                //req.Timeout = 500;
                NetworkCredential cred = new NetworkCredential(Session["CAMERAUSER1"].ToString(), Session["CAMERAPWD1"].ToString());
                req.Credentials = cred;
                WebResponse resp = req.GetResponse();
                // get response stream

                Stream stream = resp.GetResponseStream();
                // read data from stream

                //ListView1.Page = stream;
                while ((read = stream.Read(buffer, total, 1000)) != 0)
                {
                    total += read;
                }
                // get bitmap

                Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer, 0, total));
                Bitmap bmp1 = new Bitmap(bmp, 1280, 720);

                bmp1.Save(Server.MapPath("~/Camera_Snapshot/" + varFileName + ".bmp"), System.Drawing.Imaging.ImageFormat.Jpeg);

                //ImgCamera1.ImageUrl = "~/Camera_Snapshot/" + varFileName + ".jpg";

                if (File.Exists(Server.MapPath("~/Camera_Snapshot/" + varFileName + ".bmp")))
                {
                    FileStream fs = new FileStream(Server.MapPath("~/Camera_Snapshot/" + varFileName + ".bmp"), FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)br.BaseStream.Length;
                    m_Bitmap = new byte[length];
                    m_Bitmap = br.ReadBytes(length);
                    Session["IMAGE1"] = m_Bitmap;
                    br.Close();
                    fs.Close();
                }
                else
                {
                    FileStream fs = new FileStream(Server.MapPath("~/Camera_Snapshot/" + "NoImage.bmp"), FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)br.BaseStream.Length;
                    m_Bitmap = new byte[length];
                    m_Bitmap = br.ReadBytes(length);
                    Session["IMAGE1"] = m_Bitmap;
                    br.Close();
                    fs.Close();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Grab Error:" + ex, "Error!!");
            }
        }

        private void snapshotofcamera2(string varFileName)
        {
            // for snap shot
            //Session["CAMERAIP2"] = "192.168.1.250";
            if (Session["CAMERAIP2"].ToString() == "0" && Session["CAMERAACTIVE"].ToString() == "Y")
                return;

            if (Session["CAMERAIP2"].ToString() != "0" && Session["CAMERAACTIVE"].ToString() == "N")
                return;


            byte[] buffer1 = new byte[1000000];
            int read1;
            int total1 = 0;
            byte[] m_Bitmap = null;
            try
            {
                HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create("http://" + Session["CAMERAIP2"] + "/cgi-bin/snapshot.cgi?channel=1");
                req1.Method = "POST";
                //req.Timeout = 500;
                NetworkCredential cred1 = new NetworkCredential(Session["CAMERAUSER2"].ToString(), Session["CAMERAPWD2"].ToString());
                req1.Credentials = cred1;
                WebResponse resp1 = req1.GetResponse();
                // get response stream

                Stream stream1 = resp1.GetResponseStream();
                // read data from stream

                //ListView1.Page = stream;
                while ((read1 = stream1.Read(buffer1, total1, 1000)) != 0)
                {
                    total1 += read1;
                }
                // get bitmap

                Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer1, 0, total1));
                Bitmap bmp1 = new Bitmap(bmp, 1280, 720);

                bmp1.Save(Server.MapPath("~/Camera_Snapshot/" + varFileName + ".bmp"), System.Drawing.Imaging.ImageFormat.Jpeg);

                //ImgCamera2.ImageUrl = "~/Camera_Snapshot/" + varFileName + ".jpg";

                if (File.Exists(Server.MapPath("~/Camera_Snapshot/" + varFileName + ".bmp")))
                {
                    FileStream fs = new FileStream(Server.MapPath("~/Camera_Snapshot/" + varFileName + ".bmp"), FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)br.BaseStream.Length;
                    m_Bitmap = new byte[length];
                    m_Bitmap = br.ReadBytes(length);
                    Session["IMAGE2"] = m_Bitmap;
                    br.Close();
                    fs.Close();
                }
                else
                {
                    FileStream fs = new FileStream(Server.MapPath("~/Camera_Snapshot/" + "NoImage.bmp"), FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)br.BaseStream.Length;
                    m_Bitmap = new byte[length];
                    m_Bitmap = br.ReadBytes(length);

                    Session["IMAGE2"] = m_Bitmap;

                    br.Close();
                    fs.Close();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Grab Error:" + ex, "Error!!");
            }
        }

        private void livevideoofcamera1()
        {
            // for snap shot
            if (Session["CAMERAIP1"].ToString() != "0" && Session["CAMERAACTIVE"].ToString() == "Y")
            {
                mySrc1 = "http://" + Session["CAMERAUSER1"].ToString() + ":" + Session["CAMERAPWD1"].ToString() + "@" + Session["CAMERAIP1"].ToString() + "/cgi-bin/mjpg/video.cgi?channel=1&subtype=0";
            }


            if (Session["CAMERAIP2"].ToString() != "0" && Session["CAMERAACTIVE"].ToString() == "Y")
            {
                mySrc2 = "http://" + Session["CAMERAUSER2"].ToString() + ":" + Session["CAMERAPWD2"].ToString() + "@" + Session["CAMERAIP2"].ToString() + "/cgi-bin/mjpg/video.cgi?channel=1&subtype=1";
            }

            if (Session["CAMERAIP1"].ToString() != "0" && Session["CAMERAACTIVE"].ToString() == "Y")
            {
                mySrc3 = "http://" + Session["CAMERAUSER1"].ToString() + ":" + Session["CAMERAPWD1"].ToString() + "@" + Session["CAMERAIP1"].ToString() + "/cgi-bin/mjpg/video.cgi?channel=1&subtype=0";
            }


        }

        protected void txtRFIDCARDNO_TextChanged(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM tblTagUid where RFIDCARDNO=@RFIDCARDNO"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@RFIDCARDNO",txtRFIDCARDNO.Text);
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if(dr.Read())

                    {
                        txtRFIDCardUID.Text = dr["RFIDTAGUID"].ToString();
                    }
                    else
                    {
                        txtRFIDCardUID.Text = String.Empty;
                    }
                    con.Close();
                }
            }
          
        }

        public void AddRFIDData_USED_SP()
        {
            string constr = ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SP_RFIDCARD_MASTER_tab", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(txtID.Text));
                    //cmd.Parameters.AddWithValue("@NAME", txtName.Text);
                    //cmd.Parameters.AddWithValue("@ACTIVE", a);
                    //cmd.Parameters.AddWithValue("@CREATEDDATE", txtDate.Text);

                    cmd.Parameters.AddWithValue("@RFIDCARDNO", this.txtRFIDCARDNO.Text.Trim());
                    cmd.Parameters.AddWithValue("@RFIDTAGUID", this.txtRFIDCardUID.Text.Trim());
                    cmd.Parameters.AddWithValue("@TripId", this.txtTripId.Text.Trim());
                    cmd.Parameters.AddWithValue("@TripType", this.ddlinoutna.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@IsMultiProduct", ddlMultiproduct.SelectedValue);

                    cmd.Parameters.AddWithValue("@GateEntryNo", this.txtgateentryno.Text.Trim());

                    cmd.Parameters.AddWithValue("@TruckNo", this.txtTruckNo.Text.Trim());
                    try
                    {
                        if (ddlmaterial.SelectedItem.Value == "0")
                        {
                            cmd.Parameters.AddWithValue("@MaterialCode", ddlmaterial.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@MaterialName", "");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@MaterialCode", ddlmaterial.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@MaterialName", ddlmaterial.SelectedItem.Text.Split('(')[0].Replace("(", ""));
                    }
                    }
                    catch { }

                    //cmd.Parameters.AddWithValue("@MaterialCode", this.txtRFIDCARDNO.Text.Trim());

                    //cmd.Parameters.AddWithValue("@MaterialName", this.txtRFIDCARDNO.Text.Trim());

                    try
                    {
                        if (ddlmc.SelectedItem.Value == "0")
                        {
                            cmd.Parameters.AddWithValue("@MaterialCalssificationCode", ddlmc.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@MaterialClassificationName", "");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@MaterialCalssificationCode", ddlmc.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@MaterialClassificationName", ddlmc.SelectedItem.Text.Split('(')[0].Replace("(", ""));
                        }
                    }
                    catch { }

                    //cmd.Parameters.AddWithValue("@MaterialCalssificationCode", this.txtRFIDCARDNO.Text.Trim());

                    //cmd.Parameters.AddWithValue("@MaterialClassificationName", this.txtRFIDCARDNO.Text.Trim());

                    try
                    {
                        if (ddlsupplier.SelectedItem.Value == "0")
                        {
                            cmd.Parameters.AddWithValue("@SupplierCode", ddlsupplier.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@SupplierName", "");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@SupplierCode", ddlsupplier.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@SupplierName", ddlsupplier.SelectedItem.Text.Split('(')[0].Replace("(", ""));
                        }
                    }
                    catch { }

                    //cmd.Parameters.AddWithValue("@SupplierCode", this.txtRFIDCARDNO.Text.Trim());

                    //cmd.Parameters.AddWithValue("@SupplierName", this.txtRFIDCARDNO.Text.Trim());

                    try
                    {
                        if (ddltransporter.SelectedItem.Value == "0")
                        {
                            cmd.Parameters.AddWithValue("@TransporterCode", ddltransporter.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@TransporterName", "");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@TransporterCode", ddltransporter.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@TransporterName", ddltransporter.SelectedItem.Text.Split('(')[0].Replace("(", ""));
                        }
                    }
                    catch { }

                    //cmd.Parameters.AddWithValue("@TransporterCode", this.txtRFIDCARDNO.Text.Trim());

                    //cmd.Parameters.AddWithValue("@TransporterName", this.txtRFIDCARDNO.Text.Trim());

                    try
                    {
                        if (ddlpacking.SelectedItem.Value == "0")
                        {
                            cmd.Parameters.AddWithValue("@PackingCode", ddlpacking.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@PackingName", "");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@PackingCode", ddlpacking.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@PackingName", ddlpacking.SelectedItem.Text.Split('(')[0].Replace("(", ""));
                        }
                    }
                    catch { }

                    //cmd.Parameters.AddWithValue("@PackingCode", this.txtRFIDCARDNO.Text.Trim());

                    //cmd.Parameters.AddWithValue("@PackingName", this.txtRFIDCARDNO.Text.Trim());

                    cmd.Parameters.AddWithValue("@PackingQty", this.txtpackingqty.Text.Trim());
                    cmd.Parameters.AddWithValue("@ChallanNo", this.txtInvoiceNo.Text.Trim());

                    cmd.Parameters.AddWithValue("@ChallanDate", this.txtInvoiceDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@ChallanWeight", this.txtChallanWeight.Text.Trim());

                    cmd.Parameters.AddWithValue("@PONo", this.txtPONo.Text.Trim());

                    cmd.Parameters.AddWithValue("@PODate", this.txtPODate.Text.Trim());
                    cmd.Parameters.AddWithValue("@POMaterials", "");

                    cmd.Parameters.AddWithValue("@Remarks", this.txtremarks.Text.Trim());

                    cmd.Parameters.AddWithValue("@WEIGHSTATUS", "P");

                    cmd.Parameters.AddWithValue("@RFIDCARDENABLED", ddlCardStatus.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@CARDISSUEDATETIME", indianTime.ToString("MM/dd/yyyy HH:mm:ss tt"));

                  
                    cmd.ExecuteNonQuery();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record added successfully.');", true);
                    //lblMessage.Text = "Value inserted successfully";
                    //lblMessage.ForeColor = Color.Green;
                    //FillGrid();
                    con.Close();
                }

            }
        }

        public void updateimage()
        {
            snapshotofcamera1();
            snapshotofcamera2();
            string constr = ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    //cmd.CommandType
                    cmd.Connection = con;
                    cmd.CommandText = "Update tblTransactions set frontimage1=@frontimage1 ,frontimage2=@frontimage2 ,backimage1=@backimage1 ,backimage2=@backimage2 ,topimage1=@topimage1,topimage2=@topimage2 where tripid=@tripid";
                    //cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(txtID.Text));
                    //cmd.Parameters.AddWithValue("@NAME", txtName.Text);
                    //cmd.Parameters.AddWithValue("@ACTIVE", a);
                    //cmd.Parameters.AddWithValue("@CREATEDDATE", txtDate.Text);

                    cmd.Parameters.AddWithValue("@frontimage1", Session["IMAGE1"]);
                    cmd.Parameters.AddWithValue("@frontimage2", Session["IMAGE3"]);
                    cmd.Parameters.AddWithValue("@backimage1", Session["IMAGE2"]);
                    cmd.Parameters.AddWithValue("@backimage2", Session["IMAGE2"]);
                    cmd.Parameters.AddWithValue("@topimage1", Session["IMAGE3"]);

                    cmd.Parameters.AddWithValue("@topimage2", Session["IMAGE1"]);

                    cmd.Parameters.AddWithValue("@tripid", "45");
                  

                    cmd.ExecuteNonQuery();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Record added successfully.');", true);
                    //lblMessage.Text = "Value inserted successfully";
                    //lblMessage.ForeColor = Color.Green;
                    //FillGrid();
                    con.Close();
                }

            }
        }

        private void snapshotofcamera1()
        {
            

            byte[] buffer = new byte[1000000];
            int read, total = 0;
            byte[] m_Bitmap = null;

            try
            {
                

                if (File.Exists(Server.MapPath("~/Camera_Snapshot/backtruck.bmp")))
                {
                    FileStream fs = new FileStream(Server.MapPath("~/Camera_Snapshot/backtruck.bmp"), FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)br.BaseStream.Length;
                    m_Bitmap = new byte[length];
                    m_Bitmap = br.ReadBytes(length);
                    Session["IMAGE1"] = m_Bitmap;
                    br.Close();
                    fs.Close();
                }
                else
                {
                    FileStream fs = new FileStream(Server.MapPath("~/Camera_Snapshot/" + "NoImage.bmp"), FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)br.BaseStream.Length;
                    m_Bitmap = new byte[length];
                    m_Bitmap = br.ReadBytes(length);
                    Session["IMAGE1"] = m_Bitmap;
                    br.Close();
                    fs.Close();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Grab Error:" + ex, "Error!!");
            }
        }

        private void snapshotofcamera2()
        {
           

            byte[] buffer1 = new byte[1000000];
            int read1;
            int total1 = 0;
            byte[] m_Bitmap = null;
            try
            {
                //HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create("http://" + Session["CAMERAIP2"] + "/cgi-bin/snapshot.cgi?channel=1");
                //req1.Method = "POST";
                ////req.Timeout = 500;
                //NetworkCredential cred1 = new NetworkCredential(Session["CAMERAUSER2"].ToString(), Session["CAMERAPWD2"].ToString());
                //req1.Credentials = cred1;
                //WebResponse resp1 = req1.GetResponse();
                //// get response stream

                //Stream stream1 = resp1.GetResponseStream();
                //// read data from stream

                ////ListView1.Page = stream;
                //while ((read1 = stream1.Read(buffer1, total1, 1000)) != 0)
                //{
                //    total1 += read1;
                //}
                //// get bitmap

                //Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer1, 0, total1));
                //Bitmap bmp1 = new Bitmap(bmp, 1280, 720);

                //bmp1.Save(Server.MapPath("~/Camera_Snapshot/" + varFileName + ".bmp"), System.Drawing.Imaging.ImageFormat.Jpeg);

                //ImgCamera2.ImageUrl = "~/Camera_Snapshot/" + varFileName + ".jpg";

                if (File.Exists(Server.MapPath("~/Camera_Snapshot/fronttruck.bmp")))
                {
                    FileStream fs = new FileStream(Server.MapPath("~/Camera_Snapshot/fronttruck.bmp"), FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)br.BaseStream.Length;
                    m_Bitmap = new byte[length];
                    m_Bitmap = br.ReadBytes(length);
                    Session["IMAGE2"] = m_Bitmap;
                    br.Close();
                    fs.Close();
                }
                else
                {
                    FileStream fs = new FileStream(Server.MapPath("~/Camera_Snapshot/" + "NoImage.bmp"), FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)br.BaseStream.Length;
                    m_Bitmap = new byte[length];
                    m_Bitmap = br.ReadBytes(length);

                    Session["IMAGE2"] = m_Bitmap;

                    br.Close();
                    fs.Close();
                }

                if (File.Exists(Server.MapPath("~/Camera_Snapshot/toptruck.bmp")))
                {
                    FileStream fs = new FileStream(Server.MapPath("~/Camera_Snapshot/toptruck.bmp"), FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)br.BaseStream.Length;
                    m_Bitmap = new byte[length];
                    m_Bitmap = br.ReadBytes(length);
                    Session["IMAGE3"] = m_Bitmap;
                    br.Close();
                    fs.Close();
                }
                else
                {
                    FileStream fs = new FileStream(Server.MapPath("~/Camera_Snapshot/" + "NoImage.bmp"), FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)br.BaseStream.Length;
                    m_Bitmap = new byte[length];
                    m_Bitmap = br.ReadBytes(length);

                    Session["IMAGE3"] = m_Bitmap;

                    br.Close();
                    fs.Close();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Grab Error:" + ex, "Error!!");
            }
        }

        //public void  AddRFIDData ()
        //{
        //    using (DataClasses2DataContext ctx = new DataClasses2DataContext())
        //    {
        //        tblRFIDCARDDATA customer = new tblRFIDCARDDATA
        //        {
        //            RFIDCARDNO = txtRFIDCARDNO.Text,
        //            RFIDTAGUID = txtRFIDCardUID.Text,
        //            RFIDCARDENABLED = Convert.ToBoolean(ddlCardStatus.SelectedItem.Value),
        //            ChallanNo = txtInvoiceNo.Text,
        //            ChallanWeight = txtChallanWeight.Text,

        //            GateEntryNo = txtgateentryno.Text,
        //            IsMultiProduct = Convert.ToBoolean(ddlMultiproduct.SelectedValue),
        //        //if (ddlmc.SelectedItem.Value == "0")
        //        //{
        //        //    MaterialCalssificationCode = ddlmc.SelectedItem.Value.ToString(),
        //        //    MaterialClassificationName = "",
        //        //}
        //        //else
        //        //{
        //            MaterialCalssificationCode = ddlmc.SelectedItem.Value,
        //           MaterialClassificationName = ddlmc.SelectedItem.Text.Split('(')[0].Replace("(", ""),
        //        //}

        //        //try
        //        //{
        //        //    if (ddlmaterial.SelectedItem.Value == "0")
        //        //    {
        //        //        trans.MaterialCode = ddlmaterial.SelectedItem.Value;
        //        //        trans.MaterialName = ""; ;
        //        //    }
        //        //    else
        //        //    {
        //        MaterialCode = ddlmaterial.SelectedItem.Value,
        //                MaterialName = ddlmaterial.SelectedItem.Text.Split('(')[0].Replace("(", ""),
        //        //    }
        //        //}
        //        //catch { }
        //        //if (ddlpacking.SelectedItem.Value == "0")
        //        //{
        //        //    PackingCode = ddlpacking.SelectedItem.Value;
        //        //    PackingName = "";
        //        //}
        //        //else
        //        //{
        //            PackingCode = ddlpacking.SelectedItem.Value,
        //            PackingName = ddlpacking.SelectedItem.Text.Split('(')[0].Replace("(", ""),
        //        //}
        //        //try
        //        //{
        //            PackingQty = Convert.ToInt32(txtpackingqty.Text),
        //        //}
        //        //catch { }
        //        //try
        //        //{
        //            PODate = Convert.ToDateTime(txtPODate.Text),
        //        //}
        //        //catch { }
        //       PONo = txtPONo.Text,
        //        Remarks = txtremarks.Text,
        //        //if (ddlsupplier.SelectedItem.Value == "0")
        //        //{
        //        //   SupplierCode = ddlsupplier.SelectedItem.Value;
        //        //    SupplierName = "";
        //        //}
        //        //else
        //        //{
        //            SupplierCode = ddlsupplier.SelectedItem.Value,
        //            SupplierName = ddlsupplier.SelectedItem.Text.Split('(')[0].Replace("(", ""),
        //        //}

        //        //TransactionStatus = 1;
        //        //if (ddltransporter.SelectedItem.Value == "0")
        //        //{
        //        //    TransporterCode = ddltransporter.SelectedItem.Value;
        //        //    TransporterName = "";
        //        //}
        //        //else
        //        //{
        //           TransporterCode = ddltransporter.SelectedItem.Value,
        //            TransporterName = ddltransporter.SelectedItem.Text.Split('(')[0].Replace("(", ""),
        //       // }

        //        TripId = Convert.ToInt32(txtTripId.Text),
        //        //TripType = ddlinoutna.SelectedItem.Value != "0" || IsFirstWeight ? Convert.ToInt32(ddlinoutna.SelectedItem.Value) : isInbound == true ? 1 : 2;
        //        //ddlinoutna.SelectedValue = ddlinoutna.SelectedItem.Value != "0" || IsFirstWeight ? ddlinoutna.SelectedItem.Value : isInbound == true ? "1" : "2";
        //        TruckNo = txtTruckNo.Text.ToUpper()

        //    };
        //        ctx.tblRFIDCARDDATAs.InsertOnSubmit(customer);
        //        ctx.SubmitChanges();
        //    }
        //}

        private void FillGrid()
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
            {
                con.Open();
                string varParameters = "TRIPID,RFIDCARDNO,RFIDTAGUID,GateEntryNo,TruckNo,MaterialName,MaterialClassificationName,SupplierName,TransporterName,PackingName,WeighStatus,RFIDCARDENABLED";
                using (SqlCommand cmd = new SqlCommand("select " + varParameters + "  from tblRFIDCARDDATA", con))
                {
                    using (SqlDataAdapter ds = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dtbl = new DataTable())
                        {
                            ds.Fill(dtbl);
                            GridView1.DataSource = dtbl;
                            GridView1.DataBind();


                        }

                    }
                }

            }

        }
    }


}