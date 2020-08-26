using Avery_Weigh.Model;
using Avery_Weigh.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace Avery_Weigh.AveryService
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        DataClasses1DataContext db = new DataClasses1DataContext();

        #region Repositories Reference
        MaterialRepository _materialrepo = new MaterialRepository();
        SupplierRepository _supplierrepo = new SupplierRepository();
        TransporterRepository _transrepo = new TransporterRepository();
        PackingRepository _Packingrepo = new PackingRepository();
        TransactionRepository _transactionRepo = new TransactionRepository();
        TruckMasterRepository _truckRepo = new TruckMasterRepository();
        VehicleClassificationRepository _VCRepo = new VehicleClassificationRepository();
        GateEntryRepository gateRepo = new GateEntryRepository();
        AlphaDisplayRepository _alpharepo = new AlphaDisplayRepository();
        BarrierMasterRepository _barrierrepo = new BarrierMasterRepository();
        CameraMasterRepository _camrepo = new CameraMasterRepository();
        WeightMachinMasterRepository _wrepo = new WeightMachinMasterRepository();
        MaterialClassificationRepository _matrepo = new MaterialClassificationRepository();
        PlantmasterRepository _plantrepo = new PlantmasterRepository();
        UserClassificationRepository _ucrepo = new UserClassificationRepository();
        MachineParametersRepository _machinerepo = new MachineParametersRepository();
        UserMasterRepository umrepo = new UserMasterRepository();
        SensorMasterRepository sensorrepo = new SensorMasterRepository();
        ServiceMasterRepository servicerepo = new ServiceMasterRepository();

        #endregion

        #region search
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetTransactionDetail(string truckNo)
        {
            Model_ManualWeight model = new Model_ManualWeight();
            if (_transactionRepo.checkTruckIsPendingOrNot(truckNo))
            {
                model = _transactionRepo.gettransactionDetail(truckNo);
                if (model != null)
                {
                    //if (model.transWeight != null)
                    //{
                    //    List<Mode_transactionWeight> _weight = _transactionRepo.GetTruckWeight(truckNo);
                    //}
                }
            }
            model.truckMaster = _truckRepo.GetTruckMasterByTruckNo(truckNo);
            model.VC = _VCRepo.GetTruckVehicleClassification(truckNo);
            model.transmaterials = _transactionRepo.gettransactionmaterials(truckNo); //.Skip(1);
            string myJsonString = (new JavaScriptSerializer()).Serialize(model);
            return myJsonString;
        }

        //[WebMethod]
        //public string GetPendingVehicle(string truckNo)
        //{
        //    Model_ManualWeight model = new Model_ManualWeight();
        //    if (_transactionRepo.checkTruckIsPendingOrNot(truckNo))
        //    {
        //        model = _transactionRepo.gettransactionDetail(truckNo);
        //        if (model != null)
        //        {
        //            //if (model.transWeight != null)
        //            //{
        //            //    List<Mode_transactionWeight> _weight = _transactionRepo.GetTruckWeight(truckNo);
        //            //}
        //        }
        //    }
        //    model.truckMaster = _truckRepo.GetTruckMasterByTruckNo(truckNo);
        //    model.VC = _VCRepo.GetTruckVehicleClassification(truckNo);
        //    model.transmaterials = _transactionRepo.gettransactionmaterials(truckNo).Skip(1);
        //    string myJsonString = (new JavaScriptSerializer()).Serialize(model);
        //    return myJsonString;
        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetPendingVehicle(string prefix)
        {
            List<string> customers = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager
                        .ConnectionStrings["AveryDBConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select TRUCKNO from tblTransactions where TransactionStatus=1 AND " +
                    "TRUCKNO like @SearchText + '%'";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            customers.Add(string.Format("{0}-{1}", sdr["TRUCKNO"], sdr["TRUCKNO"]));
                        }
                    }
                    conn.Close();
                }
                return customers.ToArray();
            }
        }

        [WebMethod]
        public string CheckStoredtareWt()
        {
            tblMachineWorkingParameter _tstoreenabled = db.tblMachineWorkingParameters.Where(x=>x.Id==1).FirstOrDefault();
            //tblMachineWorkingParameter _tstoreenabled = db.tblMachineWorkingParameters.Where(x => x.PlantCode == Session["PlantID"].ToString() && x.MachineId == Session["WBID"].ToString()).FirstOrDefault();


            string myJsonString = (new JavaScriptSerializer()).Serialize(_tstoreenabled.StoredTare.ToString());
            return myJsonString;
        }

        [WebMethod]
        public string ToleranceTypeValue(string ttype)
        {
            AverageTareSchema _toltype = db.AverageTareSchemas.Where(x => x.Description == ttype).FirstOrDefault();
            //.Where(x => x.PlantCode == Session["PlantID"].ToString() && x.MachineId == Session["WBID"].ToString()).FirstOrDefault();


            string myJsonString = (new JavaScriptSerializer()).Serialize(_toltype.weightvalue.ToString());
            return myJsonString;
        }

        [WebMethod]
        public string CreatePendingRecord(string TripId, string truckNo)
        {
            TruckMaster _tmaster = _truckRepo.GetTruckMasterByTruckNo(truckNo);
           
            tblTransaction _trans = new tblTransaction();
            tblTransaction trans = db.tblTransactions.Where(x => x.TruckNo == truckNo && x.TransactionStatus == 1).FirstOrDefault();
            if (trans==null)
            {
                _trans.TripId = Convert.ToInt32(TripId);
                _trans.TruckNo = truckNo;
                _trans.TransporterCode = _tmaster.TransporterCode;
                _trans.TransporterName = _tmaster.TransporterName;
                _trans.WeighingUnit = _tmaster.UOMWeight;
                //_trans.CompanyCode = Session["CompanyCode"].ToString();
                //_trans.PlantCode = Session["PlantID"].ToString();
                _trans.TripType = 2;
                _trans.CreateDate = DateTime.Now;
                _trans.TransactionStatus = 1;
                _trans.IsMultiProduct = false;
                _trans.FirstWeight = Convert.ToDecimal(_tmaster.StoredTareWeight);
                _trans.FirstWtDateTime = _tmaster.StoredTareDateTime;
                _trans.UseStoredTareWt = "Y";
                db.tblTransactions.InsertOnSubmit(_trans);
                db.SubmitChanges();
            }
            string myJsonString = (new JavaScriptSerializer()).Serialize(_trans);
            return myJsonString;
        }


        [WebMethod]
        public string GetTransactionDetailbygateEntry(string gateNo)
        {
            string truckNo = gateRepo.gettuckNofromgateEntry(gateNo);
            Model_ManualWeight model = new Model_ManualWeight();
            if (_transactionRepo.checkTruckIsPendingOrNot(truckNo))
            {
                model = _transactionRepo.gettransactionDetail(truckNo);
                if (model != null)
                {
                    //if (model.transWeight != null)
                    //{
                    //    List<Mode_transactionWeight> _weight = _transactionRepo.GetTruckWeight(truckNo);
                    //}
                }
            }
            model.truckMaster = _truckRepo.GetTruckMasterByTruckNo(truckNo);
            model.VC = _VCRepo.GetTruckVehicleClassification(truckNo);
            model.transmaterials = _transactionRepo.gettransactionmaterials(truckNo);
            model.TruckNo = truckNo;
            string myJsonString = (new JavaScriptSerializer()).Serialize(model);
            return myJsonString;
        }

        [WebMethod]
        public string GetSupplier(string search1, string search2, string drop1, string drop2)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            IEnumerable<tblSupplier> tblSuppliers = _supplierrepo.Get_SuppliersList().ToList();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "Code")
                    tblSuppliers = _supplierrepo.Get_SuppliersList().Where(x => x.Code.ToLower().Contains(search1)).ToList();
                else
                    tblSuppliers = _supplierrepo.Get_SuppliersList().Where(x => x.Name.ToLower().Contains(search1)).ToList();

            if (!string.IsNullOrEmpty(search2))
                if (drop1 == "Name")
                    tblSuppliers = _supplierrepo.Get_SuppliersList().Where(x => x.Name.ToLower().Contains(search2)).ToList();
                else
                    tblSuppliers = _supplierrepo.Get_SuppliersList().Where(x => x.Code.ToLower().Contains(search2)).ToList();
            string myJsonString = (new JavaScriptSerializer()).Serialize(tblSuppliers);
            return myJsonString;
        }

        [WebMethod]
        public string GetSupplierbyCode(string code)
        {
            tblSupplier supplier = _supplierrepo.Get_SupplierbyCode(code.ToLower());
            string myJsonString = (new JavaScriptSerializer()).Serialize(supplier);
            return myJsonString;
        }

        [WebMethod]
        public string GetTransporter(string search1, string search2, string drop1, string drop2)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            IEnumerable<tblTransporter> tblTransporters = _transrepo.GetTransportersList();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "Code")
                    tblTransporters = _transrepo.GetTransportersList().Where(x => x.Code.ToLower().Contains(search1)).ToList();
                else
                    tblTransporters = _transrepo.GetTransportersList().Where(x => x.Name.ToLower().Contains(search1)).ToList();
            if (!string.IsNullOrEmpty(drop2))
            {
                if (drop2 == "Name")
                    tblTransporters = _transrepo.GetTransportersList().Where(x => x.Name.ToLower().Contains(search2)).ToList();
                else
                    tblTransporters = _transrepo.GetTransportersList().Where(x => x.Code.ToLower().Contains(search2)).ToList();
            }
            string myjsonresult = (new JavaScriptSerializer()).Serialize(tblTransporters);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_TransporterByCode(string code)
        {
            tblTransporter transporter = _transrepo.GetTransporter_ByCode(code.ToLower());
            string myjsonresult = (new JavaScriptSerializer()).Serialize(transporter);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_Packings(string search1, string search2, string drop1, string drop2)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            IEnumerable<PackingMaster> packingMasters = _Packingrepo.GetPackingMasters_List();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "Code")
                    packingMasters = _Packingrepo.GetPackingMasters_List().Where(x => x.PackingCode.ToLower().Contains(search1)).ToList();
                else
                    packingMasters = _Packingrepo.GetPackingMasters_List().Where(x => x.PackingName.ToLower().Contains(search1)).ToList();
            if (!string.IsNullOrEmpty(search2))
                if (drop2 == "Name")
                    packingMasters = _Packingrepo.GetPackingMasters_List().Where(x => x.PackingCode.ToLower().Contains(search2)).ToList();
                else
                    packingMasters = _Packingrepo.GetPackingMasters_List().Where(x => x.PackingName.ToLower().Contains(search2)).ToList();
            string myjsonresult = (new JavaScriptSerializer()).Serialize(packingMasters);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_PackingByCode(string code)
        {
            PackingMaster packingMaster = _Packingrepo.Get_PackingByCode(code.ToLower());
            string myjsonresult = (new JavaScriptSerializer()).Serialize(packingMaster);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_AlphaDisplay(string search1, string search2,string search3, string drop1, string drop2,string drop3)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            search3 = search3.ToLower();
            IEnumerable<AlphaDisplayMaster> alphaDisplays = _alpharepo.GetAlphaDisplayMasters_List();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "Code")
                    alphaDisplays = _alpharepo.GetAlphaDisplayMasters_List().Where(x => x.PlantCodeId.ToLower().Contains(search1)).ToList();
                else if (drop1 == "Id")
                    alphaDisplays = _alpharepo.GetAlphaDisplayMasters_List().Where(x => x.MachineId.ToLower().Contains(search1)).ToList();
                else
                    alphaDisplays = _alpharepo.GetAlphaDisplayMasters_List().Where(x => x.AlphaDisplayIP.Contains(search1)).ToList();
            if (!string.IsNullOrEmpty(search2))
                if (drop2 == "Id")
                    alphaDisplays = _alpharepo.GetAlphaDisplayMasters_List().Where(x => x.MachineId.ToLower().Contains(search2)).ToList();
                else if (drop2 == "Code")
                    alphaDisplays = _alpharepo.GetAlphaDisplayMasters_List().Where(x => x.PlantCodeId.ToLower().Contains(search2)).ToList();
                else
                    alphaDisplays = _alpharepo.GetAlphaDisplayMasters_List().Where(x => x.AlphaDisplayIP.Contains(search2)).ToList();
            if (!string.IsNullOrEmpty(search3))
                if (drop3 == "IP")
                    alphaDisplays = _alpharepo.GetAlphaDisplayMasters_List().Where(x => x.AlphaDisplayIP.Contains(search3)).ToList();
                else if (drop3 == "Id")
                    alphaDisplays = _alpharepo.GetAlphaDisplayMasters_List().Where(x => x.MachineId.ToLower().Contains(search3)).ToList();
                else
                    alphaDisplays = _alpharepo.GetAlphaDisplayMasters_List().Where(x => x.PlantCodeId.ToLower().Contains(search3)).ToList();
            string myjsonresult = (new JavaScriptSerializer()).Serialize(alphaDisplays);
            return myjsonresult;
        }

        [WebMethod]
        public string GetAlphaByCode(string code)
        {
            AlphaDisplayMaster alphaDisplay = _alpharepo.GetAlphaDisplayByCode(code.ToLower());
            string myjsonresult = (new JavaScriptSerializer()).Serialize(alphaDisplay);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_Barriers(string search1, string search2,string search3, string drop1, string drop2,string drop3)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            search3 = search3.ToLower();
            IEnumerable<BarrierMaster> barrierMasters = _barrierrepo.GetBarrierMasters_List();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "Code")
                    barrierMasters = _barrierrepo.GetBarrierMasters_List().Where(x => x.PlantCodeId.ToLower().Contains(search1)).ToList();
                else if (drop1 == "Name")
                    barrierMasters = _barrierrepo.GetBarrierMasters_List().Where(x => x.MachineId.ToLower().Contains(search1)).ToList();
                else
                    barrierMasters = _barrierrepo.GetBarrierMasters_List().Where(x => x.BarrierIP.Contains(search1)).ToList();
            if (!string.IsNullOrEmpty(search2))
                if (drop2 == "Name")
                    barrierMasters = _barrierrepo.GetBarrierMasters_List().Where(x => x.MachineId.ToLower().Contains(search2)).ToList();
                else if (drop2 == "Code")
                    barrierMasters = _barrierrepo.GetBarrierMasters_List().Where(x => x.PlantCodeId.ToLower().Contains(search2)).ToList();
                else
                    barrierMasters = _barrierrepo.GetBarrierMasters_List().Where(x => x.BarrierIP.Contains(search2)).ToList();
            if(!string.IsNullOrEmpty(search3))
                if (drop1 == "IP")
                    barrierMasters = _barrierrepo.GetBarrierMasters_List().Where(x => x.BarrierIP.Contains(search1)).ToList();
                else if (drop1 == "Code")
                    barrierMasters = _barrierrepo.GetBarrierMasters_List().Where(x => x.PlantCodeId.ToLower().Contains(search1)).ToList();
                else
                    barrierMasters = _barrierrepo.GetBarrierMasters_List().Where(x => x.MachineId.ToLower().Contains(search1)).ToList();
            string myjsonresult = (new JavaScriptSerializer()).Serialize(barrierMasters);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_BarrierByCode(string code)
        {
            BarrierMaster _bb = _barrierrepo.GetBarrierByCode(code.ToLower());
            string myjsonresult = (new JavaScriptSerializer()).Serialize(_bb);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_CameraMaster(string search1, string search2,string search3, string drop1, string drop2,string drop3)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            search3 = search3.ToLower();
            IEnumerable<CameraMaster> cameraMasters = _camrepo.GetCameraMasters_List();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "Code")
                    cameraMasters = _camrepo.GetCameraMasters_List().Where(x => x.PlantCodeID.ToLower().Contains(search1)).ToList();
                else if (drop1 == "Name")
                    cameraMasters = _camrepo.GetCameraMasters_List().Where(x => x.MachineId.ToLower().Contains(search1)).ToList();
                else
                    cameraMasters = _camrepo.GetCameraMasters_List().Where(x => x.CameraIP.ToLower().Contains(search1)).ToList();
            if (!string.IsNullOrEmpty(search2))
                if (drop2 == "Name")
                    cameraMasters = _camrepo.GetCameraMasters_List().Where(x => x.MachineId.ToLower().Contains(search2)).ToList();
                else if (drop2 == "Code")
                    cameraMasters = _camrepo.GetCameraMasters_List().Where(x => x.PlantCodeID.ToLower().Contains(search2)).ToList();
                else
                    cameraMasters = _camrepo.GetCameraMasters_List().Where(x => x.CameraIP.ToLower().Contains(search2)).ToList();
            if (!string.IsNullOrEmpty(search3))
                if (drop3 == "CameraIP")
                    cameraMasters = _camrepo.GetCameraMasters_List().Where(x => x.CameraIP.ToLower().Contains(search3)).ToList();
                else if (drop3 == "Code")
                    cameraMasters = _camrepo.GetCameraMasters_List().Where(x => x.PlantCodeID.ToLower().Contains(search3)).ToList();
                else
                    cameraMasters = _camrepo.GetCameraMasters_List().Where(x => x.MachineId.ToLower().Contains(search3)).ToList();
            string myjsonresult = (new JavaScriptSerializer()).Serialize(cameraMasters);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_CameraById(string code)
        {
            CameraMaster cam = _camrepo.GetCameraMaster_ByCode(code.ToLower());
            string myjsonresult = new JavaScriptSerializer().Serialize(cam);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_WeightMachineMaster(string search1, string search2, string drop1, string drop2)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            IEnumerable<WeightMachineMaster> machineMasters = _wrepo.GetMachineMasters_List();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "Code")
                    machineMasters = _wrepo.GetMachineMasters_List().Where(x => x.PlantCodeId.ToLower().Contains(search1)).ToList();
                else
                    machineMasters = _wrepo.GetMachineMasters_List().Where(x => x.MachineId.ToLower().Contains(search1)).ToList();
            if (!string.IsNullOrEmpty(search2))
                if (drop2 == "Name")
                    machineMasters = _wrepo.GetMachineMasters_List().Where(x => x.MachineId.ToLower().Contains(search2)).ToList();
                else
                    machineMasters = _wrepo.GetMachineMasters_List().Where(x => x.PlantCodeId.ToLower().Contains(search2)).ToList();
            string myjsonresult = new JavaScriptSerializer().Serialize(machineMasters);
            return myjsonresult;
        }

        [WebMethod]
        public string GetweightmachinByCode(string code)
        {
            WeightMachineMaster _wm = _wrepo.GetMachineMasterByCode(code.ToLower());
            string myjsonresult = new JavaScriptSerializer().Serialize(_wm);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_Materials(string search1, string search2, string drop1, string drop2)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            IEnumerable<Model_Materials> _Materials = _materialrepo.Get_Model_MaterialList();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "Code")
                    _Materials = _materialrepo.Get_Model_MaterialList().Where(x => x.MaterialCode.ToLower().Contains(search1)).ToList();
                else
                    _Materials = _materialrepo.Get_Model_MaterialList().Where(x => x.MaterialDesc.ToLower().Contains(search1)).ToList();
            if (!string.IsNullOrEmpty(search2))
                if (drop2 == "Name")
                    _Materials = _materialrepo.Get_Model_MaterialList().Where(x => x.MaterialDesc.ToLower().Contains(search2)).ToList();
                else
                    _Materials = _materialrepo.Get_Model_MaterialList().Where(x => x.MaterialCode.ToLower().Contains(search2)).ToList();
            string myjsonresult = new JavaScriptSerializer().Serialize(_Materials);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_MaterialByCode(string code)
        {
            Model_Materials _mat = _materialrepo.GetmaterialByCode(code.ToLower());
            string myjsonresult = new JavaScriptSerializer().Serialize(_mat);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_MaterialClassifications(string search1, string search2, string drop1, string drop2)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            IEnumerable<Model_MaterialClassification> _MaterialClassifications = _matrepo.GetModel_MaterialClassificationsList();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "Code")
                    _MaterialClassifications = _matrepo.GetModel_MaterialClassificationsList().Where(x => x.MaterialClassificationCode.ToLower().Contains(search1)).ToList();
                else
                    _MaterialClassifications = _matrepo.GetModel_MaterialClassificationsList().Where(x => x.MaterialClassificationDesc.ToLower().Contains(search1)).ToList();
            if (!string.IsNullOrEmpty(search2))
                if (drop2 == "Name")
                    _MaterialClassifications = _matrepo.GetModel_MaterialClassificationsList().Where(x => x.MaterialClassificationDesc.ToLower().Contains(search2)).ToList();
                else
                    _MaterialClassifications = _matrepo.GetModel_MaterialClassificationsList().Where(x => x.MaterialClassificationCode.ToLower().Contains(search2)).ToList();
            string myjsonresult = new JavaScriptSerializer().Serialize(_MaterialClassifications);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_MaterialClassificationByCode(string code)
        {
            Model_MaterialClassification classification = _matrepo.Get_MaterialClassificationByCode(code.ToLower());
            string myjsonresult = new JavaScriptSerializer().Serialize(classification);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_VehicleClassification(string search1, string search2, string drop1, string drop2)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            IEnumerable<Model_VehicleClassification> ModelList = _VCRepo.Get_Model_VehicleClassificationsList();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "Code")
                    ModelList = _VCRepo.Get_Model_VehicleClassificationsList().Where(x => x.ClassificationCode.ToLower().Contains(search1)).ToList();
                else
                    ModelList = _VCRepo.Get_Model_VehicleClassificationsList().Where(x => x.Make.ToLower().Contains(search1)).ToList();
            if (!string.IsNullOrEmpty(search2))
                if (drop2 == "Name")
                    ModelList = _VCRepo.Get_Model_VehicleClassificationsList().Where(x => x.Make.ToLower().Contains(search2)).ToList();
                else
                    ModelList = _VCRepo.Get_Model_VehicleClassificationsList().Where(x => x.ClassificationCode.ToLower().Contains(search2)).ToList();
            string myjsonstring = new JavaScriptSerializer().Serialize(ModelList);
            return myjsonstring;
        }

        [WebMethod]
        public string Get_VehicleclassificationByCode(string code)
        {
            Model_VehicleClassification _model = _VCRepo.Get_Model_VehicleClassificationByCode(code.ToLower());
            string myjsonresult = new JavaScriptSerializer().Serialize(_model);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_PlantMaster(string search1, string search2, string drop1, string drop2)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            IEnumerable<PlantMaster> list = _plantrepo.Get_PlantList();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "Code")
                    list = _plantrepo.Get_PlantList().Where(x => x.PlantCode.ToLower().Contains(search1)).ToList();
                else
                    list = _plantrepo.Get_PlantList().Where(x => x.PlantName.ToLower().Contains(search1)).ToList();
            if (!string.IsNullOrEmpty(search2))
                if (drop2 == "Name")
                    list = _plantrepo.Get_PlantList().Where(x => x.PlantName.ToLower().Contains(search2)).ToList();
                else
                    list = _plantrepo.Get_PlantList().Where(x => x.PlantCode.ToLower().Contains(search2)).ToList();
            string myjsonstring = new JavaScriptSerializer().Serialize(list);
            return myjsonstring;
        }

        [WebMethod]
        public string Get_PlantMasterByCode(string code)
        {
            PlantMaster _pm = _plantrepo.Get_PlantMaster_By_PlantCode(code.ToLower());
            string myjsonresult = new JavaScriptSerializer().Serialize(_pm);
            return myjsonresult;
        }

        [WebMethod]
        public string Get_UserClassification(string search1, string drop1)
        {
            search1 = search1.ToLower();
            IEnumerable<Model_UserClassification> list = _ucrepo.GetUserClassifications_List();
            if (!string.IsNullOrEmpty(search1))
            {
                if (drop1 == "UserType")
                {
                    list = _ucrepo.GetUserClassifications_List().Where(x => x.UserType.ToLower().Contains(search1)).ToList();
                }
            }
            string myjsonsstring = new JavaScriptSerializer().Serialize(list);
            return myjsonsstring;
        }

        [WebMethod]
        public string Get_UserClassificationByUserType(string type)
        {
            type = type.ToLower();
            Model_UserClassification uc = _ucrepo.GetUserClassificationByUserType(type.ToLower());
            string myjsonstring = new JavaScriptSerializer().Serialize(uc);
            return myjsonstring;
        }

        [WebMethod]
        public string Get_TruckMaster(string search1, string search2, string drop1, string drop2)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            IEnumerable<TruckMaster> list = _truckRepo.GetTruckMasters_List();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "TruckNo")
                    list = _truckRepo.GetTruckMasters_List().Where(x => x.TruckRegNo.ToLower().Contains(search1)).ToList();
                else
                    list = _truckRepo.GetTruckMasters_List().Where(x => x.ClassificationCode.ToLower().Contains(search1)).ToList();
            if (!string.IsNullOrEmpty(search2))
                if (drop2 == "code")
                    list = _truckRepo.GetTruckMasters_List().Where(x => x.ClassificationCode.ToLower().Contains(search2)).ToList();
                else
                    list = _truckRepo.GetTruckMasters_List().Where(x => x.TruckRegNo.ToLower().Contains(search2)).ToList();
            string myjsonstring = new JavaScriptSerializer().Serialize(list);
            return myjsonstring;
        }

        [WebMethod]
        public string GetTruckMasterByTruckNo(string truckno)
        {
            TruckMaster truckMaster = _truckRepo.GetTruckMasterByTruckNo(truckno.ToLower());
            string mystring = new JavaScriptSerializer().Serialize(truckMaster);
            return mystring;
        }

        [WebMethod]
        public string Get_MachinesWorkingParameter(string search1, string search2,string search3, string drop1, string drop2,string drop3)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            search3 = search3.ToLower();
            IEnumerable<tblMachineWorkingParameter> list = _machinerepo.GetMachineWorkingParameters_List();
            if (!string.IsNullOrEmpty(search1))
            {
                if (drop1 == "Code")
                {
                    list = _machinerepo.GetMachineWorkingParameters_List().Where(x => x.PlantCode.ToLower().Contains(search1)).ToList();
                }
                else if (drop1 == "MachineId")
                {
                    list = _machinerepo.GetMachineWorkingParameters_List().Where(x => x.MachineId.ToLower().Contains(search1)).ToList();
                }
                else
                {
                    list = _machinerepo.GetMachineWorkingParameters_List().Where(x => x.IPPort.Contains(search1)).ToList();
                }
            }
            if (!string.IsNullOrEmpty(search2))
            {
                if(drop2 == "MachineId")
                {
                    list = _machinerepo.GetMachineWorkingParameters_List().Where(x => x.MachineId.ToLower().Contains(search2)).ToList();
                }
                else if(drop2 == "Code")
                {
                    list = _machinerepo.GetMachineWorkingParameters_List().Where(x => x.PlantCode.ToLower().Contains(search2)).ToList();
                }
                else
                {
                    list = _machinerepo.GetMachineWorkingParameters_List().Where(x => x.IPPort.Contains(search2)).ToList();
                }
            }
            if (!string.IsNullOrEmpty(search3))
            {
                if(drop3 == "IPPort")
                {
                    list = _machinerepo.GetMachineWorkingParameters_List().Where(x => x.IPPort.Contains(search3)).ToList();
                }
                else if(drop3 == "MachineId")
                {
                    list = _machinerepo.GetMachineWorkingParameters_List().Where(x => x.MachineId.ToLower().Contains(search3)).ToList();
                }
                else
                {
                    list = _machinerepo.GetMachineWorkingParameters_List().Where(x => x.PlantCode.ToLower().Contains(search3)).ToList();
                }
            }
            string jsonstring = new JavaScriptSerializer().Serialize(list);
            return jsonstring;
        }

        [WebMethod]
        public string Get_Machines_ByMachineId(int id)
        {
            tblMachineWorkingParameter _mat = _machinerepo.Get_MachinebyId(id);
            string jsonstrin = new JavaScriptSerializer().Serialize(_mat);
            return jsonstrin;
        }

        [WebMethod]
        public string Get_UserMaster(string search1,string search2,string drop1,string drop2)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            IEnumerable<Model_UserMasters> list = umrepo.Get_Users();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "UserName")
                    list = umrepo.Get_Users().Where(x => x._UserMaster.UserName.ToLower().Contains(search1)).ToList();
                else
                    list = umrepo.Get_Users().Where(x => x._UserClassification.UserType.ToLower().Contains(search1)).ToList();
            if (!string.IsNullOrEmpty(search2))
                if (drop2 == "UserType")
                    list = umrepo.Get_Users().Where(x => x._UserClassification.UserType.ToLower().Contains(search2)).ToList();
                else
                    list = umrepo.Get_Users().Where(x => x._UserMaster.UserName.ToLower().Contains(search2)).ToList();
            string jsonstring = new JavaScriptSerializer().Serialize(list);
            return jsonstring;
        }

        [WebMethod]
        public string Get_UserMasterById(int id)
        {
            Model_UserMasters model = umrepo.Get_UserMastersById(id);
            string jsonstring = new JavaScriptSerializer().Serialize(model);
            return jsonstring;
        }

        [WebMethod]
        public string Get_SensorMaster(string search1,string search2,string search3,string drop1,string drop2,string drop3)
        {
            search1 = search1.ToLower();
            search2 = search2.ToLower();
            search3 = search3.ToLower();
            var data = sensorrepo.Get_Sensor_List();
            if (!string.IsNullOrEmpty(search1))
                if (drop1 == "PlantCode")
                    data = sensorrepo.Get_Sensor_List().Where(x => x.PlantCode.Contains(search1)).ToList();
                else if (drop1 == "MachineId")
                    data = sensorrepo.Get_Sensor_List().Where(x => x.MachineId.Contains(search1)).ToList();
                else
                    data = sensorrepo.Get_Sensor_List().Where(x => x.SensorIP.Contains(search1)).ToList();

            if(!string.IsNullOrEmpty(search2))
                if (drop2 == "MachineId")
                    data = sensorrepo.Get_Sensor_List().Where(x => x.MachineId.Contains(search2)).ToList();
                else if (drop2 == "PlantCode")
                    data = sensorrepo.Get_Sensor_List().Where(x => x.PlantCode.Contains(search2)).ToList();
                else
                    data = sensorrepo.Get_Sensor_List().Where(x => x.SensorIP.Contains(search2)).ToList();

            if (!string.IsNullOrEmpty(search3))
                if (drop3 == "SensorIP")
                    data = sensorrepo.Get_Sensor_List().Where(x => x.SensorIP.Contains(search3)).ToList();
                else if (drop3 == "PlantCode")
                    data = sensorrepo.Get_Sensor_List().Where(x => x.PlantCode.Contains(search3)).ToList();
                else
                    data = sensorrepo.Get_Sensor_List().Where(x => x.MachineId.Contains(search3)).ToList();

            string jsonresult = new JavaScriptSerializer().Serialize(data);
            return jsonresult;
        }

        [WebMethod]
        public string Get_Sensor_ById(int id)
        {
            tblSensorMaster sens = sensorrepo.Get_Sensor_by_Id(id);
            string json = new JavaScriptSerializer().Serialize(sens);
            return json;
        }
        //[WebMethod]
        //public string Get_ServiceMaster(string search1,string drop1)
        //{

        //}
        //[WebMethod]
        //public string Get_ServiceMasterById(int id)
        //{

        //}

