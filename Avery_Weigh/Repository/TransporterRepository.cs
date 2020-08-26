using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class TransporterRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public IEnumerable<Model_Transporter> Get_Transporters()
        {
            var data = (from t in db.tblTransporters
                        select new Model_Transporter
                        {
                            ddCode =  t.Code,
                            Name = t.Name + " (" + t.Code + " )"
                        }).ToList();
            return data;
        }

        //Get:Transporter DataTable
        public DataTable GetTransporterDataTable()
        {
            var data = db.tblTransporters.Where(x => x.IsDeleted == false).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr No");
            dt.Columns.Add("Code");
            dt.Columns.Add("Name");
            dt.Columns.Add("Address1");
            dt.Columns.Add("Address2");
            dt.Columns.Add("City");
            dt.Columns.Add("State");
            dt.Columns.Add("Country");
            dt.Columns.Add("GST No");
            dt.Columns.Add("PAN No");
            dt.Columns.Add("Contact Person");
            dt.Columns.Add("Contact Mobile");
            dt.Columns.Add("Contact Email");
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["Code"] = item.Code;
                dr["Name"] = item.Name;
                dr["Address1"] = item.Address1;
                dr["Address2"] = item.Address2;
                dr["City"] = item.City;
                dr["State"] = item.State;
                dr["Country"] = item.Country;
                dr["GST No"] = item.GSTNo;
                dr["PAN No"] = item.PanNo;
                dr["Contact Person"] = item.ContactPerson;
                dr["Contact Mobile"] = item.ContactMobile;
                dr["Contact Email"] = item.ContactEmail;
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        internal IList<DynamicFieldName> GetFieldNameByMachine(string machineID, string PlantId)
        {
            IList<DynamicFieldName> data = db.DynamicFieldNames.Where(x => x.MachineId == machineID && x.PlantId == PlantId).ToList();
            return data.ToList();
        }

        //Get:Transporter List Using Model
        public IEnumerable<Model_Transporter> GetTransporters_List()
        {
            var data = (from t in db.tblTransporters
                        where t.IsDeleted == false
                        select new Model_Transporter
                        {
                            Id = t.Id,
                            Code = t.Code,
                            Name = t.Name,
                            Address1 = t.Address1,
                            Address2 = t.Address2,
                            City = t.City,
                            State = t.State,
                            Country = t.Country,
                            GSTNo = t.GSTNo,
                            PANNo = t.PanNo,
                            ContactPerson = t.ContactPerson,
                            ContactMobile = t.ContactMobile,
                            ContactEmail = t.ContactEmail
                        }).ToList();
            return data;
        }

        //Get:Active Tranporter List
        public IEnumerable<tblTransporter> GetTransportersList()
        {
            IList<tblTransporter> data = db.tblTransporters.Where(x => x.IsDeleted == false).ToList();
            return data;
        }

        //Get:Transporter by transporter code
        public tblTransporter GetTransporter_ByCode(string code)
        {
            tblTransporter trans = db.tblTransporters.Where(x => x.Code == code && x.IsDeleted == false).SingleOrDefault();
            return trans;
        }

        //Save:Excel data to the server
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
                    try
                    {
                        string _code = dr["Code"].ToString();
                        string _name = dr["Name"].ToString();
                        string _address1 = dr["Address1"].ToString();
                        string _address2 = dr["Address2"].ToString();
                        string _city = dr["City"].ToString();
                        string _state = dr["State"].ToString();
                        string _country = dr["Country"].ToString();
                        string _gstno = dr["GST No"].ToString();
                        string _panno = dr["PAN No"].ToString();
                        string _contactperson = dr["Contact Person"].ToString();
                        string _contactmobile = dr["Contact Mobile"].ToString();
                        string _contactemail = dr["Contact Email"].ToString();
                        var _transporter = db.tblTransporters.Where(x => x.Code == _code && x.IsDeleted == false).FirstOrDefault();
                        if (_transporter != null)
                        {
                            if (string.IsNullOrEmpty(_name))
                            {
                                _failed++;
                            }
                            else
                            {
                                _transporter.Name = _name;
                                _transporter.Address1 = _address1;
                                _transporter.Address2 = _address2;
                                _transporter.City = _city;
                                _transporter.State = _state;
                                _transporter.Country = _country;
                                _transporter.GSTNo = _gstno;
                                _transporter.PanNo = _panno;
                                _transporter.ContactPerson = _contactperson;
                                _transporter.ContactMobile = _contactmobile;
                                _transporter.ContactEmail = _contactemail;
                                db.SubmitChanges();
                                _update++;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_code) && !string.IsNullOrEmpty(_name))
                            {
                                tblTransporter _trans = db.tblTransporters.Where(x => x.Code == _code && x.IsDeleted == false).FirstOrDefault();
                                if (_trans != null)
                                {
                                    _failed++;
                                }
                                else
                                {
                                    db.tblTransporters.InsertOnSubmit(new tblTransporter
                                    {
                                        Code = _code,
                                        Name = _name,
                                        Address1 = _address1,
                                        Address2 = _address2,
                                        City = _city,
                                        State = _state,
                                        Country = _country,
                                        GSTNo = _gstno,
                                        PanNo = _panno,
                                        ContactPerson = _contactperson,
                                        ContactMobile = _contactmobile,
                                        ContactEmail = _contactemail,
                                        IsDeleted = false
                                    });
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
                    catch { _failed++; }
                }
                result = "New Added: " + _success + "  Updated:  " + _update + "  Failed:  " + _failed + "";
            }
            return result;
        }

        //Delete:Tranporter by Id
        public bool Delete_TransporterById(int id)
        {
            bool status = false;
            tblTransporter tran = db.tblTransporters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (tran != null)
            {
                tran.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;

        } 

        //Get:Transporter By Id
        public tblTransporter Get_TransporterById(int id)
        {
            tblTransporter tran = db.tblTransporters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return tran;
        }

        //Get:Transporter code
        public IEnumerable<Model_Transporter> Get_TransporterCode()
        {
            IEnumerable<Model_Transporter> list = (from t in db.tblTransporters
                                                   where t.IsDeleted == false
                                                   select new Model_Transporter
                                                   {
                                                       Code = t.Code,
                                                       Name = t.Name + "( " + t.Code + " )"
                                                   }).ToList();
            return list;
        }

        //Add:New Transporter Record
        public bool Add_Transporter(tblTransporter tran)
        {
            bool status = false;
            if (tran != null)
            {
                db.tblTransporters.InsertOnSubmit(tran);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }
    }
}