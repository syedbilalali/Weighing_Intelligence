using Avery_Weigh.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Avery_Weigh.Repository;

namespace Avery_Weigh.Repository
{
    public class CameraMasterRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
       
        readonly RegexRepository Regex = new RegexRepository();
        
        //Get:CameraMaster List
        public IEnumerable<Model_CameraMaster> GetAllCameraMasterList()
        {
            var data = (from t in db.CameraMasters
                        where t.IsDeleted == false
                        select new Model_CameraMaster
                        {
                            Id = t.Id,
                            PlantCode = t.PlantCodeID,
                            MachineId = t.MachineId,
                            CameraIdentification = t.CameraIndentification,
                            CameraIP = t.CameraIP,
                            CameraPORT = t.CameraPort
                        }).ToList();
            return data;
        }

        //Get:CameraMaster Data In DataTable
        public DataTable Get_CameraMaster_DataTable()
        {
            var data = db.CameraMasters.Where(x => x.IsDeleted == false).ToList();

            DataTable dt = new DataTable();
            DataColumn srno = new DataColumn("Sr No");
            DataColumn plantcode = new DataColumn("Plant Code Id");
            DataColumn machinid = new DataColumn("Machine Id");
            DataColumn bridentification = new DataColumn("Camera Identification");
            DataColumn ip = new DataColumn("Camera IP");
            DataColumn port = new DataColumn("Camera PORT");
            dt.Columns.Add(srno);
            dt.Columns.Add(plantcode);
            dt.Columns.Add(machinid);
            dt.Columns.Add(bridentification);
            dt.Columns.Add(ip);
            dt.Columns.Add(port);
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["Plant Code Id"] = item.PlantCodeID;
                dr["Machine Id"] = item.MachineId;
                dr["Camera Identification"] = item.CameraIndentification;
                dr["Camera IP"] = item.CameraIP;
                dr["Camera PORT"] = item.CameraPort;
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        //Get:Active CameraMaster List
        public IEnumerable<CameraMaster> GetCameraMasters_List()
        {
            var data = db.CameraMasters.Where(x => x.IsDeleted == false).ToList();
            return data;
        }

        //Get:CameraMaster by PlantCode
        public CameraMaster GetCameraMaster_ByCode(string code)
        {
            var data = db.CameraMasters.FirstOrDefault(x => x.PlantCodeID == code && x.IsDeleted == false);
            return data;
        }
    
        //Save:Excel file data to the server
        public string SaveDataToServer(DataSet ds)
        {
            string result = string.Empty;
            int success = 0;
            int fail = 0;
            int update = 0;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string PlantCode = dr["Plant Code Id"].ToString();
                    string MachineId = dr["Machine Id"].ToString();
                    string CameraIdentification = dr["Camera Identification"].ToString();
                    string CameraIP = dr["Camera IP"].ToString();
                    string CameraPort = dr["Camera Port"].ToString();
                    int a =  CameraPort.Count();                   
                    if(CameraIdentification.Length <= 10 && CameraIP.Length <= 15 && CameraPort.Length <= 5)
                    {
                        var PlantMaster = db.PlantMasters.Where(x => x.PlantCode == PlantCode && x.IsDeleted == false).ToList();
                        var MachineMaster = db.WeightMachineMasters.Where(x => x.MachineId == MachineId && x.IsDeleted == false).ToList();
                        if(PlantMaster!=null && MachineMaster != null)
                        {
                            var data = db.CameraMasters.FirstOrDefault(x => x.PlantCodeID == PlantCode && x.MachineId == MachineId && x.CameraIP == CameraIP && x.IsDeleted == false);
                            if (data != null)
                            {
                                int count = db.CameraMasters.Count(x => x.PlantCodeID == PlantCode && x.MachineId == MachineId && x.IsDeleted == false);
                                var checkip = db.CameraMasters.FirstOrDefault(x => x.PlantCodeID == PlantCode && x.CameraIP == CameraIP);
                                if(count > 3 && checkip != null)
                                {
                                    fail++;
                                }
                                else
                                {
                                    var cam = db.CameraMasters.FirstOrDefault(x => x.PlantCodeID == PlantCode && x.MachineId == MachineId && x.IsDeleted == false);                                   
                                    if (Regex.CheckIPAddress(CameraIP))
                                    {
                                        cam.PlantCodeID = PlantCode;
                                        cam.MachineId = MachineId;
                                        cam.CameraIndentification = CameraIdentification;
                                        cam.CameraIP = CameraIP;
                                        cam.CameraPort = CameraPort;
                                        db.SubmitChanges();
                                        update++;
                                    }
                                    else { fail++; }                                                                   
                                }
                            }
                            else
                            {
                                var Plant = db.PlantMasters.Where(x => x.PlantCode == PlantCode && x.IsDeleted == false).ToList();
                                var Machine = db.WeightMachineMasters.Where(x => x.MachineId == MachineId && x.IsDeleted == false).ToList();
                                if (Plant.Count > 0 && Machine.Count > 0)
                                {
                                    int count = db.CameraMasters.Count(x => x.PlantCodeID == PlantCode && x.MachineId == MachineId && x.IsDeleted == false);
                                    var checkip = db.CameraMasters.FirstOrDefault(x => x.PlantCodeID == PlantCode && x.CameraIP == CameraIP);
                                    if (count < 3 && checkip == null)
                                    {
                                        CameraMaster cam = new CameraMaster();
                                        bool st = Regex.CheckIPAddress(CameraIP);
                                        if (Regex.CheckIPAddress(CameraIP))
                                        {
                                            cam.PlantCodeID = PlantCode;
                                            cam.MachineId = MachineId;
                                            cam.CameraIndentification = CameraIdentification;
                                            cam.CameraIP = CameraIP;
                                            cam.CameraPort = CameraPort;
                                            cam.IsDeleted = false;
                                            db.CameraMasters.InsertOnSubmit(cam);
                                            db.SubmitChanges();
                                            success++;
                                        }
                                        else { fail++; }
                                    }
                                    else
                                    {
                                        fail++;
                                    }
                                }
                                else { fail++; }
                            }
                        }
                    }
                }
                catch
                {
                    fail++;
                }
            }
            result = "New Added:"+success+"  Updated:"+update+"   Failed:"+fail+"";
            return result;
        }

        //Add:New CameraMaster Record
        public bool Add_CameraMaster(CameraMaster cam)
        {
            bool status = false;
            if (cam != null)
            {
                db.CameraMasters.InsertOnSubmit(cam);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Delete:CameraMaster Record by Id
        public bool Delete_CameraMaster(int id)
        {
            bool status = false;
            CameraMaster cam = db.CameraMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (cam != null)
            {
                cam.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Get:CameraMaster by Id
        public CameraMaster Get_CameraMaster_ById(int id)
        {
            CameraMaster cam = db.CameraMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return cam;
        }
    }
}