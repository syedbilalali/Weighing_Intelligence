using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class SupplierRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        //Get:Supplier Code
        public IEnumerable<Model_Supplier> Get_SupplierCode()
        {
            var data = (from t in db.tblSuppliers
                        where t.IsDeleted == false
                        select new Model_Supplier
                        {
                            Code = t.Code,
                            Name = t.Name + " (" + t.Code + " )"
                        }).ToList();
            return data;
        }

        //Get:Supplier DataTable
        public DataTable Get_SupplierDataTable()
        {
            var data = db.tblSuppliers.Where(x=>x.IsDeleted == false).ToList();
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
                dr["Contact Email"] = item.ContactEMail;
                dt.Rows.Add(dr);
                index++;
            }
            return dt;

        }

        //Get:Supplier By Code
        internal tblSupplier Get_SupplierbyCode(string code)
        {
            tblSupplier obj = db.tblSuppliers.FirstOrDefault(x => x.Code == code && x.IsDeleted == false);
            return obj;
        }

        //Get:Active Supplier List
        internal IEnumerable<tblSupplier> Get_SuppliersList()
        {
            IList<tblSupplier> data = db.tblSuppliers.Where(x => x.IsDeleted == false).ToList();
            return data;
        }

        //Save Excel Data To Server
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
                    var _supplier = db.tblSuppliers.Where(x => x.Code == _code && x.IsDeleted == false).FirstOrDefault();
                    if (_supplier != null)
                    {
                        if (string.IsNullOrEmpty(_name))
                        {
                            _failed++;
                        }
                        else
                        {
                            _supplier.Name = _name;
                            _supplier.Address1 = _address1;
                            _supplier.Address2 = _address2;
                            _supplier.City = _city;
                            _supplier.State = _state;
                            _supplier.Country = _country;
                            _supplier.GSTNo = _gstno;
                            _supplier.IsDeleted = false;
                            _supplier.PanNo = _panno;
                            _supplier.ContactPerson = _contactperson;
                            _supplier.ContactMobile = _contactmobile;
                            _supplier.ContactEMail = _contactemail;
                            db.SubmitChanges();
                            _update++;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(_code) && !string.IsNullOrEmpty(_name))
                        {
                            tblSupplier _tblSupplier = db.tblSuppliers.Where(x => x.Code == _code && x.IsDeleted == false).FirstOrDefault();
                            if (_tblSupplier != null)
                            {
                                _failed++;
                            }
                            else
                            {
                                db.tblSuppliers.InsertOnSubmit(new tblSupplier
                                {
                                    Code = _code,
                                    Name = _name,
                                    Address1 = _address1,
                                    Address2 = _address2,
                                    City = _city,
                                    State = _state,
                                    Country = _country,
                                    GSTNo = _gstno,
                                    IsDeleted = false,
                                    PanNo = _panno,
                                    ContactPerson = _contactperson,
                                    ContactMobile = _contactmobile,
                                    ContactEMail = _contactemail
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
                result = "New Added: " + _success + "  Updated:  " + _update + "  Failed:  " + _failed + "";
            }
            return result;
        }

        //Get:Supplier By Id
        public tblSupplier Get_SupplierById(int id)
        {
            tblSupplier sup = db.tblSuppliers.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return sup;
        }

        //Delete:Supplier By Id
        public bool Delete_SupplierById(int id)
        {
            bool status = false;
            tblSupplier sup = db.tblSuppliers.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (sup != null)
            {
                sup.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        public bool Add_Supplier(tblSupplier sup)
        {
            bool status = false;
            if (sup != null)
            {
                db.tblSuppliers.InsertOnSubmit(sup);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }
    }
}