using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class PackingRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        
        //Get:Packing Code
        public IEnumerable<Model_Packing> Get_PackingCode()
        {
            var data = (from t in db.PackingMasters
                        select new Model_Packing
                        {
                            PackingCode = t.PackingCode,
                            Name = t.PackingName + " (" + t.PackingCode + " )"
                        }).ToList();
            return data;
        }
        
        //: Return Packing Master DataTable
        public DataTable GetPackingDataTable()
        {
            var data = db.PackingMasters.Where(x => x.IsDeleted == false).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr No");
            dt.Columns.Add("Packing Code");
            dt.Columns.Add("Packing Name");
            dt.Columns.Add("Packing UOM");
            dt.Columns.Add("Packing WT");
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["Packing Code"] = item.PackingCode;
                dr["Packing Name"] = item.PackingName;
                dr["Packing UOM"] = item.PackingUOM;
                dr["Packing WT"] = item.PackingWT;
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        //Get:PackingMaster List
        public IEnumerable<PackingMaster> GetPackingMasters_List()
        {
            var data = db.PackingMasters.Where(x => x.IsDeleted == false).ToList();
            return data;
        }

        //Get:Packing Master By Packing Code
        public PackingMaster Get_PackingByCode(string code)
        {
            var data = db.PackingMasters.FirstOrDefault(x => x.PackingCode == code && x.IsDeleted == false);
            return data;
        }
        
        //Save: Excel File Data To Database
        public string SaveDataToSever(DataSet ds)
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
                        string _PackingCode = dr["Packing Code"].ToString();
                        string _PackingName = dr["Packing Name"].ToString();
                        string _PackingUOM = dr["Packing UOM"].ToString();
                        string _PackingWT = dr["Packing WT"].ToString();
                        var _PackingMaster = db.PackingMasters.Where(x => x.PackingCode == _PackingCode && x.IsDeleted == false).FirstOrDefault();
                        if (_PackingMaster != null)
                        {
                            if (string.IsNullOrEmpty(_PackingName))
                            {
                                _failed++;
                            }
                            else
                            {
                                _PackingMaster.PackingName = _PackingName;
                                _PackingMaster.PackingUOM = _PackingUOM;
                                _PackingMaster.PackingWT = _PackingWT;
                                db.SubmitChanges();
                                _update++;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_PackingCode) && !string.IsNullOrEmpty(_PackingName))
                            {
                                PackingMaster _packingmaster = db.PackingMasters.Where(x => x.PackingCode == _PackingCode && x.IsDeleted == false).FirstOrDefault();
                                if (_packingmaster != null)
                                {
                                    _failed++;
                                }
                                else
                                {
                                    db.PackingMasters.InsertOnSubmit(new PackingMaster
                                    {
                                        PackingCode = _PackingCode,
                                        PackingName = _PackingName,
                                        PackingUOM = _PackingUOM,
                                        PackingWT = _PackingWT,
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

        //Add:New Packing Master record
        public bool Add_PackingMaster(PackingMaster pack)
        {
            bool status = false;
            if (pack != null)
            {
                db.PackingMasters.InsertOnSubmit(pack);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Delete:PackingMaster by id
        public bool Delete_PackingMaster(int id)
        {
            bool status = false;
            PackingMaster pack = db.PackingMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (pack != null)
            {
                pack.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Get:PackingMaster By Id
        public PackingMaster Get_PackingMasterById(int id)
        {
            PackingMaster pack = db.PackingMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return pack;
        }
    }
}