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

namespace Avery_Weigh.Truck_Master
{
    public partial class AddEdit : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        TruckMasterRepository _truckrepo = new TruckMasterRepository();
        VehicleClassificationRepository vcrepo = new VehicleClassificationRepository();
        TransactionRepository _transactionRepo = new TransactionRepository();
        private Socket s3;

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                Get_VehicleClassificationCode();
                GetTruckForUpdate();
                timer1.Interval = 100;
                timer1.Enabled = true;
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
                int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
                TruckMaster _tran = _truckrepo.GetTruckMasters_List().Where(x => x.Id != id && x.TruckRegNo == txtTruckRegNo.Text.Trim() && x.IsDeleted == false).SingleOrDefault();
                if (_tran != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Truck Reg. No exist! Please try again');", true);
                }
                else
                {
                    TruckMaster _vc = db.TruckMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
                    if (_vc != null)
                    {
                        _vc.UOMWeight = ddluom.SelectedValue.ToString();
                        _vc.ClassificationCode = ddlClassificationCode.SelectedItem.Value.ToString();
                        _vc.TruckRegNo = txtTruckRegNo.Text;
                        if(txtStoredTareWeight.Text != "")
                            _vc.TareValidityDate = txtTareValidityDate.Text;
                        _vc.StoredTareWeight = txtStoredTareWeight.Text;
                        _vc.CurrentAverageTareValue = txtCUrrentTareValue.Text;
                        _vc.AverageTareScheme = txtAverageTareScheme.Text;
                        db.SubmitChanges();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Truck Record Saved Successfully')", true);
                        HtmlMeta meta = new HtmlMeta
                        {
                            HttpEquiv = "Refresh",
                            Content = "1;url=AddEdit.aspx?id="+id
                        };
                        Page.Controls.Add(meta);
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
                  
                    txtTruckRegNo.Text = _truck.TruckRegNo.Trim().ToString();
                    ddlClassificationCode.SelectedValue = _truck.ClassificationCode.Trim();
                    txtStoredTareWeight.Text = _truck.StoredTareWeight.Trim().ToString();
                    txtTareValidityDate.Text = _truck.TareValidityDate.ToString();
                    txtAverageTareScheme.Text = _truck.AverageTareScheme.Trim().ToString();
                    txtCUrrentTareValue.Text = _truck.CurrentAverageTareValue.Trim().ToString();
                    Get_VehicleClassificationCode();
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
                    TruckMaster _tran = _truckrepo.GetTruckMasterByTruckNo(txtTruckRegNo.Text.Trim());
                    if (_tran != null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('Same Truck No exist! Please try again');", true);
                    }
                    else
                    {
                        TruckMaster _vc = new TruckMaster();                      
                        _vc.UOMWeight = ddluom.SelectedValue.ToString();
                        _vc.ClassificationCode = ddlClassificationCode.SelectedValue;
                        _vc.TruckRegNo = txtTruckRegNo.Text;
                        if (txtStoredTareWeight.Text != "")
                            _vc.TareValidityDate = txtTareValidityDate.Text;
                        _vc.StoredTareWeight = txtStoredTareWeight.Text;
                        _vc.CurrentAverageTareValue = txtCUrrentTareValue.Text;
                        _vc.AverageTareScheme = txtAverageTareScheme.Text;
                        _vc.TareWtDateTime= DateTime.Now;
                        _vc.IsDeleted = false;
                        if (_truckrepo.Add_TruckMaster(_vc))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Truck Record Saved Successfully')", true);
                            HtmlMeta meta = new HtmlMeta
                            {
                                HttpEquiv = "Refresh",
                                Content = "1;url=AddEdit.aspx"
                            };
                            Page.Controls.Add(meta);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this,this.GetType(),"toastr","toastr.error('"+ex.Message.ToString()+"');",true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Next Record
        protected void Next_Record_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Id"].ToString());
            var next = _truckrepo.GetTruckMasters_List().Where(x => x.Id > id && x.IsDeleted == false).OrderBy(i => i.Id).FirstOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        //Get:Last Record
        protected void Last_Record_Click(object sender, EventArgs e)
        {
            var next = _truckrepo.GetTruckMasters_List().Where(x => x.IsDeleted == false).ToList().LastOrDefault();
            if (next != null)
                Response.Redirect("AddEdit.aspx?id=" + next.Id.ToString());
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('No more records founds.')", true);
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            string Weight = GetWeightFromTCPIP_SS();

            
            RuntimeWeight.Text = Weight.Trim();
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

        public tblMachineWorkingParameter getLoggedInUserWeigh()
        {
            string machineId = Session["WBID"].ToString();
            tblMachineWorkingParameter mwparameter = _transactionRepo.getMachineWorkingParameters(machineId);
            return mwparameter;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.txtStoredTareWeight.Text = this.RuntimeWeight.Text.Trim();
        }
    }
}