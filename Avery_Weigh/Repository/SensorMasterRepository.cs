using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Repository
{
    public class SensorMasterRepository
    {
        PlantmasterRepository plantrepo = new PlantmasterRepository();
        WeightMachinMasterRepository wmrepo = new WeightMachinMasterRepository();
        RegexRepository regex = new RegexRepository();
        DataClasses1DataContext db = new DataClasses1DataContext();

        //Add:New Sensor Master
        public bool Add_Sensor(tblSensorMaster master)
        {
            bool status = false;
            if (master != null)
            {
                db.tblSensorMasters.InsertOnSubmit(master);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Delete:Sensor Master Record by Id
        public bool Delete_Sensor(int id)
        {
            bool status = false;
            tblSensorMaster sensor = db.tblSensorMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (sensor != null)
            {
                sensor.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Get:Sensor Master Record by Id
        public tblSensorMaster Get_Sensor_by_Id(int id)
        {
            tblSensorMaster sensor = db.tblSensorMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return sensor;
        }

        //Get:Sensor Master data list
        public IEnumerable<tblSensorMaster> Get_Sensor_List()
        {
            IEnumerable<tblSensorMaster> list = db.tblSensorMasters.Where(x => x.IsDeleted == false);
            return list;
        }

        //Get:Sensor Master Data in Datatable
        public DataTable Get_Sensor_DataTable()
        {
            IEnumerable<tblSensorMaster> data = Get_Sensor_List();
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr No");
            dt.Columns.Add("Plant Code");
            dt.Columns.Add("Machine Id");
            dt.Columns.Add("Sensor Identification");
            dt.Columns.Add("Sensor IP");
            dt.Columns.Add("Sensor Port");
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["Plant Code"] = item.PlantCode.ToString();
                dr["Machine Id"] = item.MachineId.ToString();
                dr["Sensor Identification"] = item.SensorIdentification.ToString();
                dr["Sensor IP"] = item.SensorIP.ToString();
                dr["Sensor Port"] = item.SensorPort.ToString();
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        //Save:Excel File Data To the Server
        public string SaveDataToServer(DataSet ds)
        {
            string result = string.Empty;
            int success = 0;
            int fail = 0;
            int update = 0;
            string PlantCode = string.Empty;
            string MachineId = string.Empty;
            string SensorIdentification = string.Empty;
            string SensorIP = string.Empty;
            string SensorPort = string.Empty;
            DataTable dt = ds.Tables[0];
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        PlantCode = dr["Plant Code"].ToString().Trim();
                        MachineId = dr["Machine Id"].ToString().Trim();
                        SensorIdentification = dr["Sensor Identification"].ToString().Trim();
                        SensorIP = dr["Sensor IP"].ToString().Trim();
                        SensorPort = dr["Sensor Port"].ToString().Trim();                      
                        if (PlantCode.Length <= 10 && MachineId.Length <= 10 && SensorIdentification.Length <= 10 && SensorIP.Length <= 15 && SensorPort.Length <= 5)
                        {
                            if (regex.CheckIPAddress(SensorIP))
                            {
                                var PlantMaster = plantrepo.Get_PlantMaster_By_PlantCode(PlantCode);
                                var WeightMachine = wmrepo.GetMachineMasters_List().Where(x => x.PlantCodeId == PlantCode && x.IsDeleted == false).ToList();
                                if (PlantMaster != null && WeightMachine.Count > 0)
                                {
                                    var sens = Get_Sensor_List().FirstOrDefault(x => x.PlantCode == PlantCode && x.MachineId == MachineId && x.IsDeleted == false);
                                    var data = Get_Sensor_List().FirstOrDefault(x => x.PlantCode == PlantCode && x.SensorIP == SensorIP && x.Id!=sens.Id && x.IsDeleted == false);
                                    if (sens!=null)
                                    {
                                        tblSensorMaster sensor = db.tblSensorMasters.FirstOrDefault(x => x.Id == sens.Id && x.IsDeleted == false);
                                        if (sensor != null)
                                        {
                                            if (data!= null)
                                            {
                                                fail++;
                                            }
                                            else
                                            {
                                                sensor.PlantCode = PlantCode;
                                                sensor.MachineId = MachineId;
                                                sensor.SensorIdentification = SensorIdentification;
                                                sensor.SensorIP = SensorIP;
                                                sensor.SensorPort = SensorPort;
                                                db.SubmitChanges();
                                                update++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var data2 = Get_Sensor_List().FirstOrDefault(x => x.PlantCode == PlantCode && x.SensorIP == SensorIP && x.IsDeleted == false);
                                        tblSensorMaster sensor = new tblSensorMaster();
                                        if (data2!=null)
                                        {
                                            fail++;
                                        }
                                        else
                                        {
                                            sensor.PlantCode = PlantCode;
                                            sensor.MachineId = MachineId;
                                            sensor.SensorIdentification = SensorIdentification;
                                            sensor.SensorIP = SensorIP;
                                            sensor.SensorPort = SensorPort;
                                            sensor.IsDeleted = false;
                                            db.tblSensorMasters.InsertOnSubmit(sensor);
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
                    catch(Exception ex)
                    {
                        fail++;
                    }
                }
                result = "New Added:-" + success + " Updated:-" + update + " Failed:-" + fail + "";
            }
            return result;
        }

        //Get:Sensor By PlantCode and Sensor IP
        public tblSensorMaster Get_SensorBy_PlantCode_N_IP(string plantcode, string IP)
        {
            tblSensorMaster sensor = db.tblSensorMasters.FirstOrDefault(x => x.PlantCode == plantcode && x.SensorIP == IP && x.IsDeleted == false);
            return sensor;
        }
    }
}