using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class MachineParametersRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        //Get:MachineId
        public IEnumerable<Model_MachineParameters> Get_MachinesId()
        {
            var data = (from t in db.tblMachineWorkingParameters
                        where t.IsDeleted == false
                        select new Model_MachineParameters
                        {
                            PlantCode = t.PlantCode,
                            MachineId = t.MachineId + " (" + t.PlantCode + " )"
                        }).ToList();
            return data;
        }

        //Get:MachineWorkingParameter DataTable
        public DataTable GetMachineDataTable()
        {
            DataTable dt = new DataTable();
            IEnumerable<tblMachineWorkingParameter> data = db.tblMachineWorkingParameters.Where(x => x.IsDeleted == false).ToList();
            dt.Columns.Add("Sr No");
            dt.Columns.Add("Plant Code");
            dt.Columns.Add("Machine Id");
            dt.Columns.Add("IP Port");
            dt.Columns.Add("Port No");
            dt.Columns.Add("Mode Of Coms");
            dt.Columns.Add("Stability Nos");
            dt.Columns.Add("Stability Range");
            dt.Columns.Add("Zero Interlock");
            dt.Columns.Add("Zero Interlock Range");
            dt.Columns.Add("Transaction No Prefix");
            dt.Columns.Add("Tare Check");
            dt.Columns.Add("Stored Tare");
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["Plant Code"] = item.PlantCode.ToString();
                dr["Machine Id"] = item.MachineId.ToString();
                dr["IP Port"] = item.IPPort.ToString();
                dr["Port No"] = item.PortNo.ToString();
                dr["Mode of Coms"] = item.ModeOfComs.ToString();
                dr["Stability Nos"] = item.StabilityNos.ToString();
                dr["Stability Range"] = item.StabilityRange.ToString();
                dr["Zero Interlock"] = item.ZeroInterlock.ToString();
                dr["Zero Interlock Range"] = item.ZeroInterlockRange.ToString();
                dr["Transaction No Prefix"] = item.TransactionNoPrefix.ToString();
                dr["Tare Check"] = item.TareCheck.ToString();
                dr["Stored Tare"] = item.StoredTare.ToString();
                dt.Rows.Add(dr);
                index++;

            }
            return dt;
        }

        //Get:Machine By Id
        internal tblMachineWorkingParameter Get_MachinebyId(int id)
        {
            tblMachineWorkingParameter obj = db.tblMachineWorkingParameters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return obj;
        }

        //Get:MachineWorkingParameter List
        internal IEnumerable<tblMachineWorkingParameter> GetMachineWorkingParameters_List()
        {
            IList<tblMachineWorkingParameter> data = db.tblMachineWorkingParameters.Where(x => x.IsDeleted == false).ToList();
            return data;
        }

        //Save:Excel file data to the server
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
                        string _PlantCode = dr["Plant Code"].ToString();
                        string _MachineId = dr["Machine Id"].ToString();
                        string _IPPort = dr["IP Port"].ToString();
                        string _PortNo = dr["Port No"].ToString();
                        string _ModeOfComs = dr["Mode Of Coms"].ToString();
                        string _StabilityNos = dr["Stability Nos"].ToString();
                        string _StabilityRange = dr["Stability Range"].ToString();
                        int _ZeroInterlock = Convert.ToInt32(dr["Zero Interlock"]);
                        string _ZeroInterlockRange = dr["Zero Interlock Range"].ToString();
                        string _TransactionNoPrefix = dr["Transaction No Prefix"].ToString();
                        int _TareCheck = Convert.ToInt32(dr["Tare Check"]);
                        int _StoredTare = Convert.ToInt32(dr["Stored Tare"]);
                        var data = db.tblMachineWorkingParameters.Where(x => x.PlantCode == _PlantCode && x.MachineId == _MachineId && x.IsDeleted == false).FirstOrDefault();
                        if (data != null)
                        {
                            if (string.IsNullOrEmpty(_IPPort) && string.IsNullOrEmpty(_PortNo) && string.IsNullOrEmpty(_ModeOfComs))
                            {
                                _failed++;
                            }
                            else
                            {
                                data.PlantCode = _PlantCode;
                                data.MachineId = _MachineId;
                                data.IPPort = _IPPort;
                                data.PortNo = _PortNo;
                                data.StabilityNos = _StabilityNos;
                                data.StabilityRange = _StabilityRange;
                                data.TransactionNoPrefix = _TransactionNoPrefix;
                                data.ZeroInterlock = _ZeroInterlock;
                                data.ZeroInterlockRange = _ZeroInterlockRange;
                                data.ModeOfComs = _ModeOfComs;
                                data.TareCheck = _TareCheck;
                                data.StoredTare = _StoredTare;
                                db.SubmitChanges();
                                _update++;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_PlantCode) && !string.IsNullOrEmpty(_MachineId) && !string.IsNullOrEmpty(_ModeOfComs) && !string.IsNullOrEmpty(_IPPort) && !string.IsNullOrEmpty(_PortNo))
                            {
                                PlantMaster _plantmaster = db.PlantMasters.Where(x => x.PlantCode == _PlantCode && x.IsDeleted == false).FirstOrDefault();
                                WeightMachineMaster _machinemaster = db.WeightMachineMasters.Where(x => x.MachineId == _MachineId && x.IsDeleted == false).FirstOrDefault();
                                if (_plantmaster != null && _machinemaster != null)
                                {
                                    tblMachineWorkingParameter _machineparameter = db.tblMachineWorkingParameters.Where(x => x.PlantCode == _PlantCode && x.MachineId == _MachineId && x.IsDeleted == false).FirstOrDefault();
                                    if (_machineparameter != null)
                                    {
                                        _failed++;
                                    }
                                    else
                                    {
                                        tblMachineWorkingParameter t = new tblMachineWorkingParameter();
                                        t.PlantCode = _PlantCode;
                                        t.MachineId = _MachineId;
                                        t.IPPort = _IPPort;
                                        t.PortNo = _PortNo;
                                        t.StabilityNos = _StabilityNos;
                                        t.StabilityRange = _StabilityRange;
                                        t.TransactionNoPrefix = _TransactionNoPrefix;
                                        t.ZeroInterlock = _ZeroInterlock;
                                        t.ZeroInterlockRange = _ZeroInterlockRange;
                                        t.TareCheck = _TareCheck;
                                        t.StoredTare = _StoredTare;
                                        t.ModeOfComs = _ModeOfComs;
                                        db.tblMachineWorkingParameters.InsertOnSubmit(t);
                                        db.SubmitChanges();
                                        _success++;
                                    }
                                }
                                else
                                {
                                    _failed++;
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

        //Add:New MachineWorkingParameter
        public bool Add_WorkingParameter(tblMachineWorkingParameter model)
        {
            bool status = false;
            if (model != null)
            {
                db.tblMachineWorkingParameters.InsertOnSubmit(model);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Delete:MachineWorkingParameter
        public bool Delete_WorkingParameter(int id)
        {
            bool status = false;
            tblMachineWorkingParameter machine = db.tblMachineWorkingParameters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (machine != null)
            {
                machine.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Get:MachineWorkingParameter By Id
        public tblMachineWorkingParameter Get_WorkingParameterById(int id)
        {
            tblMachineWorkingParameter machine = db.tblMachineWorkingParameters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return machine;
        }

        //Get:Plant Code By Id
        public string Get_PlantCode_By_Id(int id)
        {
            string code = db.tblMachineWorkingParameters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false).PlantCode;
            return code;
        }
    }
}