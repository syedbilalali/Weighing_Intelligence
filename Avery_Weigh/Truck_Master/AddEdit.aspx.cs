using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Avery_Weigh.Repository;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Avery_Weigh.Model;

namespace Avery_Weigh.Truck_Master
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        TruckMasterRepository _truckrepo = new TruckMasterRepository();
        VehicleClassificationRepository vcrepo = new VehicleClassificationRepository();
        TransactionRepository _transactionRepo = new TransactionRepository();
        TransporterRepository _transReport = new TransporterRepository();
        TaretrTareToleranceRepository _toltype = new TaretrTareToleranceRepository();
        SystemLogRepository logRepo = new SystemLogRepository();
        private Socket s3;

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                timer1.Enabled = true;
                Get_ToleranceType();
                Get_VehicleClassificationCode();
                Get_TransporterCode();
                GetTruckForUpdate();
                
            }
        }

        //Get:VehicleClassification Code
        private void Get_VehicleClassificationCode()
        {
            ddlClassificationCode.DataSource = vcrepo.Get_VehicleClassificationCode();
            ddlClassificationCode.DataTextField = "Make";
            ddlClassificationCode.DataValueField = "ClassificationCode";
            ddlClassificationCode.DataBind();
            ddlClassificationCode.Items.Insert(0, new ListItem("Select", ""));
        }

        private void Get_TransporterCode()
        {
            ddlTransporterCode.DataSource = _transReport.Get_TransporterCode();
            ddlTransporterCode.DataTextField = "Name";
            ddlTransporterCode.DataValueField = "Code";
            ddlTransporterCode.DataBind();
            ddlTransporterCode.Items.Insert(0, new ListItem("Select", ""));
        }

        //Get Tolerance Type 
        protected void Get_ToleranceType()
        {
            IEnumerable<Model_TareToletrance> data = _toltype.Get_TareToleranceType_Add();
            if (data != null)
            {
                ddlTareToleranceType.DataTextField = "Description";
                ddlTareToleranceType.DataValueField = "Description";
                ddlTareToleranceType.DataSource = data;
                ddlTareToleranceType.DataBind();
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {

            if (Request.QueryString["Id"] == null)
            {
                AddTruck();
            }
            else
            {
                UpdateTruck();
            }

        }

        //Update:TruckMaster
        private void UpdateTruck()
        {
            if (Request.QueryString["Id"] != null)
            {
                if (txtStoredTareWeight.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Validity date should not be blank.');", true);
                    adderrorlog("Validity date should not be blank.", "Truck Master");
                    return;
                }
                else if (txtStoredTareWeight.Text != "")
                {
                    if (Convert.ToDateTime(txtTareValidityDate.Text.Trim()) < DateTime.Now)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Validity date should be greater then current date.');", true);
                        adderrorlog("Validity date should be greater then current date.", "Truck Master");
                        return;
                    }
                }

                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                TruckMaster _tran = _truckrepo.GetTruckMasters_List().Where(x => x.Id != id && x.TruckRegNo == txtTruckRegNo.Text.Trim() && x.IsDeleted == false).SingleOrDefault();
                if (_tran != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Truck Reg. No exist! Please try again');", true);
                    adderrorlog("Same Truck Reg. No exist! Please try again", "Truck Master");
                }
                else
                {
                    TruckMaster _vc = db.TruckMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
                    if (_vc != null)
                    {
                        _vc.UOMWeight = ddluom.SelectedValue.ToString();
                        _vc.ClassificationCode = ddlClassificationCode.SelectedItem.Value.ToString();
                        _vc.TruckRegNo = txtTruckRegNo.Text.ToUpper();
                        if(txtStoredTareWeight.Text != "")
                            _vc.TareValidityDate = txtTareValidityDate.Text;
                        _vc.StoredTareWeight = txtStoredTareWeight.Text;
                        _vc.CurrentAverageTareValue = txtWtValue.Text.Trim();  // txtCUrrentTareValue.Text;
                        _vc.AverageTareScheme = ddlTareToleranceType.Text;  // txtAverageTareScheme.Text;
                        _vc.StoredTareDateTime = Convert.ToDateTime(txtStoredTareDateTime.Text);
                        if (ddlTransporterCode.SelectedItem.Value == "0")
                        {
                            _vc.TransporterCode = ddlTransporterCode.SelectedItem.Value;
                            _vc.TransporterName = "";
                        }
                        else
                        {

                            _vc.TransporterCode = ddlTransporterCode.SelectedItem.Value;
                            _vc.TransporterName = ddlTransporterCode.SelectedItem.Text.Split('(')[0].Replace("(", "");
                        }
                        db.SubmitChanges();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Truck Record Saved Successfully');", true);
                        
                    }
                }

            }
        }

        //Get:TruckMaster Record For Update
        private void GetTruckForUpdate()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                divoptions.Style.Add("display", "block");
                txtTruckRegNo.Enabled = false;
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                TruckMaster _truck = _truckrepo.Get_TruckMasterById(id);
                if (_truck != null)
                {
                  
                    txtTruckRegNo.Text = _truck.TruckRegNo.Trim().ToUpper().ToString();
                    ddlClassificationCode.SelectedValue = _truck.ClassificationCode.Trim();
                    txtStoredTareWeight.Text = _truck.StoredTareWeight.Trim().ToString();
                    txtTareValidityDate.Text = _truck.TareValidityDate.ToString();
                    //txtAverageTareScheme.Text = _truck.AverageTareScheme.Trim().ToString();
                    ddlTareToleranceType.Text = _truck.AverageTareScheme.Trim().ToString();
                    //txtCUrrentTareValue.Text = _truck.CurrentAverageTareValue.Trim().ToString();
                    txtWtValue.Text = _truck.CurrentAverageTareValue.Trim().ToString();
                    txtStoredTareDateTime.Text = _truck.StoredTareDateTime.ToString();
                    try
                    {
                        ddlTransporterCode.Text = _truck.TransporterCode.Trim();
                    }
                    catch { }
                    Get_VehicleClassificationCode();
                    Get_TransporterCode();
                    ddluom.SelectedValue = _truck.UOMWeight.Trim().ToString();
                }
            }
        }

        //Add:New TruckMaster Record
        private void AddTruck()
        {
            if (Request.QueryString["Id"] == null)
            {
                try
                {
                    if (txtStoredTareWeight.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Validity date should not be blank.');", true);
                        adderrorlog("Validity date should not be blank.", "Truck Master");
                        return;
                    }
                    else if (txtStoredTareWeight.Text != "")
                    {
                        if (Convert.ToDateTime(txtTareValidityDate.Text.Trim())< DateTime.Now )
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Validity date should be greater then current date.');", true);
                            adderrorlog("Validity date should be greater then current date.", "Truck Master");
                            return;
                        }
                    }

                    TruckMaster _tran = _truckrepo.GetTruckMasterByTruckNo(txtTruckRegNo.Text.Trim());
                    if (_tran != null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Truck No exist! Please try again');", true);
                        adderrorlog("Same Truck No exist! Please try again.", "Truck Master");
                    }
                    else
                    {
                        TruckMaster _vc = new TruckMaster();                      
                        _vc.UOMWeight = ddluom.SelectedValue.ToString();
                        _vc.ClassificationCode = ddlClassificationCode.SelectedValue;
                        _vc.TruckRegNo = txtTruckRegNo.Text.ToUpper();
                        if (txtStoredTareWeight.Text != "")
                            _vc.TareValidityDate = txtTareValidityDate.Text;
                        _vc.StoredTareWeight = txtStoredTareWeight.Text;
                        _vc.CurrentAverageTareValue = txtWtValue.Text;  // txtCUrrentTareValue.Text;
                        _vc.AverageTareScheme = ddlTareToleranceType.Text;  // txtAverageTareScheme.Text;
                        _vc.StoredTareDateTime = Convert.ToDateTime(txtStoredTareDateTime.Text);
                        _vc.IsDeleted = false;
                        if (ddlTransporterCode.SelectedItem.Value == "0")
                        {
                            _vc.TransporterCode = ddlTransporterCode.SelectedItem.Value;
                            _vc.TransporterName = "";
                        }
                        else
                        {

                            _vc.TransporterCode = ddlTransporterCode.SelectedItem.Value;
                            _vc.TransporterName = ddlTransporterCode.SelectedItem.Text.Split('(')[0].Replace("(", "");
                        }
                        if (_truckrepo.Add_TruckMaster(_vc))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Truck Record Saved Successfully');", true);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('"+ex.Message.ToString()+"');",true);
                    adderrorlog(ex.Message.ToString(), "Truck Master");
                }
            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("Add.aspx");
        }

        //Get:First Record
        protected void First_Record_Click(object sender, EventArgs e)
        {
            var next = _truckrepo.GetTruckMasters_List().Where(x => x.IsDeleted == false).ToList().FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.');", true);
        }

        //Get:Previous Record
        protected void Previous_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            TruckMaster next = null;
            try
            {
                next = _truckrepo.GetTruckMasters_List().Where(x => x.Id < id && x.IsDeleted == false).OrderByDescending(i => i.Id).FirstOrDefault();
            }
            catch
            {
            }
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.');", true);
        }

        //Get:Next Record
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = _truckrepo.GetTruckMasters_List().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.');", true);
        }

        //Get:Last Record
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = _truckrepo.GetTruckMasters_List().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.');", true);
        }

        protected void btnGetTareWt_Click(object sender, EventArgs e)
        {
            //Enable this code if weight machine connected on local client machine 
            //string Weight = GetWeightFromIP();

            txtStoredTareWeight.Text = lbkTareWt.Text.Split(':')[1].Trim();
            txtStoredTareDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
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

            return "5000";
        }

        public tblMachineWorkingParameter getLoggedInUserWeigh()
        {
            string machineId = string.Empty;
            try
            {
                machineId = Session["WBID"].ToString();
            }
            catch { Response.Redirect("~/Login.aspx"); }
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
        protected void timer1_Tick(object sender, EventArgs e)
        {

            //Enable this code if weight machine connected on local client machine 
           string Weight = GetWeightFromTCPIP_SS();
            //string Weight = "5000";
            lbkTareWt.Text = "Weight: " + Weight;
            
        }

        protected void ddlTareToleranceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTareToleranceType.Text.Trim()))
            {
                AverageTareSchema _avgtarewt = db.AverageTareSchemas.Where(x => x.Description == ddlTareToleranceType.Text.Trim()).FirstOrDefault();
                this.txtWtValue.Text = _avgtarewt.weightvalue.ToString();
            }
        }
        private void adderrorlog(string varerrdesc, string varerrortitle)
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