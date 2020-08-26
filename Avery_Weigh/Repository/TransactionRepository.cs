using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class TransactionRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public int GetTripId()
        {
            if (db.tblTransactions.Count() > 0)
            {
                int RowId = db.tblTransactions.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                return RowId + 1;
            }
            else
            {
                return 1;
            }
        }

        public int GetTripId(string wbid)
        {
            tblTransaction trans = db.tblTransactions.FirstOrDefault(x => x.WeighbridgeId == wbid);
            if (trans != null)
            {
                if (db.tblTransactions.Count() > 0)
                {
                    //int RowId = db.tblTransactions.OrderByDescending(x => x.WeighbridgeId == wbid).FirstOrDefault().Id;
                    //return RowId;

                    tblTransaction wei = (from a in db.tblTransactions
                                          join b in db.tblTransactions on a.Id equals b.TripId 
                                          where a.WeighbridgeId == wbid 
                                          select b).ToList().Last();

                    return wei.Id;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }

        public int GetTripId_new(string wbid,string vartripid,string vartruck)
        {
            tblTransaction trans = db.tblTransactions.FirstOrDefault(x => x.WeighbridgeId == wbid && x.TruckNo== vartruck && x.TripId == Convert.ToInt32(vartripid));
            if (trans != null)
            {
                if (db.tblTransactions.Count() > 0)
                {
                    //int RowId = db.tblTransactions.OrderByDescending(x => x.WeighbridgeId == wbid).FirstOrDefault().Id;
                    //return RowId;

                    tblTransaction wei = (from a in db.tblTransactions
                                          join b in db.tblTransactions on a.Id equals b.TripId
                                          where a.WeighbridgeId == wbid && a.TruckNo==vartruck && a.TripId == Convert.ToInt32(vartripid)
                                          select b).ToList().Last();

                    return wei.Id;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }


        public int GetTripId_SS(string wbid, string vartripid, string vartruck)
        {
            tblTransaction trans = db.tblTransactions.FirstOrDefault(x => x.WeighbridgeId == wbid && x.TruckNo == vartruck);
            if (trans != null)
            {
                if (db.tblTransactions.Count() > 0)
                {
                    //int RowId = db.tblTransactions.OrderByDescending(x => x.WeighbridgeId == wbid).FirstOrDefault().Id;
                    //return RowId;

                    var max_tripid = (from a in db.tblTransactions
                                      where a.WeighbridgeId == wbid && a.TruckNo == vartruck
                                      select a).Max(a => a.TripId);

                    return Convert.ToInt32(max_tripid);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }



        public bool checkTruckIsPendingOrNot(string TruckNo)
        {
            bool check = false;
            tblTransaction trans = db.tblTransactions.Where(x => x.TruckNo == TruckNo && x.TransactionStatus == 1).FirstOrDefault();
            if (trans != null)
            {
                check = true;
                
            }
            return check;
        }

        public string  checkTruckTolerance(string TruckNo)
        {
            
            tblTransaction trans = db.tblTransactions.Where(x => x.TruckNo == TruckNo && x.TransactionStatus == 1).FirstOrDefault();
            if (trans != null)
                return trans.TARETOLERANCESTATUS.ToString();
            else
                return "";
        }

        internal Model_ManualWeight gettransactionDetail(string truckNo)
        {
            Model_ManualWeight model = new Model_ManualWeight();
            tblTransaction trans = db.tblTransactions.Where(x => x.TruckNo == truckNo && x.TransactionStatus == 1).FirstOrDefault();
            if (trans != null)
            {
                model.trans = trans;
                //IEnumerable<tblTransactionWeight> weightList = db.tblTransactionWeights.Where(x => x.TransactionId == trans.Id);
                //model.transWeight = weightList;
            }
            return model;
        }

        //public List<Mode_transactionWeight> GetTruckWeight(string truckNo)
        //{
        //    var res = (from a in db.tblTransactions
        //               join b in db.tblTransactionWeights on a.Id equals b.TransactionId
        //               where a.TruckNo == truckNo && a.TransactionStatus == 1
        //               select new Mode_transactionWeight
        //               {
        //                   TransactionId = (int)b.TransactionId,
        //                   Id = b.Id,
        //                   Weight = (decimal)b.Weight,
        //                   WeightDate = (DateTime)b.WeightDate,
        //                   WeightTime = (DateTime)b.WeightTime,
        //               }).ToList();

        //    return res;
        //}

        internal void saveTransactionRecord(Model_ManualWeight model)
        {
            tblTransaction trans = db.tblTransactions.FirstOrDefault(x => x.TruckNo == model.trans.TruckNo && x.TransactionStatus == 1);
            if (trans == null)
            {
                trans = new tblTransaction();
                trans = model.trans;
                trans.WeighbridgeId = model.WeibridgeId;
                trans.CreatedBy = model.UserName;
                trans.Shift = model.shiftName;
                trans.PlantCode = model.plantCode;
                trans.CompanyCode = model.companyCode;
                db.tblTransactions.InsertOnSubmit(trans);
                db.SubmitChanges();
                int transId = trans.Id;
                //tblTransactionWeight wei = model.Weight;
                //wei.TransactionId = transId;
                //db.tblTransactionWeights.InsertOnSubmit(wei);
                //db.SubmitChanges();
            }
            else
            {
                trans.TripType = model.trans.TripType;
                trans.SecondWeight = model.trans.SecondWeight;
                trans.SecondWtDateTime = model.trans.SecondWtDateTime;
                trans.NetWeight = model.trans.NetWeight;
                trans.TripType = model.trans.TripType;
                trans.Shift = model.trans.Shift;
                trans.SHIFTDATE = model.trans.SHIFTDATE;
                trans.FrontImage2 = model.trans.FrontImage2;
                trans.BackImage2 = model.trans.BackImage2;
                trans.PlantCode = model.trans.PlantCode;
                trans.CompanyCode = model.trans.CompanyCode;
                trans.WeighbridgeId = model.trans.WeighbridgeId;
                trans.SupplierCode = model.trans.SupplierCode;
                trans.SupplierName = model.trans.SupplierName;
                trans.TransporterCode = model.trans.TransporterCode;
                trans.TransporterName = model.trans.TransporterName;
                trans.MaterialCode = model.trans.MaterialCode;
                trans.MaterialName = model.trans.MaterialName;
                if (trans.IsMultiProduct == false)
                {
                    trans.TransactionStatus = 2;
                    db.SubmitChanges();
                }
                int transId = trans.Id;
                if(model.material != null)
                {
                    db.tblTransactionMaterials.InsertOnSubmit(model.material);
                    db.SubmitChanges();
                }

                TruckTareWeight _tarewt = db.TruckTareWeights.FirstOrDefault();
                _tarewt = new TruckTareWeight();
                if (trans.TripType == 1)
                {
                    _tarewt.TruckNo = trans.TruckNo;
                    _tarewt.TareWeight = trans.SecondWeight;
                    _tarewt.TareWtDateTime = trans.SecondWtDateTime;
                }
                else
                {
                    _tarewt.TruckNo = trans.TruckNo;
                    _tarewt.TareWeight = trans.FirstWeight;
                    _tarewt.TareWtDateTime = trans.FirstWtDateTime;
                }
                db.TruckTareWeights.InsertOnSubmit(_tarewt);
                db.SubmitChanges();

            }
        }

        internal tblMachineWorkingParameter getMachineWorkingParameters(string machineId)
        {
            tblMachineWorkingParameter tbl = db.tblMachineWorkingParameters.FirstOrDefault(x => x.MachineId == machineId);
            return tbl;
        }

        internal bool DeleteTransactionRecord(int id)
        {
            bool res = false;
            tblTransaction trans = db.tblTransactions.FirstOrDefault(x => x.Id == id);
            if(trans != null)
            {
                db.tblTransactions.DeleteOnSubmit(trans);
                db.SubmitChanges();
                res = true;
            }
            return res;
        }

        internal IEnumerable<tblTransactionMaterial> gettransactionmaterials(string truckNo)
        {
            IEnumerable<tblTransactionMaterial> materials = (from a in db.tblTransactions
                                                      join b in db.tblTransactionMaterials on a.Id equals b.TransactionId
                                                      where a.TruckNo == truckNo && a.TransactionStatus == 1
                                                      select b).ToList();

            return materials;
        }

        internal void InsertTripMaterialsInfo(tblTransactionMaterial mat)
        {
            tblTransactionMaterial _transMaterial = db.tblTransactionMaterials.FirstOrDefault(x => x.TransactionId == mat.TransactionId && x.MaterialCode == mat.MaterialCode);
            if(_transMaterial == null)
            {
                mat.CreteDate = DateTime.Now;
                db.tblTransactionMaterials.InsertOnSubmit(mat);
                db.SubmitChanges();
            }
            else
            {
                mat.CreteDate = DateTime.Now;
                _transMaterial.Weight = mat.Weight;
                db.SubmitChanges();
            }

        }

        internal bool checkTruckTripClosed(string truckNo)
        {
            bool check = false;
            tblTransaction trans = db.tblTransactions.Where(x => x.TruckNo == truckNo && x.TransactionStatus == 1).FirstOrDefault();
            if (trans != null)
            {
                check = true;
            }
            return check;
        }

        //internal tblTransactionWeight getFirstWeightFromTruckTrip(string truckNo)
        //{
        //    tblTransactionWeight wei = (from a in db.tblTransactions
        //                                join b in db.tblTransactionWeights on a.Id equals b.TransactionId
        //                                where a.TruckNo == truckNo && a.TransactionStatus == 1
        //                                select b).ToList().First();

        //    return wei;
        //}

        //internal tblTransactionWeight getlastWeightFromTruckTrip(string truckNo)
        //{
        //    tblTransactionWeight wei = (from a in db.tblTransactions
        //                                join b in db.tblTransactionWeights on a.Id equals b.TransactionId
        //                                where a.TruckNo == truckNo && a.TransactionStatus == 1
        //                                select b).ToList().Last();

        //    return wei;
        //}

        public IList<Model_Records> GetTransactions(int transType)
        {
            IList<Model_Records> Records = (from a in db.tblTransactions
                                            where a.TransactionStatus == transType
                                            select new Model_Records
                                            {
                                                Id = a.Id,
                                                Materials = a.MaterialName,
                                                Supplier = a.SupplierName,
                                                TruckNo = a.TruckNo,
                                                TripType = a.TripType == 0 ? "N/A" : a.TripType == 1 ? "In" : "Out",
                                                DateIn = getTransData(a.FirstWtDateTime, "FirstDate"), //a.FirstWtDateTime == null ? "--" : Convert.ToDateTime(a.FirstWtDateTime.Value).ToShortDateString(),
                                                DateOut = getTransDate2(a.SecondWtDateTime),
                                                TimeIn = getTransTime1(a.FirstWtDateTime),
                                                TimeOut = getTransTime2(a.SecondWtDateTime),
                                                GrossWt = a.TripType == 1 ? a.FirstWeight.ToString() : a.SecondWeight.ToString(),
                                                NetWt = a.NetWeight.ToString(),
                                                TareWt = a.TripType == 1 ? a.SecondWeight.ToString() : a.FirstWeight.ToString(),
                                                Transporter = a.TransporterName,
                                                IsMultiProduct = a.IsMultiProduct == true ? "True" : "False",

                                            }).ToList();
            return Records;
        }

        public IList<Model_Records> GetTransactionsTolerance(int transType)
        {
            IList<Model_Records> Records = (from a in db.tblTransactions
                                            where a.TransactionStatus == transType && a.TARETOLERANCESTATUS=="No"
                                            select new Model_Records
                                            {
                                                Id = a.Id,
                                                Materials = a.MaterialName,
                                                Supplier = a.SupplierName,
                                                TruckNo = a.TruckNo,
                                                TripType = a.TripType == 0 ? "N/A" : a.TripType == 1 ? "In" : "Out",
                                                DateIn = getTransData(a.FirstWtDateTime, "FirstDate"), //a.FirstWtDateTime == null ? "--" : Convert.ToDateTime(a.FirstWtDateTime.Value).ToShortDateString(),
                                                DateOut = getTransDate2(a.SecondWtDateTime),
                                                TimeIn = getTransTime1(a.FirstWtDateTime),
                                                TimeOut = getTransTime2(a.SecondWtDateTime),
                                                GrossWt = a.TripType == 1 ? a.FirstWeight.ToString() : a.SecondWeight.ToString(),
                                                NetWt = a.NetWeight.ToString(),
                                                TareWt = a.TripType == 1 ? a.SecondWeight.ToString() : a.FirstWeight.ToString(),
                                                Transporter = a.TransporterName,
                                                IsMultiProduct = a.IsMultiProduct == true ? "True" : "False",

                                            }).ToList();
            return Records;
        }

        private string getTransTime2(DateTime? secondWtDateTime)
        {
            if (secondWtDateTime == null)
                return "--";
            else
                return secondWtDateTime.Value.ToShortTimeString();
        }

        private string getTransTime1(DateTime? firstWtDateTime)
        {
            if (firstWtDateTime == null)
                return "--";
            else
                return firstWtDateTime.Value.ToShortTimeString();

        }

        private string getTransDate2(DateTime? secondWtDateTime)
        {
            if (secondWtDateTime == null)
                return "--";
            else
                return secondWtDateTime.Value.ToShortDateString();
        }

        private string getTransData(DateTime? firstWtDateTime, string type)
        {
            if (firstWtDateTime == null)
                return "";
            else
                return firstWtDateTime.Value.ToShortDateString();
        }

        //private string getTransRecord(int id, int tripType, string action)
        //{
        //    string result = "";
        //    if (tripType == 0 || tripType == 1)
        //    {
        //        IList<tblTransactionWeight> wt = db.tblTransactionWeights.Where(x => x.TransactionId == id).OrderBy(x => x.Id).ToList();
        //        if (wt != null)
        //        {
        //            switch (action)
        //            {
        //                case "DateIn":
        //                    result = wt.First().WeightDate.Value.ToString("dd/MM/yyyy");
        //                    break;

        //                case "DateOut":
        //                    result = wt.Last().WeightDate.Value.ToString("dd/MM/yyyy");
        //                    break;

        //                case "TimeIn":
        //                    result = wt.First().WeightTime.Value.ToString("HH:mm:ss tt");
        //                    break;

        //                case "TimeOut":
        //                    result = wt.Last().WeightTime.Value.ToString("HH:mm:ss tt");
        //                    break;

        //                case "GrossWt":
        //                    result = wt.First().Weight.ToString();
        //                    break;

        //                case "NetWt":
        //                    result = (wt.First().Weight - wt.Last().Weight).ToString();
        //                    break;

        //                case "TareWt":
        //                    result = wt.Last().Weight.ToString();
        //                    break;
        //                default:
        //                    result = "";
        //                    break;

        //            }
        //        }
        //    }
        //    else
        //    {
        //        IList<tblTransactionWeight> wt = db.tblTransactionWeights.Where(x => x.TransactionId == id).OrderBy(x => x.Id).ToList();
        //        if (wt != null)
        //        {
        //            switch (action)
        //            {
        //                case "DateIn":
        //                    result = wt.First().WeightDate.Value.ToString("dd/MM/yyyy");
        //                    break;

        //                case "DateOut":
        //                    result = wt.Last().WeightDate.Value.ToString("dd/MM/yyyy");
        //                    break;

        //                case "TimeIn":
        //                    result = wt.First().WeightTime.Value.ToString("HH:mm:ss tt");
        //                    break;

        //                case "TimeOut":
        //                    result = wt.Last().WeightTime.Value.ToString("HH:mm:ss tt");
        //                    break;

        //                case "GrossWt":
        //                    result = wt.Last().Weight.ToString();
        //                    break;

        //                case "NetWt":
        //                    result = (wt.Last().Weight - wt.First().Weight).ToString();
        //                    break;

        //                case "TareWt":
        //                    result = wt.First().Weight.ToString();
        //                    break;
        //                default:
        //                    result = "";
        //                    break;

        //            }
        //        }
        //    }
        //    return result;
        //}

        private string getTransactionmaterials(int id)
        {
            string name = "";
            IList<tblTransactionMaterial> material = db.tblTransactionMaterials.Where(x => x.TransactionId == id).ToList();
            foreach(tblTransactionMaterial M in material)
            {
                name = name + M.MaterialName + ",";
            }
            if (name == "")
                return "";
            else
                return name.Remove(name.Length - 1, 1);
        }

        public tblTransaction getTransactionById(int Id, int transType)
        {
            tblTransaction trans = db.tblTransactions.FirstOrDefault(x => x.Id == Id && x.TransactionStatus == transType);
            return trans;
        }

        public tblTransaction getTransactionByTripId(int Id)
        {
            tblTransaction trans = db.tblTransactions.FirstOrDefault(x => x.TripId == Id);
            return trans;
        }

        public tblTransaction getPendingTransactionById(string TruckNo)
        {
            tblTransaction trans = db.tblTransactions.FirstOrDefault(x => x.TruckNo == TruckNo && x.TransactionStatus == 1);
            return trans;
        }

        public IList<tblTransactionMaterial> getmaterialsByTransactionId(int transId)
        {
            IList<tblTransactionMaterial> mat = db.tblTransactionMaterials.Where(x => x.TransactionId == transId).ToList();
            return mat;
        }

        internal void CloseTicket(int transId)
        {
            tblTransaction trans = db.tblTransactions.FirstOrDefault(x => x.TripId == transId);
            if(trans != null)
            {
                trans.TransactionStatus = 2;
                db.SubmitChanges();
            }
        }
    }
}