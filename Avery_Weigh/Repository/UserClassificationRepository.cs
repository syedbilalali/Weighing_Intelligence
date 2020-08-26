using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class UserClassificationRepository
    {
        /*   
         *   this class contains all the methods which is use to 
         *   perform operations with database                      
             */
        DataClasses1DataContext db = new DataClasses1DataContext();

        //Get:UserClassification List
        public IList<UserClassification> Get_ActiveUserClassificationList()
        {
            var data = (from a in db.UserClassifications
                        where a.IsDeleted == false
                        select a).ToList();
            return data;
        }

        //Get:UserClassification Data into DataTable
        public DataTable GetDataTable_UserClassification()
        {
            var data = db.UserClassifications.Where(x => x.IsDeleted == false).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr No");
            dt.Columns.Add("User Type");
            dt.Columns.Add("Master File Updation");
            dt.Columns.Add("Master Record Deletion");
            dt.Columns.Add("Pending Record Deletion");
            dt.Columns.Add("Transaction Deletion");
            dt.Columns.Add("Configuration");
            dt.Columns.Add("Password Policy");
            dt.Columns.Add("Password Reset");
            dt.Columns.Add("User Creation");
            dt.Columns.Add("Gate Entry");
            dt.Columns.Add("RFID Issue");
            dt.Columns.Add("Weighment");
            dt.Columns.Add("Database Operation");
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["User Type"] = item.UserType;
                dr["Master File Updation"] = item.MasterFileUpdation.Value == true ? "Enable" : "Disable";
                dr["Master Record Deletion"] = item.MasterRecordDeletion == true ? "Enable" : "Disable";
                dr["Pending Record Deletion"] = item.PendingRecordDeletion == true ? "Enable" : "Disable";
                dr["Transaction Deletion"] = item.TransactionDeletion == true ? "Enable" : "Disable";
                dr["Configuration"] = item.Configuration == true ? "Enable" : "Disable";
                dr["Password Policy"] = item.PasswordPolicy == true ? "Enable" : "Disable";
                dr["Password Reset"] = item.PasswordReset == true ? "Enable" : "Disable";
                dr["User Creation"] = item.UserCreation == true ? "Enable" : "Disable";
                dr["Gate Entry"] = item.GateEntry == true ? "Enable" : "Disable";
                dr["RFID Issue"] = item.RFIDIssue == true ? "Enable" : "Disable";
                dr["Weighment"] = item.Weighment == true ? "Enable" : "Disable";
                dr["Database Operation"] = item.DatabaseOperation == true ? "Enable" : "Disable";
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        //Save Data To The Server
        public string SaveDataToServer(DataSet ds)
        {
            string result = string.Empty;
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                string connection = ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString;
                DataTable dt = ds.Tables[0];
                int _failed = 0;
                int _success = 0;
                int _update = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    string _UserType = dr["User Type"].ToString();
                    bool _MasterFileUpdation = dr["Master File Updation"].ToString() == "Enable" ? true : false;
                    bool _MasterRecordDeletion = dr["Master Record Deletion"].ToString() == "Enable" ? true : false;
                    bool _PendingRecordDeletion = dr["Pending Record Deletion"].ToString() == "Enable" ? true : false;
                    bool _TransactionDeletion = dr["Transaction Deletion"].ToString() == "Enable" ? true : false;
                    bool _Configuration = dr["Configuration"].ToString() == "Enable" ? true : false;
                    bool _PasswordPolicy = dr["Password Policy"].ToString() == "Enable" ? true : false;
                    bool _PasswordReset = dr["Password Reset"].ToString() == "Enable" ? true : false;
                    bool _UserCreation = dr["User Creation"].ToString() == "Enable" ? true : false;
                    bool _GateEntry = dr["Gate Entry"].ToString() == "Enable" ? true : false;
                    bool _RFIDIssue = dr["RFID Issue"].ToString() == "Enable" ? true : false;
                    bool _Weighment = dr["Weighment"].ToString() == "Enable" ? true : false;
                    bool _DatabaseOperation = dr["Database Operation"].ToString() == "Enable" ? true : false;
                    var _Uc = db.UserClassifications.Where(x => x.UserType == _UserType && x.IsDeleted == false).FirstOrDefault();
                    if (_UserType.Length <= 25)
                    {
                        if (_Uc != null)
                        {
                            int id = db.UserClassifications.FirstOrDefault(x => x.UserType == _UserType && x.IsDeleted == false).Id;
                            UserClassification _User = db.UserClassifications.FirstOrDefault(x => x.UserType == _UserType && x.IsDeleted == false && x.Id != id);
                            if (_User != null)
                            {
                                _failed++;
                            }
                            else
                            {
                                _Uc.UserType = _UserType;
                                _Uc.MasterFileUpdation = _MasterFileUpdation;
                                _Uc.MasterRecordDeletion = _MasterRecordDeletion;
                                _Uc.PendingRecordDeletion = _PendingRecordDeletion;
                                _Uc.TransactionDeletion = _TransactionDeletion;
                                _Uc.Configuration = _Configuration;
                                _Uc.PasswordPolicy = _PasswordPolicy;
                                _Uc.PasswordReset = _PasswordReset;
                                _Uc.UserCreation = _UserCreation;
                                _Uc.GateEntry = _GateEntry;
                                _Uc.RFIDIssue = _RFIDIssue;
                                _Uc.Weighment = _Weighment;
                                _Uc.DatabaseOperation = _DatabaseOperation;
                                db.SubmitChanges();
                                _update++;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_UserType))
                            {
                                var data = db.UserClassifications.FirstOrDefault(x => x.UserType == _UserType && x.IsDeleted == false);
                                if (data != null)
                                {
                                    _failed++;
                                }
                                else
                                {
                                    UserClassification uc = new UserClassification();
                                    uc.UserType = _UserType;
                                    uc.MasterFileUpdation = _MasterFileUpdation;
                                    uc.MasterRecordDeletion = _MasterRecordDeletion;
                                    uc.PendingRecordDeletion = _PendingRecordDeletion;
                                    uc.Configuration = _Configuration;
                                    uc.PasswordPolicy = _PasswordPolicy;
                                    uc.PasswordReset = _PasswordReset;
                                    uc.RFIDIssue = _RFIDIssue;
                                    uc.UserCreation = _UserCreation;
                                    uc.TransactionDeletion = _TransactionDeletion;
                                    uc.Weighment = _Weighment;
                                    uc.DatabaseOperation = _DatabaseOperation;
                                    uc.GateEntry = _GateEntry;
                                    uc.IsDeleted = false;
                                    db.UserClassifications.InsertOnSubmit(uc);
                                    db.SubmitChanges();
                                    _success++;
                                }
                            }
                            else
                            {
                                _failed++;
                            }
                        }
                    }
                }

                result = "New Added: "+_success+" Updated:  "+_update+"  Failed:  "+_failed+"";
            }
            return result;
        }

        //Get:UserClassification By UserType
        public Model_UserClassification GetUserClassificationByUserType(string type)
        {
            var t = db.UserClassifications.Where(x => x.UserType == type && x.IsDeleted == false).FirstOrDefault();
            Model_UserClassification uc = new Model_UserClassification();
            try
            {               
                if (t != null)
                {                  
                    uc.Id = t.Id;
                    uc.UserType = t.UserType;
                    uc.MasterFileUpdation = t.MasterFileUpdation == true ? "Enable" : "Disable";
                    uc.MasterRecordDeletion = t.MasterRecordDeletion == true ? "Enable" : "Disable";
                    uc.PendingRecordDeletion = t.PendingRecordDeletion == true ? "Enable" : "Disable";
                    uc.Configuration = t.Configuration == true ? "Enable" : "Disable";
                    uc.PasswordPolicy = t.PasswordPolicy == true ? "Enable" : "Disable";
                    uc.PasswordReset = t.PasswordReset == true ? "Enable" : "Disable";
                    uc.TransactionDeletion = t.TransactionDeletion == true ? "Enable" : "Disable";
                    uc.GateEntry = t.GateEntry == true ? "Enable" : "Disable";
                    uc.RFIDIssue = t.RFIDIssue == true ? "Enable" : "Disable";
                    uc.UserCreation = t.UserCreation == true ? "Enable" : "Disable";
                    uc.Weighment = t.Weighment == true ? "Enable" : "Disable";
                    uc.DatabaseOperation = t.DatabaseOperation == true ? "Enable" : "Disable";                   
                }               
            }
            catch
            {

            }
            return uc;
        }

        //Get:Active UserClassification List
        public IEnumerable<Model_UserClassification> GetUserClassifications_List()
        {
            var data = (from t in db.UserClassifications
                        where t.IsDeleted == false
                        select new Model_UserClassification
                        {
                            Id = t.Id,
                            UserType = t.UserType,
                            MasterFileUpdation = t.MasterFileUpdation == true ? "Enable" : "Disable",
                            MasterRecordDeletion = t.MasterRecordDeletion == true ? "Enable" : "Disable",
                            PendingRecordDeletion = t.PendingRecordDeletion == true ? "Enable" : "Disable",
                            TransactionDeletion = t.TransactionDeletion == true ? "Enable" : "Disable",
                            Configuration = t.Configuration == true ? "Enable" : "Disable",
                            PasswordPolicy = t.PasswordPolicy == true ? "Enable" : "Disable",
                            PasswordReset = t.PasswordReset == true ? "Enable" : "Disable",
                            GateEntry = t.GateEntry == true ? "Enable" : "Disable",
                            RFIDIssue = t.RFIDIssue == true ? "Enable" : "Disable",
                            UserCreation = t.UserCreation == true ? "Enable" : "Disable",
                            Weighment = t.Weighment == true ? "Enable" : "Disable",
                            DatabaseOperation = t.DatabaseOperation == true ? "Enable" : "Disable"
                        }).ToList();
            return data;
        }

        //Get:User Classification By UserType
        public UserClassification GetUserClassification_by_UserType(string type)
        {
            UserClassification uc = db.UserClassifications.FirstOrDefault(x => x.UserType == type && x.IsDeleted == false);
            return uc;
        }

        //Get:User Classification By Id
        public UserClassification GetUserClassification_by_Id(int id)
        {
            UserClassification uc = db.UserClassifications.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return uc;
        }

        //Delete:UserClassification by Id
        public bool Delete_UserClassification(int id)
        {
            bool status = false;
            UserClassification uc = db.UserClassifications.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (uc != null)
            {
                uc.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Add:New UserClassification
        public bool Add_UserClassification(UserClassification user)
        {
            bool status = false;
            if (user != null)
            {
                db.UserClassifications.InsertOnSubmit(user);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }
    }
}