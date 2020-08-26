using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class VehicleClassificationRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
       

        public VehicleClassification GetTruckVehicleClassification(string TruckNo)
        {
            VehicleClassification VC = (from a in db.TruckMasters
                                       join b in db.VehicleClassifications on a.ClassificationCode equals b.Id.ToString()
                                       where a.TruckRegNo == TruckNo
                                       select b).SingleOrDefault();
            return VC;
        }

        //Get:VehicleClassification list with vcmodel
        public IEnumerable<Model_VehicleClassification> Get_Model_VehicleClassificationsList()
        {
            var data = (from t in db.VehicleClassifications
                        where t.IsDeleted == false
                        select new Model_VehicleClassification
                        {
                            Id = t.Id,
                            ClassificationCode = t.ClassificationCode,
                            Make = t.Make,
                            Model = t.Model,
                            NoOfAxies = t.NoOfAxies,
                            BodyType = t.BodyType,
                            KerbWT = t.KerbWt,
                            ManufactureYear = t.ManufactureYear,
                            GrossWeight = t.GrossWeight,
                            UOMWeight = t.UOMWeight
                        }).ToList();
            return data;
        }

        //Get vehicleclassification by code
        public Model_VehicleClassification Get_Model_VehicleClassificationByCode(string code)
        {
           var data = db.VehicleClassifications.Where(x => x.ClassificationCode == code && x.IsDeleted == false).FirstOrDefault();
            Model_VehicleClassification _model = new Model_VehicleClassification();
            if (data != null)
            {
                _model.Id = data.Id;
                _model.ClassificationCode = data.ClassificationCode;
                _model.Make = data.Make;
                _model.Model = data.Model;
                _model.NoOfAxies = data.NoOfAxies;
                _model.BodyType = data.BodyType;
                _model.KerbWT = data.KerbWt;
                _model.ManufactureYear = data.ManufactureYear;
                _model.GrossWeight = data.GrossWeight;
                _model.UOMWeight = data.UOMWeight;
            }
            return _model;
        }

        //Return vehicleclassification datatable
        public DataTable Get_VCDataTable()
        {
            var data = db.VehicleClassifications.Where(x => x.IsDeleted == false).ToList();
            DataTable dt = new DataTable();
            if (data != null)
            {
               
                dt.Columns.Add("Sr No");
                dt.Columns.Add("ClassificationCode");
                dt.Columns.Add("Make");
                dt.Columns.Add("Model");
                dt.Columns.Add("NoOfAxies");
                dt.Columns.Add("BodyType");
                dt.Columns.Add("KerbWt");
                dt.Columns.Add("ManufactureYear");
                dt.Columns.Add("GrossWeight");
                dt.Columns.Add("UOMWeight");           
            }
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["ClassificationCode"] = item.ClassificationCode;
                dr["Make"] = item.Make;
                dr["Model"] = item.Model;
                dr["NoOfAxies"] = item.NoOfAxies;
                dr["BodyType"] = item.BodyType;
                dr["KerbWt"] = item.KerbWt;
                dr["ManufactureYear"] = item.ManufactureYear;
                dr["GrossWeight"] = item.GrossWeight;
                dr["UOMWeight"] = item.UOMWeight;
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        //Save Excel data to server
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
                StringBuilder sb = new StringBuilder();
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        string _vehicleclassificationcode = dr["ClassificationCode"].ToString();
                        string _make = dr["Make"].ToString();
                        string _model = dr["Model"].ToString();
                        decimal _noofaxies = Convert.ToDecimal(dr["NoOfAxies"]);
                        string _bodytype = dr["BodyType"].ToString();
                        decimal _kerbwt = Convert.ToDecimal(dr["KerbWt"]);
                        int _manufactureyear = Convert.ToInt32(dr["ManufactureYear"]);
                        decimal _grossweight = Convert.ToDecimal(dr["GrossWeight"]);
                        string _uomweight = dr["UOMWeight"].ToString();
                        var _vc = db.VehicleClassifications.Where(x => x.ClassificationCode == _vehicleclassificationcode && x.IsDeleted == false).FirstOrDefault();
                        if (_vc != null)
                        {
                            try
                            {
                                _vc.Make = _make;
                                _vc.Model = _model;
                                _vc.BodyType = _bodytype;
                                _vc.NoOfAxies = _noofaxies;
                                _vc.KerbWt = _kerbwt;
                                _vc.ManufactureYear = _manufactureyear;
                                _vc.GrossWeight = _grossweight;
                                _vc.UOMWeight = _uomweight;
                                db.SubmitChanges();
                                _update++;
                            }
                            catch
                            {
                                _failed++;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_vehicleclassificationcode))
                            {
                                VehicleClassification _vlc = db.VehicleClassifications.Where(x => x.ClassificationCode == _vehicleclassificationcode && x.IsDeleted == false).FirstOrDefault();
                                if (_vlc != null)
                                {
                                    _failed++;
                                }
                                else
                                {
                                    try
                                    {
                                        VehicleClassification _v = new VehicleClassification();
                                        _v.ClassificationCode = _vehicleclassificationcode;
                                        _v.Make = _make;
                                        _v.Model = _model;
                                        _v.NoOfAxies = _noofaxies;
                                        _v.BodyType = _bodytype;
                                        _v.KerbWt = _kerbwt;
                                        _v.ManufactureYear = _manufactureyear;
                                        _v.GrossWeight = _grossweight;
                                        _v.UOMWeight = _uomweight;
                                        _v.IsDeleted = false;
                                        db.VehicleClassifications.InsertOnSubmit(_v);
                                        db.SubmitChanges();
                                        _success++;
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
                    catch
                    {
                        _failed++;
                    }                  
                }
                result = "New Added: " + _success + "  Updated:  " + _update + "  Failed:  " + _failed + "";
            }
            return result;
        }

        //Add:New Vehicleclassification record
        public bool Add_vc(VehicleClassification vc)
        {
            bool status = false;
            if (vc != null)
            {
                db.VehicleClassifications.InsertOnSubmit(vc);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Delete:VehicleClassification by id
        public bool Delete_vc(int id)
        {
            bool status = false;
            VehicleClassification vc = db.VehicleClassifications.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (vc != null)
            {
                vc.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Get:Vehicleclassification by id
        public VehicleClassification Get_vcById(int id)
        {
            VehicleClassification vc = db.VehicleClassifications.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return vc;
        }

        //Return vehicleclassification list
        public IEnumerable<VehicleClassification> Get_VehicleClassification_List()
        {
            IEnumerable<VehicleClassification> list = db.VehicleClassifications.Where(x => x.IsDeleted == false);
            return list;
        }

        //Get:VehicleClassification Code
        public IEnumerable<Model_VehicleClassification> Get_VehicleClassificationCode()
        {
            IEnumerable<Model_VehicleClassification> list = (from t in db.VehicleClassifications
                                                             where t.IsDeleted == false
                                                             select new Model_VehicleClassification
                                                             {
                                                                 ClassificationCode = t.ClassificationCode,
                                                                 Make = t.Make + "( " + t.ClassificationCode + " )"
                                                             }).ToList();
            return list;
        }


        //Get:VehicleClassification By Classification code
        public VehicleClassification Get_VehicleClassificationByCode(string code)
        {
            VehicleClassification vc = db.VehicleClassifications.FirstOrDefault(x => x.ClassificationCode == code && x.IsDeleted == false);
            return vc;
        }
    }
}