using Avery_Weigh.Model;
using Avery_Weigh.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Management;
using System.Net.NetworkInformation;

namespace Avery_Weigh
{
    public partial class Login : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        UserMasterRepository umRepo = new UserMasterRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
           
            
            if (!IsPostBack)
            {
                
                SystemMacID.Visible = false;
                UnlockCode.Visible = false;
                Panel1.Visible = false;
                Panel2.Visible = false;
                IEnumerable<SiteParameterSetting> setting = db.SiteParameterSettings.ToList();
                if (setting.Count() == 0)
                {
                    this.DefaultDataInsert();
                }
                if (setting.Count() >= 2)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('Multiple Record found. Please contact with service engineer.');", true);
                }
                else
                {
                    BindSettingsFromDB();
                }
            }
        }

        

        private void BindSettingsFromDB()
        {
            Image5.ImageUrl = "images/Type5/wi_base_normal.png";
            SiteParameterSetting setting = db.SiteParameterSettings.FirstOrDefault();
            if (setting != null)
            {
                if(setting.Cameras == 1)
                {
                    Image6.ImageUrl = "images/Type5/wi_view_normal.png";
                }
                else
                {
                    Image6.ImageUrl = "images/Type5/wi_view_disable.png";
                }
                if (setting.Sensors == 1)
                {
                    Image7.ImageUrl = "images/Type5/wi_sense_normal.png";
                }
                else
                {
                    Image7.ImageUrl = "images/Type5/wi_sense_disable.png";
                }
                if (setting.RFIDReader == 1)
                {
                    Image8.ImageUrl = "images/Type5/wi_tag_normal.png";
                }
                else
                {
                    Image8.ImageUrl = "images/Type5/wi_tag_disable.png";
                }
                if (setting.ConnectivityToCustomer == 1)
                {
                    Image9.ImageUrl = "images/Type5/wi_connect_normal.png";
                }
                else
                {
                    Image9.ImageUrl = "images/Type5/wi_connect_disable.png";
                }
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            Session["LoggedUser"] = "True";
            Session["LoggedOutUser"] = "True";
            string _UserName = UserName.Value;
            string _Password = Password.Value;
            string _WBID = WBID.Value;
            string _PlantID = PlantID.Value;
            UserMaster um = umRepo.CheckUserCredentials(_UserName, _Password, _WBID, _PlantID);
            SiteParameterSetting setting = db.SiteParameterSettings.FirstOrDefault();
            PlantMaster _plantCode=db.PlantMasters.Where(x => x.PlantCode == _PlantID).FirstOrDefault();

            if (SystemMacID.Visible==true && SystemMacID.Value.ToString().Length !=0 && UnlockCode.Visible==true && UnlockCode.Value.ToString().Length  !=0)
            {
                AddSystemMacID();
            }
            if (um != null && um.Id != 0)
            {
                Session["UserName"] = _UserName;
                Session["Password"] = _Password;
                Session["WBID"] = _WBID;
                Session["PlantID"] = _PlantID;

                int count = (from row in db.LoggedUsers
                             where row.MachineId == Session["WBID"].ToString() && row.IsDeleted == false
                             select row).Count();

                if (count == 1)
                {
                    LoggedUser tblloged = db.LoggedUsers.FirstOrDefault(x => x.MachineId == Session["WBID"].ToString() && x.IsDeleted == false);
                    tblloged.LogOutUser = true;
                    tblloged.LogOutUserDateTime = DateTime.Now;
                    tblloged.IsDeleted = true;
                    db.SubmitChanges();
                }

                LoggedUser tblloged1 = db.LoggedUsers.Where(x => x.MachineId == Session["WBID"].ToString() && x.IsDeleted == false).FirstOrDefault();

                

                if (tblloged1 != null)
                {
                    if (tblloged1.LoginUser == true && tblloged1.LogOutUser == true)
                    {
                        LoggedUser tblloged = new LoggedUser();
                        tblloged.UserName = Session["UserName"].ToString();
                        tblloged.PlantCodeID = Session["PlantID"].ToString();
                        tblloged.MachineId = Session["WBID"].ToString();
                        tblloged.LoginUser = true;
                        tblloged.LoginUserDateTime = DateTime.Now;
                        tblloged.LogOutUser = false;
                        tblloged.LogOutUserDateTime = DateTime.Now;
                        tblloged.IsDeleted = false;
                        db.LoggedUsers.InsertOnSubmit(tblloged);
                        db.SubmitChanges();

                        Session["LoggedUser"] = tblloged1.LoginUser;
                        Session["LoggedOutUser"] = tblloged1.LogOutUser;
                    }
                    else
                    {
                        Session["LoggedUser"] = tblloged1.LoginUser;
                        Session["LoggedOutUser"] = tblloged1.LogOutUser;
                    }
                }
                else
                {
                    LoggedUser tblloged = new LoggedUser();
                    tblloged.UserName = Session["UserName"].ToString();
                    tblloged.PlantCodeID = Session["PlantID"].ToString();
                    tblloged.MachineId = Session["WBID"].ToString();
                    tblloged.LoginUser = true;
                    tblloged.LoginUserDateTime = DateTime.Now;
                    tblloged.LogOutUser = false;
                    tblloged.LogOutUserDateTime = DateTime.Now;
                    tblloged.IsDeleted = false;
                    db.LoggedUsers.InsertOnSubmit(tblloged);
                    db.SubmitChanges();

                    Session["LoggedUser"] = tblloged.LoginUser;
                    Session["LoggedOutUser"] = "True";   // tblloged.LogOutUser;

                }
            }
            if (um != null && um.Id != 0)
            {
                this.checkMacID();
            }

            if (um!= null && um.Id != 0 && Session["LoggedUser"].ToString() == "True" && Session["LoggedOutUser"].ToString() == "True" && Session["MACID"].ToString() != "0")
            {
                Session["UserName"] = _UserName;
                Session["Password"] = _Password;
                Session["WBID"] = _WBID;
                Session["UserId"] = um.Id;
                Session["PlantID"] = _PlantID;
                Session["CompanyCode"] =_plantCode.CompanyCode;
                Session["WBFORM"] = "0";
                GLOBALVARIABLE();

                FormsAuthentication.RedirectFromLoginPage(um.Id.ToString(), true);
                UserClassification uc = umRepo.GetUserAuthorization(um.Id);
                New_Weighing_Unit(_WBID, _PlantID);

                tblMachineWorkingParameter _tmachine = db.tblMachineWorkingParameters.Where(x => x.PlantCode == Session["PlantID"].ToString() && x.MachineId == Session["WBID"].ToString()).FirstOrDefault();
                if (_tmachine != null)
                    Session["TareCheck"] = _tmachine.TareCheck;
                else
                    Session["TareCheck"] = "0";


                


                Session["FIRSTWT_RCD"] = "0";
                Session["SECONDWT_RCD"] = "0";

                //if (Session["LoggedUser"].ToString() == "True" && Session["LoggedOutUser"].ToString() == "True")
                //{
                    if (uc != null)
                    {
                        if (uc.GateEntry == true)
                            Response.Redirect("/GateEntryForm");
                        else if (uc.Weighment == false)
                        {
                            Session["WBFORM"] = "1";
                            Response.Redirect("/ManageMasters");
                        }
                        else
                            Response.Redirect("/Manual_Weighment");
                    }
                //}
                //else
                //{

                //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('User has already logged this Machine. Please check and try again.');", true);
                //    //Response.Redirect(Request.UrlReferrer.ToString());
                //}


                

            }
            else
            {
                if (Session["LoggedUser"].ToString() == "True" && Session["LoggedOutUser"].ToString() == "True")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('No User account found. Please try again.');", true);
                    AddError_Logs("No User account found. Please try again.");
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('User has already logged this weighbridge id. Please check weighbridge id and try again.');", true);
                    AddError_Logs("User has already logged this weighbridge id. Please check weighbridge id and try again.");
                }
            }
        }

        private void AddError_Logs(string varerrordesc)
        {
            Log  sm = new Log();
            sm.UserId = UserName.Value;
            sm.PlantCode = PlantID.Value;
            sm.LogTitle = "login";
            sm.LogDescription = varerrordesc.ToString();
            sm.LogDate = DateTime.Now;
            sm.URL = HttpContext.Current.Request.Url.AbsoluteUri;
            db.Logs.InsertOnSubmit(sm);
            db.SubmitChanges();


        }

        private void checkMacID()
        {
            tblSystemCheckedID tblcheck = db.tblSystemCheckedIDs.FirstOrDefault(x => x.WBID == Session["WBID"].ToString() && x.PLANTID == Session["PlantID"].ToString());
            if (tblcheck != null)
            {
                Session["MACID"] = tblcheck.SYSTEMCHECKDATA;
                if (SystemMacIDChecked() != Session["MACID"].ToString())
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "toastr.error('System MAC-id is not Matched. Please try again.');", true);
                    AddError_Logs("System MAC-id is not Matched. Please try again.");
                    Session["MACID"] = "0";
                    //Response.Redirect("/Logout");
                    FindMacIDofSystem();
                    SystemMacID.Visible = true;
                    UnlockCode.Visible = true;
                    Panel1.Visible = true;
                    Panel2.Visible = true;
                    //Response.Redirect(Request.UrlReferrer.ToString());
                }

            }
            else
            {
                FindMacIDofSystem();
                SystemMacID.Visible = true;
                UnlockCode.Visible = true;
                Panel1.Visible = true;
                Panel2.Visible = true;
                Session["MACID"] = "0";
            }
        }

        private void AddSystemMacID()
        {
            tblSystemCheckedID tblcheck = db.tblSystemCheckedIDs.FirstOrDefault(x => x.WBID == Session["WBID"].ToString() && x.PLANTID == Session["PlantID"].ToString());
            //int k = 0;
            if (tblcheck != null)
            {
                //k = tblcheck.Id;
                //tblSystemCheckedID sm = new tblSystemCheckedID();
                //tblcheck.Id= db.tblSystemCheckedIDs.Where(x => x.Id == k).FirstOrDefault();
                tblcheck.WBID = WBID.Value;
                tblcheck.PLANTID = PlantID.Value;
                tblcheck.SYSTEMCHECKDATA = UnlockCode.Value;

                //db.tblSystemCheckedIDs.InsertOnSubmit(sm);
                db.SubmitChanges();
            }
            else
            {
                tblSystemCheckedID sm = new tblSystemCheckedID();
                sm.WBID = WBID.Value;
                sm.PLANTID = PlantID.Value;
                sm.SYSTEMCHECKDATA = UnlockCode.Value;

                db.tblSystemCheckedIDs.InsertOnSubmit(sm);
                db.SubmitChanges();
            }


        }

        private string SystemMacIDChecked()
        {
            string macAddresses = string.Empty;
            string sMacAddress = string.Empty;

            System.Management.ManagementClass theClass = new System.Management.ManagementClass("Win32_Processor");
            System.Management.ManagementObjectCollection theCollectionOfResults = theClass.GetInstances();

            foreach (System.Management.ManagementObject currentResult in theCollectionOfResults)
            {
                sMacAddress = currentResult["ProcessorID"].ToString();
            }

            macAddresses = sMacAddress;


            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] arr_ = Encoding.ASCII.GetBytes(macAddresses);
            arr_ = mD5CryptoServiceProvider.ComputeHash(arr_);

            macAddresses = Convert.ToBase64String(arr_).ToString();


            return macAddresses;


        }

        private void FindMacIDofSystem()
        {
            string macAddresses = string.Empty;
            string sMacAddress = string.Empty;

            System.Management.ManagementClass theClass = new System.Management.ManagementClass("Win32_Processor");
            System.Management.ManagementObjectCollection theCollectionOfResults = theClass.GetInstances();

            foreach (System.Management.ManagementObject currentResult in theCollectionOfResults)
            {
                sMacAddress = currentResult["ProcessorID"].ToString();
            }

            macAddresses = sMacAddress;
            SystemMacID.Value = macAddresses;
        }

        private void GLOBALVARIABLE()
        {
            Session["IMAGE1"] = 0;
            Session["IMAGE2"] = 0;
            Session["SENSORCONNECT"] = "0";
            Session["DIOIP"] = "0";
            Session["DIOPORTNO"] = "0";
            Session["RFIDTAGUID"] = "0";
            Session["BALANCE"] = "0";
            Session["RFIDCARDLENGTH"] = "24";
            Session["CAMERAIP1"] = "0";
            Session["CAMERAPORTNO1"] = "0";
            Session["CAMERAIP2"] = "0";
            Session["CAMERAPORTNO2"] = "0";
            Session["CAMERAACTIVE"] = "N";
            Session["CAMERAUSER1"] = "0";
            Session["CAMERAPWD1"] = "0";
            Session["CAMERAUSER2"] = "0";
            Session["CAMERAPWD2"] = "0";
            Session["CAMERAUSER3"] = "0";
            Session["CAMERAPWD3"] = "0";
            Session["CAMERAIP3"] = "0";
            Session["CAMERAPORTNO3"] = "0";
            Session["FIRSTWT_RCD"] = "0";
            Session["SECONDWT_RCD"] = "0";
            Session["ALPHADISPLAYIP1"] = "0";
            Session["ALPHADISPLAYPORTNO1"] = "0";
            Session["ALPHADISPLAYIP2"] = "0";
            Session["ALPHADISPLAYPORTNO2"] = "0";
            Session["ALPHADISPLAYACTIVE"] = "N";
            Session["FLAGREADER1"] = "0";
            Session["FLAGREADER2"] = "0";
        }

      
        //Check Assign Sensor IP Address 
        private void SensorMaster_Glb(string WBId, string PlantId)
        {
            //tblSensorMaster sensor_glb_1 = new tblSensorMaster();
            var sensor_glb = (from a in db.tblSensorMasters
                              join b in db.PlantMasters on a.PlantCode equals b.PlantCode
                              where a.MachineId == WBId && b.PlantCode == PlantId
                              select a).FirstOrDefault();
            if (sensor_glb != null)
            {
                Session["DIOIP"] = sensor_glb.SensorIP.ToString();
                Session["DIOPORTNO"] = sensor_glb.SensorPort.ToString();
                Session["SENSORSA"] = 0;
                Session["SENSORSB"] = 0;
                Session["SENSORSC"] = 0;
            }
            else
            {
                Session["DIOIP"] = "0";
                Session["DIOPORTNO"] = "0";
            }
            
        }

        //Check Assign Alpha Display IP Address 
        private void AlphaDisplayMaster_Glb(string WBId, string PlantId)
        {
            //tblSensorMaster sensor_glb_1 = new tblSensorMaster();
            var alphadisplay_glb = (from a in db.AlphaDisplayMasters 
                              join b in db.PlantMasters on a.PlantCodeId equals b.PlantCode
                              where a.MachineId == WBId && b.PlantCode == PlantId
                              select a).FirstOrDefault();
            if (alphadisplay_glb.AlphaDisplayIdentification =="1")
            {
                Session["ALPHADISPLAYIP1"] = alphadisplay_glb.AlphaDisplayIP.ToString();
                Session["ALPHADISPLAYPORTNO1"] = alphadisplay_glb.AlphaDisplayPort.ToString();
                Session["ALPHADISPLAYMSG1"] = string.Empty;
            }
            else
            {
                Session["ALPHADISPLAYIP1"] = "0";
                Session["ALPHADISPLAYPORTNO1"] = "0";
            }

            if (alphadisplay_glb.AlphaDisplayIdentification == "2")
            {
                Session["ALPHADISPLAYIP2"] = alphadisplay_glb.AlphaDisplayIP.ToString();
                Session["ALPHADISPLAYPORTNO2"] = alphadisplay_glb.AlphaDisplayPort.ToString();

            }
            else
            {
                Session["ALPHADISPLAYIP2"] = "0";
                Session["ALPHADISPLAYPORTNO2"] = "0";
                Session["ALPHADISPLAYMSG2"] = string.Empty;
            }

        }

        //Check Assign Camera IP Address 
        private void CameraMaster_Glb(string WBId, string PlantId)
        {
            //tblSensorMaster sensor_glb_1 = new tblSensorMaster();
            var CameraMaster_glb = (from a in db.CameraMasters
                                    join b in db.PlantMasters on a.PlantCodeID equals b.PlantCode
                                    where a.MachineId == WBId && b.PlantCode == PlantId
                                    select a).FirstOrDefault();
            if (CameraMaster_glb.CameraIndentification == "1")
            {
                Session["CAMERAIP1"] = CameraMaster_glb.CameraIP.ToString();
                Session["CAMERAPORTNO1"] = CameraMaster_glb.CameraPort.ToString();
               
            }
            else
            {
                Session["CAMERAIP1"] = "0";
                Session["CAMERAPORTNO1"] = "0";
            }

            if (CameraMaster_glb.CameraIndentification == "2")
            {
                Session["CAMERAIP2"] = CameraMaster_glb.CameraIP.ToString();
                Session["CAMERAPORTNO2"] = CameraMaster_glb.CameraPort.ToString();

            }
            else
            {
                Session["CAMERAIP2"] = "0";
                Session["CAMERAPORTNO2"] = "0";
            }

        }

        private void New_Cameramaster_Glb(string WBID, string PlantId)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select * from CameraMaster where PlantCodeID='" + PlantId + "' and MachineId='" + WBID + "'", con))
                {
                    using (SqlDataAdapter ds = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dtbl = new DataTable())
                        {
                            ds.Fill(dtbl);
                            
                            if (dtbl.Rows.Count > 0)
                            {
                                for (int i=0; i< dtbl.Rows.Count;i++)
                                {
                                    if (dtbl.Rows[i]["CameraIndentification"].ToString() == "1")
                                    {
                                        Session["CAMERAIP1"] = dtbl.Rows[i]["CameraIP"].ToString();
                                        Session["CAMERAPORTNO1"] = dtbl.Rows[i]["CameraPort"].ToString();
                                        Session["CAMERAUSER1"] = dtbl.Rows[i]["CameraUserName"].ToString();
                                        Session["CAMERAPWD1"] = dtbl.Rows[i]["CameraPwd"].ToString();

                                    }
                                    if (dtbl.Rows[i]["CameraIndentification"].ToString() == "2")
                                    {
                                        Session["CAMERAIP2"] = dtbl.Rows[i]["CameraIP"].ToString();
                                        Session["CAMERAPORTNO2"] = dtbl.Rows[i]["CameraPort"].ToString();
                                        Session["CAMERAUSER2"] = dtbl.Rows[i]["CameraUserName"].ToString();
                                        Session["CAMERAPWD2"] = dtbl.Rows[i]["CameraPwd"].ToString();

                                    }
                                    if (dtbl.Rows[i]["CameraIndentification"].ToString() == "3")
                                    {
                                        Session["CAMERAIP3"] = dtbl.Rows[i]["CameraIP"].ToString();
                                        Session["CAMERAPORTNO3"] = dtbl.Rows[i]["CameraPort"].ToString();
                                        Session["CAMERAUSER3"] = dtbl.Rows[i]["CameraUserName"].ToString();
                                        Session["CAMERAPWD3"] = dtbl.Rows[i]["CameraPwd"].ToString();

                                    }
                                   
                                }
                               
                            }
                        }

                    }
                }

            }
        }

        private void New_Weighing_Unit(string WBID, string PlantId)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select * from WeightMachineMaster where PlantCodeID='" + PlantId + "' and MachineId='" + WBID + "'", con))
                {
                    using (SqlDataAdapter ds = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dtbl = new DataTable())
                        {
                            ds.Fill(dtbl);

                            if (dtbl.Rows.Count > 0)
                            {
                                
                                Session["WEIGHINGUNIT"] = dtbl.Rows[0]["WeighingUnit"].ToString();
                                        
                            }
                        }

                    }
                }

            }
        }

        private void DefaultDataInsert()
        {
            SqlConnection tmpConn = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString);

            string vardata1 = "INSERT [dbo].[UserMaster] ([UserName], [Password], [UserType], [Plantcode], [WeighbridgeID], [IsDeleted]) VALUES (N'admin', N'admin', 1, N'PC001', N'WB510', 0)";
            string vardata2 = "INSERT [dbo].[UserClassifications] ([UserType], [MasterFileUpdation], [MasterRecordDeletion], [PendingRecordDeletion], [TransactionDeletion], [Configuration], [PasswordPolicy], [PasswordReset], [UserCreation], [GateEntry], [RFIDIssue], [Weighment], [DatabaseOperation], [IsDeleted]) VALUES (N'User 1', 1, 1, 1, 1, 0, 1, 1, 0, 0, 0, 1, 0, 0)";
            string vardata3 = "INSERT [dbo].[SHIFTTIME] ([NOOFSHIFTS], [STA], [EDA], [STB], [EDB], [STC], [EDC]) VALUES (N'THREE', N'06:00:00 AM', N'02:00:00 PM', N'02:00:00 PM', N'10:00:00 PM', N'10:00:00 PM', N'06:00:00 AM')";
            //string vardata4 = "INSERT [dbo].[tblMachineWorkingParameters] ([PlantCode], [MachineId], [IPPort], [PortNo], [ModeOfComs], [StabilityNos], [StabilityRange], [ZeroInterlock], [ZeroInterlockRange], [TransactionNoPrefix], [TareCheck], [StoredTare], [TareScheme], [TareWeightValue], [IsDeleted]) VALUES ( N'PC001', N'WB510', N'10.110.0.29', N'3002', N'0', N'1', N'1', 1, N'12', N'2', 1, 1, N'Tolerance(%)', 2, 0)";
            string vardata4 = "INSERT[dbo].[tblMachineWorkingParameters]([PlantCode], [MachineId], [IPPort], [PortNo], [ModeOfComs], [StabilityNos], [StabilityRange], [ZeroInterlock], [ZeroInterlockRange], [WeightInterlock], [WeightInterlockRange], [LastStoredWeight], [TransactionNoPrefix], [TareCheck], [StoredTare], [TareScheme], [TareWeightValue], [IsDeleted], [NetWeightLimit]) VALUES( N'PC001', N'WB510', N'192.168.1.50', N'1', N'0', N'1', N'1', 0, N'12', 0, N'10', CAST(0.00 AS Numeric(18, 2)), N'2', 1, 0, N'Tolerance(%)', 2, 0, 0)";
            string vardata5 = "INSERT [dbo].[PlantMaster] ([CompanyCode], [PlantCode], [PlantName], [PlantAddress1], [PlantAddress2], [PlantContactPerson], [Designation], [ContactMobile], [ContactEmail], [NoOfMachine], [IsDeleted]) VALUES ( N'com1', N'PC001', N'Avery India Limited', N'Sector-25', N'Faridabad', N'S SINGH', N'DEVELOPER', N'9560667095', N'sanjaysingh.srs@gmail.com', 52, 0)";
            string vardata6 = "INSERT [dbo].[CompanyMaster] ([CompanyCode], [CompanyName], [CompanyLogo], [CompanyAddress1], [CompanyAddress2], [ContactPerson], [ContactMobile], [ContactEmail], [IsDeleted]) VALUES (N'com1', N'Avery India limited', N'yOUR COMPANY LOGO.jpg', N'Sector - 25', N'Faridabad', N'sanjay singh', N'9289707935', N'sanjaysingh@awtxglobal.com', NULL)";
            string vardata7 = "INSERT [dbo].[WeightMachineMaster] ([PlantCodeId], [MachineId], [MachineName], [Capacity], [WeighingUnit], [Resolution], [Model], [PlatformSize], [MachineNo], [Indicator], [LCType], [NoOfLoadCells], [LoadCellSerialNos], [EquipmentId], [InvoiceNo], [DespatchDate], [InstallationDate], [WarrentyUpto], [ReasonWarrentyUptoDate], [IsDeleted]) VALUES (N'PC001', N'WB510', NULL, N'150000kg', N'kg', N'10kg', N'213', N'12', N'21', N'1', N'214', N'12', N'12', 124, N'124', CAST(0x0000AABD00000000 AS DateTime), CAST(0x0000AABE00000000 AS DateTime), CAST(0x0000AAC300000000 AS DateTime), N'123', 0)";

            string vardata10 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Trip Id', N'Trip Id', 0, 0, 0)";
            string vardata11 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Weighing Type', N'Weighing Type', 0, 0, 0)";
            string vardata12 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Multi Product', N'Multi Product', 0, 0, 1)";
            string vardata13 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Gate Entry No', N'Gate Entry No', 0, 0, 0)";
            string vardata14 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Truck No', N'Vehicle No', 1, 0, 0)";
            string vardata15 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Material', N'Product Name', 1, 1, 0)";
            string vardata16 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Material Classification', N'Material Classification', 0, 0, 0)";
            string vardata17 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Supplier/customer', N'Supplier/customer Name', 1, 0, 0)";
            string vardata18 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Transporter', N'Haulier Name', 1, 0, 0)";
            string vardata19 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Packing', N'Packing', 0, 0, 0)";
            string vardata20 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Packing qty', N'Packing qty', 0, 0, 0)";
            string vardata21 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Challan/Invoice no', N'Challan/Invoice no', 0, 0, 0)";
            string vardata22 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Challan/Invoice  Date', N'Challan/Invoice  Date', 0, 0, 0)";
            string vardata23 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Challan weight', N'Challan weight', 0, 0, 0)";
            string vardata24 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'PO /SO/DO no', N'PO /SO/DO no', 0, 0, 0)";
            string vardata25 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'PO/SO/DO date', N'PO/SO/DO date', 0, 0, 0)";
            string vardata26 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'PO/SO/DO materials', N'PO/SO/DO materials', 0, 0, 1)";
            string vardata27 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Remarks', N'Remarks', 0, 0, 0)";
            string vardata28 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'1st weight', N'1st weight', 0, 0, 0)";
            string vardata29 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'2nd weight', N'2nd weight', 0, 0, 0)";
            string vardata30 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Net weight', N'Net weight', 0, 0, 0)";
            string vardata31 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Gate Pass no', N'Gate Pass no', 0, 0, 0)";
            string vardata32 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Security name', N'Security name', 0, 0, 0)";
            string vardata33 = "INSERT[dbo].[DynamicFieldNames]( [PlantId], [MachineId], [FieldName], [FieldValue], [IsMandatory1], [IsMandatory2], [IsRequired]) VALUES( N'PC001', N'WB510', N'Security Remarks', N'Security Remarks', 0, 0, 0)";




            //for (int i = 1; i <= 7; i++)
            //{
            using (SqlCommand sqlCmd = new SqlCommand(vardata1, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }
            //}
            using (SqlCommand sqlCmd = new SqlCommand(vardata2, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }
            using (SqlCommand sqlCmd = new SqlCommand(vardata3, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }
            using (SqlCommand sqlCmd = new SqlCommand(vardata4, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }
            using (SqlCommand sqlCmd = new SqlCommand(vardata5, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }
            using (SqlCommand sqlCmd = new SqlCommand(vardata6, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata7, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }
            //}

            string vardata8 = "INSERT [dbo].[SiteParameterSettings] ([CompanyId], [IsGateEntry], [Cameras], [AlphaNumericDisplay], [Sensors], [Barriers], [PASystem], [RFIDReader], [ConnectivityToCustomer], [NoSpectalCharacterForTruck], [AxleWeighting], [TMS], [AuthorizedForTARE], [ToleranceCheckforCustQty], [ToleranceCheckforSupQty], [CreateDate], [WeightMachineId], [PlantCodeId]) VALUES (NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, CAST(0x0000AB650131D679 AS DateTime), N'WB510', N'PC001')";

            using (SqlCommand sqlCmd = new SqlCommand(vardata8, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            string vardata9 = "INSERT [dbo].[UserWeightMachineMaster] ([UserId],[WeightMachineId]) VALUES (1, 1)";

            using (SqlCommand sqlCmd = new SqlCommand(vardata9, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            
            using (SqlCommand sqlCmd = new SqlCommand(vardata10, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata11, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata12, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata13, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata14, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata15, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata16, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata17, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata18, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata19, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata20, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata21, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata22, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata23, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata24, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata25, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata26, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata27, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata28, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata29, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata30, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata31, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata32, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }

            using (SqlCommand sqlCmd = new SqlCommand(vardata33, tmpConn))
            {
                tmpConn.Open();
                int resultObj = sqlCmd.ExecuteNonQuery();
                tmpConn.Close();
            }
        }
    }
}