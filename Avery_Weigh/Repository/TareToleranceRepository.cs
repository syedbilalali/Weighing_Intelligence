using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class TaretrTareToleranceRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public IEnumerable<Model_TareToletrance> Get_TareTolerances()
        {
            var data = (from t in db.AverageTareSchemas
                        select new Model_TareToletrance
                        {
                            Description =  t.Description,
                            weightvalue  = t.weightvalue + " (" + t.Description + " )"
                        }).ToList();
            return data;
        }

        //Get:Transporter DataTable
        public DataTable GetTareToleranceDataTable()
        {
            var data = db.AverageTareSchemas.ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr No");
            dt.Columns.Add("Description");
            dt.Columns.Add("weightvalue");
            
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["Description"] = item.Description;
                dr["weightvalue"] = item.weightvalue;
               
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        //internal IList<DynamicFieldName> GetFieldNameByMachine(string machineID, string PlantId)
        //{
        //    IList<DynamicFieldName> data = db.DynamicFieldNames.Where(x => x.MachineId == machineID && x.PlantId == PlantId).ToList();
        //    return data.ToList();
        //}

        //Get:Transporter List Using Model
        public IEnumerable<Model_TareToletrance> GetTareTolerance_List()
        {
            var data = (from t in db.AverageTareSchemas
                        select new Model_TareToletrance
                        {
                            Id = t.Id,
                            Description = t.Description,
                            weightvalue = t.weightvalue.ToString()
                            
                        }).ToList();
            return data;
        }

        //Get:Active Tranporter List
        public IEnumerable<AverageTareSchema> GetTareToleranceList()
        {
            IList<AverageTareSchema> data = db.AverageTareSchemas.ToList();
            return data;
        }

        //Get:Transporter by transporter code
        public AverageTareSchema GetTareTolerance_ByCode(string code)
        {
            AverageTareSchema trans = db.AverageTareSchemas.Where(x => x.Description == code).SingleOrDefault();
            return trans;
        }

        //Save:Excel data to the server
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
                        string _Description = dr["Description"].ToString();
                        string _weightvalue = dr["weightvalue"].ToString();
                      
                        var _transporter = db.AverageTareSchemas.Where(x => x.Description == _Description).FirstOrDefault();
                        if (_transporter != null)
                        {
                            if (string.IsNullOrEmpty(_Description))
                            {
                                _failed++;
                            }
                            else
                            {
                                _transporter.Description = _Description;
                                _transporter.weightvalue= Convert.ToDecimal(_weightvalue);
                               
                                db.SubmitChanges();
                                _update++;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_Description) && !string.IsNullOrEmpty(_weightvalue))
                            {
                                AverageTareSchema _trans = db.AverageTareSchemas.Where(x => x.Description == _Description).FirstOrDefault();
                                if (_trans != null)
                                {
                                    _failed++;
                                }
                                else
                                {
                                    db.AverageTareSchemas.InsertOnSubmit(new AverageTareSchema
                                    {
                                        Description = _Description,
                                        weightvalue = Convert.ToDecimal(_weightvalue)
                                      
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

        //Delete:Tranporter by Id
        public bool Delete_TareToleranceById(int id)
        {
            bool status = false;
            AverageTareSchema tran = db.AverageTareSchemas.FirstOrDefault(x => x.Id == id );
            if (tran != null)
            {
                db.AverageTareSchemas.DeleteOnSubmit(tran);
                //tran.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;

        } 

        //Get:Transporter By Id
        public AverageTareSchema Get_TareToleranceById(int id)
        {
            AverageTareSchema tran = db.AverageTareSchemas.FirstOrDefault(x => x.Id == id);
            return tran;
        }

        //Get:Transporter code
        public IEnumerable<Model_TareToletrance> Get_TransporterCode()
        {
            IEnumerable<Model_TareToletrance> list = (from t in db.AverageTareSchemas
                                                     
                                                   select new Model_TareToletrance
                                                   {
                                                       //Code = t.Code,
                                                       //Name = t.Name + "( " + t.Code + " )"
                                                       Description = t.Description,
                                                       weightvalue = t.weightvalue + " (" + t.Description + " )"
                                                   }).ToList();
            return list;
        }

        //Add:New Transporter Record
        public bool Add_TareTolerance(AverageTareSchema tran)
        {
            bool status = false;
            if (tran != null)
            {
                db.AverageTareSchemas.InsertOnSubmit(tran);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        public IEnumerable<Model_TareToletrance> Get_TareToleranceType_Add()
        {
            IEnumerable<Model_TareToletrance> list = (from t in db.AverageTareSchemas
                                                         
                                                          select new Model_TareToletrance
                                                          {
                                                              Description = t.Description
                                                          }).ToList();
            return list;
        }
    }
}