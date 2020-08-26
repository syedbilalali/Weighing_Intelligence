using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class DynamicFieldRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public IEnumerable<DynamicFieldName> getFieldsUsingMachineId(string plantId, string machineId)
        {
            var list = db.DynamicFieldNames.Where(x => x.PlantId == plantId && x.MachineId == machineId).ToList();
            return list;
        }

        public void InsertFieldNames(string plantId, string machineId, string fieldname, string fieldvalue, bool chk1, bool chk2, bool chk3)
        {
            DynamicFieldName _dynamic = db.DynamicFieldNames.Where(x => x.PlantId == plantId && x.MachineId == machineId && x.FieldName == fieldname).FirstOrDefault();
            if(_dynamic == null)
            {
                _dynamic = new DynamicFieldName();
                _dynamic.FieldName = fieldname;
                _dynamic.FieldValue = fieldvalue;
                _dynamic.IsMandatory1 = chk1;
                _dynamic.IsMandatory2 = chk2;
                _dynamic.IsRequired = chk3;
                
                _dynamic.MachineId = machineId;
                _dynamic.PlantId = plantId;
                db.DynamicFieldNames.InsertOnSubmit(_dynamic);
                db.SubmitChanges();
            }
            else
            {
                _dynamic.FieldName = fieldname;
                _dynamic.FieldValue = fieldvalue;
                _dynamic.IsMandatory1 = chk1;
                _dynamic.IsMandatory2 = chk2;
                _dynamic.IsRequired = chk3;
                _dynamic.MachineId = machineId;
                _dynamic.PlantId = plantId;
                db.SubmitChanges();
            }
        } 

        public IEnumerable<DynamicFieldModel> GetDynamicFieldsList()
        {
            var list = (from a in db.DynamicFieldNames
                        select new DynamicFieldModel
                        {
                            PlantId = a.PlantId,
                            MachineId = a.MachineId,
                        }).Distinct().ToList();
            return list;
        }

        //Delete:Material by id
        public bool Delete_DynamicFieldsData(string varplantId,string varMachineId)
        {
            bool status = false;
            var _dynamics = db.DynamicFieldNames.Where(x => x.PlantId == varplantId && x.MachineId == varMachineId);
            //DynamicFieldName  _dynamics = db.DynamicFieldNames.FirstOrDefault(x => x.PlantId  == varplantId && x.MachineId==varMachineId);
            if (_dynamics != null)
            {
                db.DynamicFieldNames.DeleteAllOnSubmit(_dynamics);
                //mat.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }
    }
}