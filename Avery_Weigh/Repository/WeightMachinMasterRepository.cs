using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    
    public class WeightMachinMasterRepository
    {
        readonly DataClasses1DataContext db = new DataClasses1DataContext();
      
        //Get:Weight MachineId
        internal IEnumerable<Model_WeightMachinMaster> GetMachinId()
        {
            var data = (from t in db.WeightMachineMasters
                        select new Model_WeightMachinMaster
                        {
                            MachineId = t.MachineId
                        }).ToList();
            return data;
        }

        //Get:Weight Machine Master List Using Model
        public IEnumerable<Model_WeightMachinMaster> GetWeightMachinMastersList()
        {
            var data = (from t in db.WeightMachineMasters
                        where t.IsDeleted == false
                        select new Model_WeightMachinMaster
                        {
                            Id = t.Id,
                            PlantId = t.PlantCodeId,
                            MachineId = t.MachineId,
                            Capacity = t.Capacity,
                            Resolution = t.Resolution,
                            Model = t.Model,
                            PlatformSize = t.PlatformSize,
                            MachineNo = t.MachineNo,
                            Indicator = t.Indicator,
                            LCType = t.LCType,
                            NoOfLoadCells = t.NoOfLoadCells,
                            LoadCellSerialNos = t.LoadCellSerialNos,
                            EquipmentId = t.EquipmentId,
                            InvoiceNo = t.InvoiceNo,
                            DespatchDate = t.DespatchDate,
                            InstallationDate = t.InstallationDate,
                            WarrantyUpto = t.WarrentyUpto,
                            ReasonOfWarrantyUptoDate = t.ReasonWarrentyUptoDate.ToString()
                        }).ToList();
            return data;
        }

        //Get:Weightmachinemaster datatable
        public DataTable GetWeightMachinMasterDataTable()
        {
            var data = db.WeightMachineMasters.Where(x => x.IsDeleted == false).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr No");
            dt.Columns.Add("Plant Code");
            dt.Columns.Add("Machine Id");
            dt.Columns.Add("Capacity");
            dt.Columns.Add("Resolution");
            dt.Columns.Add("Model");
            dt.Columns.Add("Platform Size");
            dt.Columns.Add("Machin No");
            dt.Columns.Add("Indicator");
            dt.Columns.Add("LC Type");
            dt.Columns.Add("No Of Load Cells");
            dt.Columns.Add("Load Cell Serial Nos");
            dt.Columns.Add("Equipment");
            dt.Columns.Add("Invoice No");
            dt.Columns.Add("Despatch Date");
            dt.Columns.Add("Installation Date");
            dt.Columns.Add("Warranty Upto");
            dt.Columns.Add("Reason Of Warranty");
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Plant Code"] = item.PlantCodeId;
                dr["Machine Id"] = item.MachineId;
                dr["Capacity"] = item.Capacity;
                dr["Resolution"] = item.Resolution;
                dr["Model"] = item.Model;
                dr["Platform Size"] = item.PlatformSize;
                dr["Machin No"] = item.MachineNo;
                dr["Indicator"] = item.Indicator;
                dr["LC Type"] = item.LCType;
                dr["No Of Load Cells"] = item.NoOfLoadCells;
                dr["Load Cell Serial Nos"] = item.LoadCellSerialNos;
                dr["Equipment"] = item.EquipmentId;
                dr["Invoice No"] = item.InvoiceNo;
                dr["Despatch Date"] = item.DespatchDate;
                dr["Installation Date"] = item.DespatchDate;
                dr["Warranty Upto"] = item.WarrentyUpto;
                dr["Reason Of Warranty"] = item.ReasonWarrentyUptoDate;
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }
        
        //Save Excel Data To the Server
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
                        string _capacity = dr["Capacity"].ToString();
                        string _resolution = dr["Resolution"].ToString();
                        string _model = dr["Model"].ToString();
                        string _platformsize = dr["Platform Size"].ToString(); 
                        string _machinno = dr["Machin No"].ToString();
                        string _indicator = dr["Indicator"].ToString();
                        string _lctype = dr["LC Type"].ToString();
                        string _noofloadcells = dr["No Of Load Cells"].ToString();
                        string _loadcellserialnos = dr["Load Cell Serial Nos"].ToString();
                        string _Equipment = dr["Equipment"].ToString();
                        string _invoiceno = dr["Invoice No"].ToString();
                        DateTime? _despatchdate = null;
                        _despatchdate = Convert.ToDateTime(dr["Despatch Date"]);
                        DateTime? _installationdate = Convert.ToDateTime(dr["Installation Date"]);
                        DateTime? _warrantydate = Convert.ToDateTime(dr["Warranty Upto"]);
                        string _reasonofwarrantyuptodate = dr["Reason Of Warranty"].ToString();
                        var _wt = db.WeightMachineMasters.Where(x => x.MachineId == _MachineId && x.IsDeleted == false).FirstOrDefault();
                        var _PlantMaster = db.PlantMasters.Where(x => x.PlantCode == _PlantCode && x.IsDeleted == false).FirstOrDefault();
                        if (_wt != null)
                        {
                            if (_PlantMaster == null && string.IsNullOrEmpty(_MachineId) && string.IsNullOrEmpty(_capacity)
                                && string.IsNullOrEmpty(_resolution) && string.IsNullOrEmpty(_model) && string.IsNullOrEmpty(_platformsize) &&
                                string.IsNullOrEmpty(_machinno) && string.IsNullOrEmpty(_indicator) && string.IsNullOrEmpty(_lctype)
                                && string.IsNullOrEmpty(_noofloadcells) && string.IsNullOrEmpty(_loadcellserialnos) &&
                                string.IsNullOrEmpty(_Equipment) && string.IsNullOrEmpty(_invoiceno) && _despatchdate == null && _installationdate == null &&
                                _warrantydate == null && string.IsNullOrEmpty(_reasonofwarrantyuptodate))
                            {
                                _failed++;
                            }
                            else
                            {
                                _wt.PlantCodeId = _PlantCode;
                                _wt.Capacity = _capacity;
                                _wt.Resolution = _resolution;
                                _wt.Model = _model;
                                _wt.PlatformSize = _platformsize;
                                _wt.MachineNo = _machinno;
                                _wt.Indicator = _indicator;
                                _wt.LCType = _lctype;
                                _wt.NoOfLoadCells = _noofloadcells;
                                _wt.LoadCellSerialNos = _loadcellserialnos;
                                _wt.EquipmentId = Convert.ToInt32(_Equipment);
                                _wt.InvoiceNo = _invoiceno;
                                _wt.DespatchDate = _despatchdate;
                                _wt.InstallationDate = _installationdate;
                                _wt.WarrentyUpto = _warrantydate;
                                _wt.ReasonWarrentyUptoDate = _reasonofwarrantyuptodate;
                                db.SubmitChanges();
                                _update++;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_PlantCode) && _PlantMaster != null && !string.IsNullOrEmpty(_MachineId)
                                && !string.IsNullOrEmpty(_capacity) && !string.IsNullOrEmpty(_resolution) && !string.IsNullOrEmpty(_model) &&
                                !string.IsNullOrEmpty(_machinno) && !string.IsNullOrEmpty(_indicator) && !string.IsNullOrEmpty(_lctype)
                                && !string.IsNullOrEmpty(_noofloadcells) && !string.IsNullOrEmpty(_loadcellserialnos) &&
                                !string.IsNullOrEmpty(_Equipment) && !string.IsNullOrEmpty(_invoiceno) && _despatchdate != null
                                && _installationdate != null && _warrantydate != null && !string.IsNullOrEmpty(_platformsize) &&
                                 !string.IsNullOrEmpty(_reasonofwarrantyuptodate))
                            {
                                WeightMachineMaster _mm = db.WeightMachineMasters.Where(x => x.MachineId == _MachineId && x.IsDeleted == false).FirstOrDefault();
                                if (_mm != null)
                                {
                                    _failed++;
                                }
                                else
                                {
                                    db.WeightMachineMasters.InsertOnSubmit(new WeightMachineMaster
                                    {
                                        PlantCodeId = _PlantCode,
                                        MachineId = _MachineId,
                                        Capacity = _capacity,
                                        Resolution = _resolution,
                                        Model = _model,
                                        PlatformSize = _platformsize,
                                        MachineNo = _machinno,
                                        Indicator = _indicator,
                                        LCType = _lctype,
                                        NoOfLoadCells = _noofloadcells,
                                        LoadCellSerialNos = _loadcellserialnos,
                                        EquipmentId = Convert.ToInt16(_Equipment),
                                        InvoiceNo = _invoiceno,
                                        DespatchDate = _despatchdate,
                                        InstallationDate = _installationdate,
                                        WarrentyUpto = _warrantydate,
                                        ReasonWarrentyUptoDate = _reasonofwarrantyuptodate,
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

        //Get:Weight Machine List
        public IEnumerable<WeightMachineMaster> GetMachineMasters_List()
        {
            var data = db.WeightMachineMasters.Where(x => x.IsDeleted == false).ToList();
            return data;
        }

        //Get:Weight Machine Master By PlantCode
        public WeightMachineMaster GetMachineMasterByCode(string code)
        {
            var data = db.WeightMachineMasters.FirstOrDefault(x => x.PlantCodeId == code && x.IsDeleted == false);
            return data;
        }

        //Get:Weight Machine Master by MachineId
        public WeightMachineMaster GetMachineMaster_ByMachineId(string MachineId)
        {
            WeightMachineMaster machine = (from t in db.WeightMachineMasters
                                           where t.MachineId == MachineId && t.IsDeleted == false
                                           select t).FirstOrDefault();
            return machine;
        }

        //Add:New Weight Machine Master record
        public bool Add_WeightMachineMaster(WeightMachineMaster wt)
        {
            bool status = false;
            if (wt != null)
            {
                db.WeightMachineMasters.InsertOnSubmit(wt);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Delete:Weight Machine Master
        public bool Delete_WeightMachineMaster(int id)
        {
            bool status = false;
            WeightMachineMaster w = db.WeightMachineMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (w != null)
            {
                w.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Get:Weight Machine Master By Id
        public WeightMachineMaster Get_WeightMachineMasterById(int id)
        {
            WeightMachineMaster w = db.WeightMachineMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return w;
        }

        public WeightMachineMaster Get_WeightMachineMasterById_ss(string plantcode,string wbid)
        {
            WeightMachineMaster w1 = db.WeightMachineMasters.FirstOrDefault(x => x.PlantCodeId  == plantcode && x.MachineId== wbid && x.IsDeleted == false);
            return w1;
        }
        //Return string value
        public string Get_String(string value)
        {
            string str = string.Empty;
            foreach (char item in value)
            {
                if (char.IsLetter(item))
                {
                    str += item;
                }
            }
            return str;
        }

        //Retrun Number value
        public string Get_Number(string value)
        {
            string number = string.Empty;
            foreach (char item in value)
            {
                if (char.IsNumber(item))
                {
                    number += item;
                }
            }
            return number;
        }

        //Get:MachineId by Plant Code
        public IEnumerable<Model_WeightMachinMaster> Get_MachineIdBy_PlantCode(string code)
        {
            IEnumerable<Model_WeightMachinMaster> list = (from t in db.WeightMachineMasters
                                                          where t.PlantCodeId == code && t.IsDeleted == false
                                                          select new Model_WeightMachinMaster
                                                          {
                                                              MachineId = t.MachineId
                                                          }).ToList();
            return list;
        }
    }
}