using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class TruckMasterRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        //: Get Truck Master By Truck Number
        public TruckMaster GetTruckMasterByTruckNo(string TruckNo)
        {
            TruckMaster _truck = db.TruckMasters.FirstOrDefault(x => x.TruckRegNo == TruckNo && x.IsDeleted == false);
            return _truck;
        }

        //:Get Truck Master List
        public IEnumerable<TruckMaster> GetTruckMasters_List()
        {
            var data = db.TruckMasters.Where(x => x.IsDeleted == false).ToList();
            return data;
        }
       
        //:Get Truck master datatable
        public DataTable GetDataTable_TruckMaster()
        {
            DataTable dt = new DataTable();
            IEnumerable<TruckMaster> data = db.TruckMasters.Where(x => x.IsDeleted == false).ToList();
            dt.Columns.Add("Sr No");
            dt.Columns.Add("Truck Reg No");
            dt.Columns.Add("Classification Code");
            dt.Columns.Add("Store Tare Weight");
            dt.Columns.Add("Tare Validity Date");
            dt.Columns.Add("Average Tare Scheme");
            dt.Columns.Add("Current Average Tare Value");
            dt.Columns.Add("UOM Weight");
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["Truck Reg No"] = item.TruckRegNo;
                dr["Classification Code"] = item.ClassificationCode;
                dr["Store Tare Weight"] = item.StoredTareWeight;
                dr["Tare Validity Date"] = item.TareValidityDate;
                dr["Average Tare Scheme"] = item.AverageTareScheme;
                dr["Current Average Tare Value"] = item.CurrentAverageTareValue;
                dr["UOM Weight"] = item.UOMWeight;
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        //:Save excel data to database
        internal string SaveDataToServer(DataSet ds)
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
                        string _TruckRegNo = dr["Truck Reg No"].ToString();
                        string _ClassificationCode = dr["Classification Code"].ToString();
                        string _StoreTareWeight = dr["Store Tare Weight"].ToString();
                        string _TareValidityDate = dr["Tare Validity Date"].ToString();
                        string _AveraTareScheme = dr["Average Tare Scheme"].ToString();
                        string _CurrentAverageTareValue = dr["Current Average Tare Value"].ToString();
                        string _UOMWeight = dr["UOM Weight"].ToString();
                        TruckMaster _TruckMaster = db.TruckMasters.Where(x => x.TruckRegNo == _TruckRegNo && x.IsDeleted == false).FirstOrDefault();
                        VehicleClassification _vc = db.VehicleClassifications.FirstOrDefault(x => x.ClassificationCode == _ClassificationCode && x.IsDeleted == false);
                        if (_TruckMaster != null)
                        {
                            if (string.IsNullOrEmpty(_ClassificationCode))
                            {
                                _failed++;
                            }
                            else
                            {
                                _TruckMaster.ClassificationCode = _ClassificationCode;
                                _TruckMaster.StoredTareWeight = _StoreTareWeight;
                                _TruckMaster.TareValidityDate = _TareValidityDate;
                                _TruckMaster.AverageTareScheme = _AveraTareScheme;
                                _TruckMaster.CurrentAverageTareValue = _CurrentAverageTareValue;
                                _TruckMaster.UOMWeight = _UOMWeight;
                                db.SubmitChanges();
                                _update++;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_TruckRegNo) && !string.IsNullOrEmpty(_ClassificationCode))
                            {

                                if (_vc == null)
                                {
                                    _failed++;
                                }
                                else
                                {
                                    TruckMaster t = new TruckMaster();
                                    t.TruckRegNo = _TruckRegNo;
                                    t.ClassificationCode = _ClassificationCode;
                                    t.AverageTareScheme = _AveraTareScheme;
                                    t.CurrentAverageTareValue = _CurrentAverageTareValue;
                                    t.StoredTareWeight = _StoreTareWeight;
                                    t.TareValidityDate = _TareValidityDate;
                                    t.UOMWeight = _UOMWeight;
                                    t.IsDeleted = false;
                                    db.TruckMasters.InsertOnSubmit(t);
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

        //Add:New Truck Master Record
        public bool Add_TruckMaster(TruckMaster truck)
        {
            bool status = false;
            if (truck != null)
            {
                db.TruckMasters.InsertOnSubmit(truck);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Delete:Truck Master record by Id
        public bool Delete_TruckMaster(int id)
        {
            bool status = false;
            TruckMaster truck = db.TruckMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (truck != null)
            {
                truck.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Get:TruckMaster By Id
        public TruckMaster Get_TruckMasterById(int id)
        {
            TruckMaster truck = db.TruckMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return truck;
        }
    }
}