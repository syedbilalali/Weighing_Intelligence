using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class UserMasterRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        //Get:Return usermaster list
        public IEnumerable<Model_UserMasters> Get_Users()
        {
            List<Model_UserMasters> data = (from a in db.UserMasters
                                            join b in db.UserClassifications on a.UserType equals b.Id
                                            join c in db.PlantMasters on a.Plantcode equals c.PlantCode                                        
                                            where a.IsDeleted == false
                                            select new Model_UserMasters
                                            {
                                                _UserMaster = a,
                                                _UserClassification = b,
                                                _PlantMaster = c,
                                                WeighbridgeId = getuserweighingmachines(a.Id)
                                            }).ToList();
            return data;
        }

        //Return Weighing machines by user id
        private string getuserweighingmachines(int id)
        {
            string name = "";
            IList<WeightMachineMaster> a = getUserWeighMachinesNames(id).ToList();
            foreach(WeightMachineMaster umm in a)
            {
                name = name + umm.MachineId+",";
            }
            return name;
        }

        //Get:UserMaster by id
        internal UserMaster GetUserMaster_ById(int id)
        {
            UserMaster um = db.UserMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return um;
        }

        //Check User Credentials
        internal UserMaster CheckUserCredentials(string UserName, string Password, string WBId, string PlantId)
        {
            UserMaster res = new UserMaster();
            var um = (from a in db.UserMasters
                      join b in db.PlantMasters on a.Plantcode equals b.PlantCode
                      where a.UserName == UserName && a.Password == Password && b.PlantCode == PlantId
                      select a).FirstOrDefault();
            if (um != null)
            {
                IEnumerable<WeightMachineMaster> data = (from a in db.UserWeightMachineMasters
                                                         join b in db.WeightMachineMasters on a.WeightMachineId equals b.Id
                                                         where b.MachineId == WBId
                                                         select b).ToList();
                if (data.Count() > 0)
                {
                    HttpContext.Current.Session["InstalledOn"] = data.FirstOrDefault().InstallationDate.Value.ToShortDateString();
                    res = um;
                }
            }
            return res;
        }

        internal UserClassification GetUserAuthorization(int id)
        {
            UserClassification um = (from a in db.UserMasters
                                     join b in db.UserClassifications on a.UserType equals b.Id
                                     where a.Id == id
                                     select b).FirstOrDefault();
            return um;
        }

        //Get:UserMaster By Id
        public Model_UserMasters Get_UserMastersById(int id)
        {
            Model_UserMasters model = (from t in db.UserMasters
                                       join s in db.PlantMasters on t.Plantcode equals s.PlantCode
                                       join m in db.UserClassifications on t.UserType equals m.Id
                                       select new Model_UserMasters
                                       {
                                           _UserMaster = t,
                                           _PlantMaster = s,
                                           _UserClassification = m,
                                           WeighbridgeId = getuserweighingmachines(id)
                                       }).FirstOrDefault();
            return model;
        }

        //Retrun User Weighing Machines by user id
        internal IEnumerable<UserWeightMachineMaster> getUserWeighMachines(int id)
        {
            IEnumerable<UserWeightMachineMaster> data = db.UserWeightMachineMasters.Where(x => x.UserId == id).ToList();
            return data;
        }

        //Retruns user weigh machines names by user id
        internal IEnumerable<WeightMachineMaster> getUserWeighMachinesNames(int id)
        {
            IEnumerable<WeightMachineMaster> data = (from a in db.UserWeightMachineMasters
                                                     join b in db.WeightMachineMasters on a.WeightMachineId equals b.Id
                                                     where a.UserId == id
                                                     select b).ToList();
            return data;
        }

        //Return DataTable with UserMaster Data
        public DataTable GetDataTable_UserMaster()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr No");
            dt.Columns.Add("User Name");
            dt.Columns.Add("Password");
            dt.Columns.Add("User Type");
            dt.Columns.Add("Plant Code");
            dt.Columns.Add("Weigh Machines");
            int index = 1;
            IEnumerable<Model_UserMasters> _UM = (from u in db.UserMasters
                                                  join uc in db.UserClassifications on u.UserType equals uc.Id
                                                  join p in db.PlantMasters on u.Plantcode equals p.PlantCode
                                                  where u.IsDeleted == false
                                                  select new Model_UserMasters
                                                  {
                                                      _UserMaster = u,
                                                      _UserClassification = uc,
                                                      _PlantMaster = p                                                     
                                                  }).ToList();
            foreach (var item in _UM)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["User Name"] = item._UserMaster.UserName;
                dr["Password"] = item._UserMaster.Password;
                dr["User Type"] = item._UserClassification.UserType;
                dr["Plant Code"] = item._PlantMaster.PlantCode;
                dr["Weigh Machines"] = getUserWeigh(item._UserMaster.Id);
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        private string getUserWeigh(int id)
        {
            string machines = "";
            IList<WeightMachineMaster> usermachines = getUserWeighMachinesNames(id).ToList();
            foreach(WeightMachineMaster um in usermachines)
            {
                machines = machines + um.MachineId + ",";
            }
            try
            {
                machines = machines.Remove(machines.Length - 1, 1);
            }
            catch { }
            return machines;
        }

       

        //Save Excel File Data To SQL Server
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
                    int userid = 0;
                    string _UserName = dr["User Name"].ToString();
                    string _Password = dr["Password"].ToString();
                    string _UserType = dr["User Type"].ToString();
                    string _PlantCode = dr["Plant Code"].ToString();
                    string _machines = dr["Weigh Machines"].ToString();
                    int _UserTypeId = 0;
                    try
                    {
                        _UserTypeId = db.UserClassifications.FirstOrDefault(x => x.UserType == _UserType && x.IsDeleted == false).Id;
                    }
                    catch { }
                    PlantMaster _pm = db.PlantMasters.FirstOrDefault(x => x.PlantCode == _PlantCode && x.IsDeleted == false);
                    var data = db.UserMasters.FirstOrDefault(x => x.UserName == _UserName && x.IsDeleted == false);
                    if (data != null)
                    {
                        int id = 0;
                        try
                        {
                            id = db.UserMasters.FirstOrDefault(x => x.UserName == _UserName && x.IsDeleted == false).Id;
                        }
                        catch
                        {
                        }
                        userid = id;
                        UserMaster _um = db.UserMasters.FirstOrDefault(x => x.UserName == _UserName & x.IsDeleted == false && x.Id != id);
                        if (_um != null)
                        {
                            _failed++;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_Password) && !string.IsNullOrEmpty(_UserName) && _UserTypeId > 0 && _pm != null)
                            {
                                data.UserName = _UserName;
                                data.Password = _Password;
                                data.UserType = _UserTypeId;
                                data.Plantcode = _PlantCode;
                                db.SubmitChanges();
                                _update++;
                            }
                            else
                            {
                                _failed++;
                            }
                        }
                    }
                    else
                    {
                        int id = db.UserClassifications.FirstOrDefault(x => x.UserType == _UserType && x.IsDeleted == false).Id;
                        var _plant = db.PlantMasters.FirstOrDefault(x => x.PlantCode == _PlantCode && x.IsDeleted == false);
                        if(id > 0 && _plant!=null && !string.IsNullOrEmpty(_UserName) && !string.IsNullOrEmpty(_Password))
                        {
                            var _user = db.UserMasters.FirstOrDefault(x => x.UserName == _UserName && x.IsDeleted == false);
                            if (_user != null)
                            {
                                _failed++;
                            }
                            else
                            {
                                UserMaster _um = new UserMaster();
                                _um.UserName = _UserName;
                                _um.Password = _Password;
                                _um.UserType = id;
                                _um.Plantcode = _PlantCode;
                                db.UserMasters.InsertOnSubmit(_um);
                                db.SubmitChanges();
                                userid = _um.Id;
                                _success++;
                            }
                        }
                    }

                    string[] machines = _machines.Split(',');
                    foreach(string _mc in machines)
                    {
                        int machineId = 0;
                        try
                        {
                            machineId = db.WeightMachineMasters.Where(x => x.MachineId == _mc).FirstOrDefault().Id;
                        }
                        catch { }
                        if(machineId != 0 && userid != 0)
                        {
                            UserWeightMachineMaster uwmm = db.UserWeightMachineMasters.Where(x => x.UserId == userid && x.WeightMachineId == machineId).FirstOrDefault();
                            if(uwmm == null)
                            {
                                uwmm = new UserWeightMachineMaster();
                                uwmm.WeightMachineId = machineId;
                                uwmm.UserId = userid;
                                db.UserWeightMachineMasters.InsertOnSubmit(uwmm);
                                db.SubmitChanges();
                            }
                        }                                     
                    }
                }
                result = "Result: Not saved: " + _failed + ", Saved: " + _success + ", Updated: " + _update;
            }
            return result;
        }

        //Get:UserMaster By UserName
        public UserMaster Get_UserMasterByUserName(string username)
        {
            var data = (from t in db.UserMasters
                        where t.UserName == username && t.IsDeleted == false
                        select t).FirstOrDefault();
            return data;
        }

        //Get:UserMaster List
        public IEnumerable<UserMaster> Get_UserMaster_List()
        {
            IEnumerable<UserMaster> list = db.UserMasters.Where(x => x.IsDeleted == false).ToList();
            return list;
        } 

        //Get: Delete User Master
        public bool Delete_UserMasterById(int id)
        {
            bool status = false;
            UserMaster um = db.UserMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (um != null)
            {
                um.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }     
      
        //Add:New UserMaster
        public bool Add_UserMaster(UserMaster user)
        {
            bool status = false;
            if (user != null)
            {
                db.UserMasters.InsertOnSubmit(user);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }
    }
}