#endregion

        #region CheckExistOrNot
        [WebMethod]
        public string Check_PlantMaster(string code)
        {
            code = code.ToLower();
            string jsonstring = string.Empty;
            PlantMaster mat = _plantrepo.Get_PlantList().Where(x => x.PlantCode.ToLower() == code && x.IsDeleted == false).FirstOrDefault();
            if (mat != null)
            {
                jsonstring = new JavaScriptSerializer().Serialize(mat);
            }         
            return jsonstring;
        }
        [WebMethod]
        public string Check_UserClassification(string userType)
        {
            userType = userType.ToLower();
            Model_UserClassification _user = _ucrepo.GetUserClassificationByUserType(userType);
            string jsonstring = string.Empty;
            if (_user.Id!=0)
            {
               jsonstring =  new JavaScriptSerializer().Serialize(_user);
            }
            return jsonstring;
        }
        [WebMethod]
        public string Check_UserMaster(string username)
        {
            username = username.ToLower();
            UserMaster user = umrepo.Get_UserMasterByUserName(username);
            string jsonstring = string.Empty;
            if(user!=null)
            {
                jsonstring = new JavaScriptSerializer().Serialize(user);
            }
            return jsonstring;
        }
        [WebMethod]
        public string Check_Supplier(string code)
        {
            string jsonstring = string.Empty;
            tblSupplier sup = _supplierrepo.Get_SupplierbyCode(code);
            if(sup!= null)
            {
                jsonstring = new JavaScriptSerializer().Serialize(sup);
            }     
            return jsonstring;
        }
        [WebMethod]
        public string Check_Transporter(string code)
        {
            string s = string.Empty;
            tblTransporter t = _transrepo.GetTransporter_ByCode(code);
            if (t != null)
            {
                s = new JavaScriptSerializer().Serialize(t);
            }
            return s;
        }
        [WebMethod]
        public string Check_PackingMaster(string code)
        {
            string result = string.Empty;
            PackingMaster m = _Packingrepo.Get_PackingByCode(code);
            if (m != null)
            {
                result = new JavaScriptSerializer().Serialize(m);
            }
            return result;
        }
        [WebMethod]
        public string Check_MaterialClassification(string code)
        {
            string jsons = string.Empty;
            Model_MaterialClassification m = _matrepo.Get_MaterialClassificationByCode(code.ToLower());
            if(m.Id > 0)
            {
                jsons = new JavaScriptSerializer().Serialize(m);
            }
            return jsons;
        }
        [WebMethod]
        public string Check_Materials(string code)
        {
            string json = string.Empty;
            Model_Materials m = _materialrepo.GetmaterialByCode(code.ToLower());
            if(m.Id > 0)
            {
                json = new JavaScriptSerializer().Serialize(m);
            }
            return json;
        }
        [WebMethod]
        public string Check_VehicleClassification(string code)
        {
            string json = string.Empty;
            Model_VehicleClassification v = _VCRepo.Get_Model_VehicleClassificationByCode(code.ToLower());
            if(v.Id > 0)
            {
                json = new JavaScriptSerializer().Serialize(v);
            }
            return json;
        }
        [WebMethod]
        public string Check_TruckMaster(string truckno)
        {
            string json = string.Empty;
            TruckMaster t = _truckRepo.GetTruckMasterByTruckNo(truckno);
            if (t != null)
            {
                json = new JavaScriptSerializer().Serialize(t);
            }
            return json;
        }
        [WebMethod]
        public string Check_WeightMachineMaster(string machineid)
        {
            string json = string.Empty;
            WeightMachineMaster m = _wrepo.GetMachineMaster_ByMachineId(machineid.ToLower());
            if (m != null)
            {
                json = new JavaScriptSerializer().Serialize(m);
            }
            return json;
        }
        [WebMethod]
        public string Check_MachineWorkingParameter(string plantcode,string machineid)
        {
            string json = string.Empty;
            IEnumerable<tblMachineWorkingParameter> list = _machinerepo.GetMachineWorkingParameters_List();
            if(!string.IsNullOrEmpty(plantcode) && !string.IsNullOrEmpty(machineid))
            {
                list = _machinerepo.GetMachineWorkingParameters_List().Where(x => x.PlantCode == plantcode && x.MachineId == machineid && x.IsDeleted == false).ToList();
            }         
            if (list.Count() > 0)
            {
                json = new JavaScriptSerializer().Serialize(list);
            }
            return json;
        }
        [WebMethod]
        public string Check_MachineWorkingParameterIPPort(string ipport)
        {
            string json = string.Empty;          
            IEnumerable<tblMachineWorkingParameter> list = _machinerepo.GetMachineWorkingParameters_List();            
            if (!string.IsNullOrEmpty(ipport))
            {
                list = _machinerepo.GetMachineWorkingParameters_List().Where(x => x.IPPort == ipport && x.IsDeleted == false).ToList();
            }
            if (list.Count() > 0)
            {
                json = new JavaScriptSerializer().Serialize(list);
            }
            return json;
        }
        [WebMethod]
        public string Check_CameraMaster(string plantcode,string machineid)
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(plantcode) && !string.IsNullOrEmpty(machineid))
            {
                IEnumerable<CameraMaster> list = _camrepo.GetCameraMasters_List();
                int count = _camrepo.GetCameraMasters_List().Count(x => x.PlantCodeID == plantcode && x.MachineId == machineid && x.IsDeleted == false);
                if (count >= 3)
                {
                    list = _camrepo.GetCameraMasters_List().Where(x => x.PlantCodeID == plantcode && x.MachineId == machineid && x.IsDeleted == false).ToList();
                    json = new JavaScriptSerializer().Serialize(list);
                }
            }
            return json;
        }
        [WebMethod]
        public string Check_CameraMasterIP(string plantcode, string cameraip)
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(plantcode) && !string.IsNullOrEmpty(cameraip))
            {
                IEnumerable<CameraMaster> list = _camrepo.GetCameraMasters_List();
                list = _camrepo.GetCameraMasters_List().Where(x => x.PlantCodeID == plantcode && x.CameraIP == cameraip && x.IsDeleted == false).ToList();
                if (list.Count() > 0)
                {
                    json = new JavaScriptSerializer().Serialize(list);
                }
            }
            return json;
        }
        [WebMethod]
        public string Check_BarrierMaster(string plantcode,string machineid)
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(plantcode) && !string.IsNullOrEmpty(machineid))
            {
                IEnumerable<BarrierMaster> list = _barrierrepo.GetBarrierMasters_List();
                int count = _barrierrepo.GetBarrierMasters_List().Count(x => x.PlantCodeId == plantcode && x.MachineId == machineid && x.IsDeleted == false);
                if (count >= 2)
                {
                    list = _barrierrepo.GetBarrierMasters_List().Where(x => x.PlantCodeId == plantcode && x.MachineId == machineid && x.IsDeleted == false).ToList();
                    json = new JavaScriptSerializer().Serialize(list);
                }
            }
            return json;
        }
        [WebMethod]
        public string Check_BarrierMasterIP(string plantcode, string barrierip)
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(plantcode) && !string.IsNullOrEmpty(barrierip))
            {
                IEnumerable<BarrierMaster> list = _barrierrepo.GetBarrierMasters_List();
                list = _barrierrepo.GetBarrierMasters_List().Where(x => x.PlantCodeId == plantcode && x.BarrierIP == barrierip && x.IsDeleted == false).ToList();
                if (list.Count() > 0)
                {
                    json = new JavaScriptSerializer().Serialize(list);
                }
            }
            return json;
        }
        [WebMethod]
        public string Check_AlphaDisplayMaster(string plantcode,string machineid)
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(plantcode) && !string.IsNullOrEmpty(machineid))
            {
                IEnumerable<AlphaDisplayMaster> list = _alpharepo.GetAlphaDisplayMasters_List();
                int count = _alpharepo.GetAlphaDisplayMasters_List().Count(x => x.PlantCodeId == plantcode && x.MachineId == machineid && x.IsDeleted == false);
                if (count >= 2)
                {
                    list = _alpharepo.GetAlphaDisplayMasters_List().Where(x => x.PlantCodeId == plantcode && x.MachineId == machineid && x.IsDeleted == false).ToList();
                    json = new JavaScriptSerializer().Serialize(list);
                }
            }
            return json;
        }
        [WebMethod]
        public string Check_AlphaDisplayIP(string plantcode, string ip)
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(plantcode)&& !string.IsNullOrEmpty(ip))
            {               
                IEnumerable<AlphaDisplayMaster> list = _alpharepo.GetAlphaDisplayMasters_List();
                list = _alpharepo.GetAlphaDisplayMasters_List().Where(x => x.PlantCodeId == plantcode && x.AlphaDisplayIP == ip && x.IsDeleted == false).ToList();
                if (list.Count() > 0)
                {
                    json = new JavaScriptSerializer().Serialize(list);
                }               
            }
            return json;
        }
        [WebMethod]
        public string Check_Sensor(string plantcode,string machineid)
        {
            string json = string.Empty;
            tblSensorMaster sens = sensorrepo.Get_Sensor_List().Where(x => x.PlantCode == plantcode && x.MachineId == machineid).FirstOrDefault();
            if (sens != null)
            {
                json = new JavaScriptSerializer().Serialize(sens);
            }
            return json;
        }
        [WebMethod]
        public string Check_SensorIP(string plantcode,string ip)
        {
            string json = string.Empty;
            tblSensorMaster sens = sensorrepo.Get_Sensor_List().FirstOrDefault(x => x.PlantCode == plantcode && x.SensorIP == ip);
            if (sens != null)
            {
                json = new JavaScriptSerializer().Serialize(sens);
            }
            return json;
        }
        #endregion

    }
}
