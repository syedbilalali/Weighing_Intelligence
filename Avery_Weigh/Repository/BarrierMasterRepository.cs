using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class BarrierMasterRepository
    {
        /// <summary>
        /// This class contains all the method which will use to Perform all the 
        /// database operation with the barrier master database.
        /// </summary>
        DataClasses1DataContext db = new DataClasses1DataContext();
     
        //Get:Barrier Master Data List using Model
        public IEnumerable<Model_BarrierMaster> GetAllBarrierMasterList()
        {
            var data = (from t in db.BarrierMasters
                        where t.IsDeleted == false
                        select new Model_BarrierMaster
                        {
                            Id = t.Id,
                            PlantCodeId = t.PlantCodeId,
                            MachineId = t.MachineId,
                            BarrierIdentification = t.BarrierIdentification,
                            BarrierIP = t.BarrierIP,
                            BarrierPORT = t.BarrierPort,
                            BarrierScheme = t.BarrierScheme
                        }).ToList();
            return data;            
        }

        //Get:BarrierMaster By PlantCode
        public BarrierMaster GetBarrierByCode(string code)
        {
            var data = db.BarrierMasters.Where(x => x.PlantCodeId == code && x.IsDeleted == false).FirstOrDefault();
            return data;
        }

        //Get: Active BarrierMaster List
        public IEnumerable<BarrierMaster> GetBarrierMasters_List()
        {
            var data = db.BarrierMasters.Where(x => x.IsDeleted == false).ToList();
            return data;
        }

        //Get:Barrier Master DataTable Which Contains all Data List
        public DataTable GetBarrierMasterDataTable()
        {
            var data = (from t in db.BarrierMasters
                        where t.IsDeleted == false
                        select t).ToList();

            DataTable dt = new DataTable();
            DataColumn srno = new DataColumn("Sr No");
            DataColumn plantcode = new DataColumn("Plant Code");
            DataColumn machinid = new DataColumn("Machine Id");
            DataColumn bridentification = new DataColumn("Barrier Identification");
            DataColumn ip = new DataColumn("Barrier IP");
            DataColumn port = new DataColumn("Barrier PORT");
            DataColumn scheme = new DataColumn("Barrier Scheme");
            dt.Columns.Add(srno);
            dt.Columns.Add(plantcode);
            dt.Columns.Add(machinid);
            dt.Columns.Add(bridentification);
            dt.Columns.Add(ip);
            dt.Columns.Add(port);
            dt.Columns.Add(scheme);
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["Plant Code"] = item.PlantCodeId.ToString();
                dr["Machine Id"] = item.MachineId;
                dr["Barrier Identification"] = item.BarrierIdentification;
                dr["Barrier IP"] = item.BarrierIP;
                dr["Barrier PORT"] = item.BarrierPort;
                dr["Barrier Scheme"] = item.BarrierScheme;
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        //Save:Excel Data To The Server
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
                        string _plantcodeid = dr["Plant Code"].ToString();
                        string _machinid = dr["Machine Id"].ToString();
                        string _barrieridentification = dr["Barrier Identification"].ToString();
                        string _barrierip = dr["Barrier IP"].ToString();
                        string _barrierport = dr["Barrier PORT"].ToString();
                        string _barrierscheme = dr["Barrier Scheme"].ToString();
                        var _bm = db.BarrierMasters.Where(x => x.BarrierIP == _barrierip && x.IsDeleted == false).FirstOrDefault();
                        if (_bm != null)
                        {
                            var _plantcode = db.PlantMasters.Where(x => x.PlantCode == _plantcodeid && x.IsDeleted == false).FirstOrDefault();
                            var _mid = db.WeightMachineMasters.Where(x => x.MachineId == _machinid && x.IsDeleted == false).FirstOrDefault();
                            if (_plantcode != null && _mid != null)
                            {
                                _bm.PlantCodeId = _plantcodeid;
                                _bm.MachineId = _machinid;
                                _bm.BarrierIdentification = _barrieridentification;
                                _bm.BarrierPort = _barrierport;
                                if (_barrierscheme == "N/C" || _barrierscheme == "N/O")
                                {
                                    _bm.BarrierScheme = _barrierscheme;
                                }
                                else
                                {
                                    _bm.BarrierScheme = "";
                                }
                                _update++;
                                db.SubmitChanges();
                            }
                            else
                            {
                                _failed++;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_barrierip))
                            {
                                BarrierMaster _brm = db.BarrierMasters.Where(x => x.BarrierIP == _barrierip && x.IsDeleted == false).FirstOrDefault();
                                if (_brm != null)
                                {
                                    _failed++;
                                }
                                else
                                {
                                    if (_barrierscheme == "N/C" || _barrierscheme == "N/O")
                                    {

                                    }
                                    else
                                    {
                                        _barrierscheme = "";
                                    }
                                    db.BarrierMasters.InsertOnSubmit(new BarrierMaster
                                    {
                                        PlantCodeId = _plantcodeid,
                                        MachineId = _machinid,
                                        BarrierIdentification = _barrieridentification,
                                        BarrierIP = _barrierip,
                                        BarrierPort = _barrierport,
                                        BarrierScheme = _barrierscheme,
                                        IsDeleted = false
                                    });
                                    _success++;
                                    db.SubmitChanges();
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

        //Add:New Barrier Master Record
        public bool Add_BarrierMaster(BarrierMaster bar)
        {
            bool status = false;
            if (bar != null)
            {
                db.BarrierMasters.InsertOnSubmit(bar);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Delete:Barrier Master By Id
        public bool Delete_BarrierMaster(int id)
        {
            bool status = false;
            BarrierMaster master = db.BarrierMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (master != null)
            {
                master.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Get:Barrier Master by Id
        public BarrierMaster Get_BarrierMasterBy_Id(int id)
        {
            BarrierMaster barrier = db.BarrierMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return barrier;
        }
    }
}