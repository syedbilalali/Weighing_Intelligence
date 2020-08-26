using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Repository
{
    public class ServiceMasterRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        RegexRepository regex = new RegexRepository();
        public bool Add_ServiceMaster(ServiceMaster master)
        {
            bool status = false;
            if (master != null)
            {
                db.ServiceMasters.InsertOnSubmit(master);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        public bool Delete_ServiceMaster(int id)
        {
            bool status = false;
            if (id > 0)
            {
                ServiceMaster sm = db.ServiceMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
                sm.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        public IEnumerable<ServiceMaster> Get_ServiceMasterList()
        {
            IEnumerable<ServiceMaster> list = db.ServiceMasters.Where(x => x.IsDeleted == false);
            return list;
        }

        public ServiceMaster Get_ServiceMasterById(int id)
        {
            ServiceMaster sm = db.ServiceMasters.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return sm;
        }

        public DataTable Get_ServiceMaster_DataTable()
        {
            var data = Get_ServiceMasterList();
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("AMC Type");
            dt.Columns.Add("AMC Contact No");
            dt.Columns.Add("AMC Valid Upto");
            dt.Columns.Add("AMC Reminder");
            dt.Columns.Add("Stamping Date");
            dt.Columns.Add("Stamping Reminder");
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = item.Id;
                dr["AMC Type"] = item.AMCType.ToString();
                
                dr["AMC Contact No"] = item.AMCContactNo.ToString();
                dr["AMC Valid Upto"] = regex.Get_FormatDate(item.AMCValidUpto);
                dr["AMC Reminder"] = item.AMCReminder;
                dr["Stamping Date"] = regex.Get_FormatDate(item.StampingDate);
                dr["Stamping Reminder"] = item.StampingReminder;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public string SaveDataToServer(DataSet ds)
        {
            string result = string.Empty;
            int fail = 0;
            int success = 0;
            int update = 0;
            int Id = 0;
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                try
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            Id = Convert.ToInt32(dr["Id"]);
                        }
                        catch {
                            Id = 0;
                        }
                        
                        string AMCType = dr["AMC Type"].ToString();
                        string AMCContactNo = dr["AMC Contact No"].ToString();
                        DateTime AMCValidUpto = Convert.ToDateTime(dr["AMC Valid Upto"].ToString());
                        int AMCReminder = Convert.ToInt32(dr["AMC Reminder"].ToString());
                        DateTime StampingDate = Convert.ToDateTime(dr["Stamping Date"].ToString());
                        int  StampingReminder = Convert.ToInt32(dr["Stamping Reminder"].ToString());
                        if (AMCType.ToLower() == "Silver(No Spares)".ToLower() || AMCType.ToLower() == "Gold(Without Load Cells)".ToLower() || AMCType.ToLower() == "Platinum(All Spares)".ToLower())
                        {
                            ServiceMaster sm = Get_ServiceMasterById(Id);
                            if (sm != null)
                            {
                                sm.AMCType = AMCType;
                                sm.AMCContactNo = AMCContactNo;
                                sm.AMCReminder = AMCReminder;
                                sm.AMCValidUpto = AMCValidUpto;
                                sm.StampingDate = StampingDate;
                                sm.StampingReminder = StampingReminder;
                                db.SubmitChanges();
                                update++;
                            }
                            else
                            {
                                ServiceMaster master = new ServiceMaster();
                                master.AMCType = AMCType;
                                master.AMCReminder = AMCReminder;
                                master.AMCContactNo = AMCContactNo;
                                master.AMCValidUpto = AMCValidUpto;
                                master.StampingDate = StampingDate;
                                master.StampingReminder = StampingReminder;
                                master.IsDeleted = false;
                                db.ServiceMasters.InsertOnSubmit(master);
                                db.SubmitChanges();
                                success++;
                            }
                        }
                        else
                        {
                            fail++;
                        }
                    }
                }
                catch(Exception ex)
                {
                    fail++;
                }

                result = "New Added: " + success + "  Updated: " + update + "  Failed: " + fail + "";
            }
            return result;
        }
    }
}