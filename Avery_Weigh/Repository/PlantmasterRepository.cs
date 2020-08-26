using Avery_Weigh.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
namespace Avery_Weigh.Repository
{
    public class PlantmasterRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        RegexRepository regex = new RegexRepository();

        //Get:PlantMaster data list
        public IEnumerable<PlantMaster> Get_PlantList()
        {
            var data = (from a in db.PlantMasters
                        where a.IsDeleted == false
                        select a).ToList();
            return data;
        }

        public PlantMaster getplantByWeighingMachine(string plantCode, string companyCode)
        {
            PlantMaster master = db.PlantMasters.Where(x => x.PlantCode == plantCode && x.CompanyCode == companyCode).FirstOrDefault();
            return master;
        }

        //Get:PlantMaster data into datatable
        public DataTable GetDataTable_PlantMaster()
        {
            var data = db.PlantMasters.Where(x => x.IsDeleted == false).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr No");
            dt.Columns.Add("Company Code");
            dt.Columns.Add("Plant Code");
            dt.Columns.Add("Plant Name");
            dt.Columns.Add("Plant Address1");
            dt.Columns.Add("Plant Address2");
            dt.Columns.Add("Plant Contact Person");
            dt.Columns.Add("Designation");
            dt.Columns.Add("Contact Mobile");
            dt.Columns.Add("Contact Email");
            dt.Columns.Add("No Of Machine");
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["Company Code"] = item.CompanyCode;
                dr["Plant Code"] = item.PlantCode;
                dr["Plant Name"] = item.PlantName;
                dr["Plant Address1"] = item.PlantAddress1;
                dr["Plant Address2"] = item.PlantAddress2;
                dr["Plant Contact Person"] = item.PlantContactPerson;
                dr["Designation"] = item.Designation;
                dr["Contact Mobile"] = item.ContactMobile;
                dr["Contact Email"] = item.ContactEmail;
                dr["No Of Machine"] = item.NoOfMachine;
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        //Save Excel File data to the server
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
                        string _companycode = dr["Company Code"].ToString();
                        string _plantcode = dr["Plant Code"].ToString();
                        string _plantname = dr["Plant Name"].ToString();
                        string _plantcontactperson = dr["Plant Contact Person"].ToString();
                        string _designation = dr["Designation"].ToString();
                        string _contactmobile = dr["Contact Mobile"].ToString();
                        string _contactemail = dr["Contact Email"].ToString();
                        string _plantaddress1 = dr["Plant Address1"].ToString();
                        string _plantaddress2 = dr["Plant Address2"].ToString();
                        int? _noofmachine = Convert.ToInt32(dr["No Of Machine"]);

                        if (_plantcode.Length <= 10 && _plantname.Length <= 25 && _plantcontactperson.Length <= 25 && _designation.Length <= 25 && _contactmobile.Length <= 12 && _plantaddress1.Length <= 25 && _plantaddress2.Length <= 25 && _noofmachine.ToString().Length <= 3)
                        {
                            var _plantmaster = db.PlantMasters.Where(x => x.PlantCode == _plantcode && x.IsDeleted == false).FirstOrDefault();
                            if (_plantmaster != null)
                            {
                                if (string.IsNullOrEmpty(_plantname))
                                {
                                    _failed++;
                                }
                                else
                                {
                                    int id = db.PlantMasters.FirstOrDefault(x => x.PlantCode == _plantcode && x.IsDeleted == false).Id;
                                    var _pm = db.PlantMasters.FirstOrDefault(x => x.PlantCode == _plantcode && x.IsDeleted == false && x.Id != id);
                                    if (_pm != null)
                                    {
                                        _failed++;
                                    }
                                    else
                                    {
                                        if (regex.CheckNumber(_contactmobile) && regex.ValidateEmail(_contactemail))
                                        {
                                            _plantmaster.PlantCode = _plantcode.ToString();
                                            _plantmaster.PlantName = _plantname.ToString();
                                            _plantmaster.Designation = _designation.ToString();
                                            _plantmaster.PlantContactPerson = _plantcontactperson.ToString();
                                            _plantmaster.PlantAddress1 = _plantaddress1.ToString();
                                            _plantmaster.PlantAddress2 = _plantaddress2.ToString();
                                            _plantmaster.ContactEmail = _contactemail.ToString();
                                            _plantmaster.ContactMobile = _contactmobile.ToString();
                                            _plantmaster.NoOfMachine = _noofmachine;
                                            db.SubmitChanges();
                                            _update++;
                                        }
                                        else
                                        {
                                            _failed++;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(_plantcode) && !string.IsNullOrEmpty(_plantname))
                                {
                                    PlantMaster _master = db.PlantMasters.FirstOrDefault(x => x.PlantCode == _plantcode && x.IsDeleted == false);
                                    if (_master != null)
                                    {
                                        _failed++;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            
                                            if (regex.CheckNumber(_contactmobile) && regex.ValidateEmail(_contactemail))
                                            {
                                                PlantMaster plant = new PlantMaster();
                                                plant.PlantCode = _plantcode.ToString();
                                                plant.PlantName = _plantname.ToString();
                                                plant.Designation = _designation.ToString();
                                                plant.PlantContactPerson = _plantcontactperson.ToString();
                                                plant.PlantAddress1 = _plantaddress1.ToString();
                                                plant.PlantAddress2 = _plantaddress2.ToString();
                                                plant.ContactEmail = _contactemail.ToString();
                                                plant.ContactMobile = _contactmobile.ToString();
                                                plant.NoOfMachine = _noofmachine;
                                                plant.IsDeleted = false;
                                                db.PlantMasters.InsertOnSubmit(plant);
                                                db.SubmitChanges();
                                                _success++;
                                            }
                                            else
                                            {
                                                _failed++;
                                            }
                                        }
                                        catch
                                        {
                                            _failed++;
                                        }
                                    }
                                }
                                else
                                {
                                    _failed++;
                                }
                            }
                        }
                    }
                    catch
                    {
                        _failed++;
                    }
                }
                result = "New Added:- "+_success+" Updated:- "+_update+" Failed:- "+_failed+"";
            }
            return result;
        }                  

        //:Get Plant master record by plantcode
        public PlantMaster Get_PlantMaster_By_PlantCode(string code)
        {
            PlantMaster plantMaster = db.PlantMasters.FirstOrDefault(x => x.PlantCode == code && x.IsDeleted == false);
            return plantMaster;
        }

        //Get:Plant Code
        public IEnumerable<Model_PlantMaster> Get_PlantCodeId()
        {
            IEnumerable<Model_PlantMaster> _PlantMasters = (from t in db.PlantMasters
                                                            where t.IsDeleted == false
                                                            select new Model_PlantMaster
                                                            {
                                                                PlantCode = t.PlantCode,
                                                                PlantName = t.PlantName + "(" + t.PlantCode + " )"
                                                            }).ToList();
            return _PlantMasters;
        }

        //Get:Plant Master By Plant Id
        public PlantMaster Get_PlantMaster_by_Id(int id)
        {
            PlantMaster plant = db.PlantMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return plant;
        }

        //Delete:Plant Master By Id
        public bool Delete_PlantById(int id)
        {
            bool status = false;
            PlantMaster plant = db.PlantMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (plant != null)
            {
                plant.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Add:New plant master
        public bool Add_PlantMaster(PlantMaster plant)
        {
            bool status = false;
            if (plant != null)
            {
                db.PlantMasters.InsertOnSubmit(plant);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }
    }
}