using Avery_Weigh.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Avery_Weigh.Repository;

namespace Avery_Weigh.Repository
{
    public class AlphaDisplayRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        RegexRepository regex = new RegexRepository();
        
        //Get:AlphaDisplayMaster List Using Model
        public IEnumerable<Model_AlphaDisplayMaster> GetAllAlphaDisplayMasterList()
        {
            var data = (from t in db.AlphaDisplayMasters
                        where t.IsDeleted == false
                        select new Model_AlphaDisplayMaster
                        {
                            Id = t.Id,
                            PlantCode = t.PlantCodeId,
                            MachineId = t.MachineId,
                            AlphaDisplayIdentification = t.AlphaDisplayIdentification,
                            AlphaDisplayIP = t.AlphaDisplayIP,
                            AlphaDisplayPORT = t.AlphaDisplayPort
                        }).ToList();
            return data;
        }

        //Get:AlphaDisplayMaster DataTable
        public DataTable GetAlphaDisplayMasterDataTable()
        {
            var data = db.AlphaDisplayMasters.Where(x => x.IsDeleted == false).ToList();

            DataTable dt = new DataTable();
            DataColumn srno = new DataColumn("Sr No");
            DataColumn plantcode = new DataColumn("Plant Code Id");
            DataColumn machinid = new DataColumn("Machine Id");
            DataColumn bridentification = new DataColumn("Alpha Display Identification");
            DataColumn ip = new DataColumn("Alpha Display IP");
            DataColumn port = new DataColumn("Alpha Display PORT");
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
                dr["Plant Code Id"] = item.PlantCodeId;
                dr["Machine Id"] = item.MachineId;
                dr["Alpha Display Identification"] = item.AlphaDisplayIdentification;
                dr["Alpha Display IP"] = item.AlphaDisplayIP;
                dr["Alpha Display PORT"] = item.AlphaDisplayPort;
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
                int fail = 0;
                int success = 0;
                int update = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        string PlantCodeId = dr["Plant Code Id"].ToString();
                        string MachineId = dr["Machine Id"].ToString();
                        string AlphaDisplayIdentification = dr["Alpha Display Identification"].ToString();
                        string AlphaDisplayIp = dr["Alpha Display IP"].ToString();
                        string AlphaDisplayPort = dr["Alpha Display PORT"].ToString();
                        var plantmaster = db.PlantMasters.Where(x => x.PlantCode == PlantCodeId && x.IsDeleted == false).ToList();
                        var machinemaster = db.WeightMachineMasters.Where(x => x.MachineId == MachineId && x.IsDeleted == false).ToList();

                        int count = db.AlphaDisplayMasters.Count(x => x.PlantCodeId == PlantCodeId & x.MachineId == MachineId && x.IsDeleted == false);

                        if(plantmaster.Count > 0 && machinemaster.Count > 0)
                        {
                            if(AlphaDisplayIdentification.Length <= 10 && AlphaDisplayIp.Length <= 15 && AlphaDisplayPort.Length <= 5)
                            {
                                var data = db.AlphaDisplayMasters.FirstOrDefault(x => x.PlantCodeId == PlantCodeId && x.MachineId == MachineId && x.AlphaDisplayIP == AlphaDisplayIp && x.IsDeleted == false);
                                var IPExist = db.AlphaDisplayMasters.FirstOrDefault(x => x.PlantCodeId == PlantCodeId && x.AlphaDisplayIP == AlphaDisplayIp && x.Id!=data.Id && x.IsDeleted == false);
                                if (data != null)
                                {
                                    if (IPExist == null)
                                    {
                                        if (regex.CheckIPAddress(AlphaDisplayIp))
                                        {
                                            data.PlantCodeId = PlantCodeId;
                                            data.MachineId = MachineId;
                                            data.AlphaDisplayPort = AlphaDisplayPort;
                                            data.AlphaDisplayIdentification = AlphaDisplayIdentification;
                                            data.AlphaDisplayIP = AlphaDisplayIp;
                                            db.SubmitChanges();
                                            update++;
                                        }
                                        else { fail++; }
                                    }
                                    else
                                    {
                                        fail++;
                                    }
                                }
                                else
                                {
                                    if(count < 2)
                                    {
                                        var a = db.AlphaDisplayMasters.FirstOrDefault(x => x.PlantCodeId == PlantCodeId && x.AlphaDisplayIP == AlphaDisplayIp && x.IsDeleted == false);
                                        if (a != null)
                                        {
                                            if (regex.CheckIPAddress(AlphaDisplayIp))
                                            {
                                                AlphaDisplayMaster alpha = new AlphaDisplayMaster();
                                                alpha.PlantCodeId = PlantCodeId;
                                                alpha.MachineId = MachineId;
                                                alpha.AlphaDisplayIdentification = AlphaDisplayIdentification;
                                                alpha.AlphaDisplayIP = AlphaDisplayIp;
                                                alpha.AlphaDisplayPort = AlphaDisplayPort;
                                                db.AlphaDisplayMasters.InsertOnSubmit(alpha);
                                                db.SubmitChanges();
                                                success++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        fail++;
                                    }
                                }
                            }
                            else
                            {
                                fail++;
                            }
                        }
                        else
                        {
                            fail++;
                        }
                    }
                    catch
                    {
                        fail++;
                    }
                }
                result = "New Added: " + success + "  Updated:  " + update + "  Failed:  " + fail + "";
            }
            return result;
        }

        //Get:AlphaDisplayMaster List
        public IEnumerable<AlphaDisplayMaster> GetAlphaDisplayMasters_List()
        {
            var data = db.AlphaDisplayMasters.Where(x => x.IsDeleted == false).ToList();
            return data;
        }

        //Get:AlphaDisplayMaster By Code
        public AlphaDisplayMaster GetAlphaDisplayByCode(string code)
        {
            var data = db.AlphaDisplayMasters.FirstOrDefault(x => x.PlantCodeId == code && x.IsDeleted == false);
            return data;
        }

        //Add:New AlphaDisplayMaster Record
        public bool Add_AlphaDisplay(AlphaDisplayMaster alpha)
        {
            bool status = false;
            if (alpha != null)
            {
                db.AlphaDisplayMasters.InsertOnSubmit(alpha);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Delete:AlphaDisplayMaster by Id
        public bool Delete_AlphaDisplayById(int id)
        {
            bool status = false;
            AlphaDisplayMaster master = db.AlphaDisplayMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (master != null)
            {
                master.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Get:AlphaDisplayMaster by Id
        public AlphaDisplayMaster Get_AlphaDisplayById(int id)
        {
            AlphaDisplayMaster master = db.AlphaDisplayMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return master;
        }
    }
}