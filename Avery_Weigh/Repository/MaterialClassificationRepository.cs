using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{

    public class MaterialClassificationRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        //Get:MaterialClassification Code
        public IEnumerable<Model_MaterialClassification> GetMaterialClassifications_Code()
        {
            var data = (from t in db.MaterialClassifications
                        where t.IsDeleted == false
                        select new Model_MaterialClassification
                        {
                            Code = t.MaterialClassificationCode,
                            Name = t.MaterialClassificationDesc + " (" + t.MaterialClassificationCode + " )"
                        }).ToList();
            return data;
        }

        public IEnumerable<Model_Supplier> Get_Suppliers()
        {
            var data = (from t in db.tblSuppliers
                        where t.IsDeleted == false
                        select new Model_Supplier
                        {
                            Id = t.Code,
                            Name = t.Name + "( " + t.Code + " )"
                        }).ToList();
            return data;
        }

        //: Get MaterialClassification Datatable
        public DataTable Get_MaterialClassification_DataTable()
        {
            var data = (from t in db.MaterialClassifications
                        where t.IsDeleted == false
                        select new Model_MaterialClassification
                        {
                            MaterialClassificationCode = t.MaterialClassificationCode,
                            MaterialClassificationDesc = t.MaterialClassificationDesc,
                            Supplier_VendorCode = t.Supplier_VendorCode
                        }).ToList();

            DataTable dt = new DataTable();
            DataColumn srno = new DataColumn("Sr No");
            DataColumn column = new DataColumn("Material Classification Code");
            DataColumn _column = new DataColumn("Material Classification Des");
            DataColumn __column = new DataColumn("Supplier Vendor Code");
            dt.Columns.Add(srno);
            dt.Columns.Add(column);
            dt.Columns.Add(_column);
            dt.Columns.Add(__column);
            int index = 1;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["Material Classification Code"] = item.MaterialClassificationCode;
                dr["Material Classification Des"] = item.MaterialClassificationDesc;
                dr["Supplier Vendor Code"] = item.Supplier_VendorCode;
                dt.Rows.Add(dr);
                index++;

            }
            return dt;
        }  

        //Get:Material Classification List
        public IEnumerable<Model_MaterialClassification> GetModel_MaterialClassificationsList()
        {
            var data = (from t in db.MaterialClassifications
                        join s in db.tblSuppliers on t.Supplier_VendorCode equals s.Code
                        where t.IsDeleted == false
                        select new Model_MaterialClassification
                        {
                            Id = t.Id,
                            MaterialClassificationCode = t.MaterialClassificationCode,
                            MaterialClassificationDesc = t.MaterialClassificationDesc,
                            Supplier_VendorCode = s.Code
                        }).ToList();
            return data;
        }

        //Get:Material Classification By Code
        public Model_MaterialClassification Get_MaterialClassificationByCode(string code)
        {
            var data = (from t in db.MaterialClassifications
                        join s in db.tblSuppliers on t.Supplier_VendorCode equals s.Code
                        where t.MaterialClassificationCode == code && t.IsDeleted == false
                        select new { t.Id, t.MaterialClassificationCode, t.MaterialClassificationDesc, s.Code }).FirstOrDefault();
            Model_MaterialClassification _mat = new Model_MaterialClassification();

            if (data != null)
            {
                _mat.Id = data.Id;
                _mat.MaterialClassificationCode = data.MaterialClassificationCode;
                _mat.MaterialClassificationDesc = data.MaterialClassificationDesc;
                _mat.Supplier_VendorCode = data.Code;
            }
            return _mat;
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
                        string _MaterialClassificationCode = dr["Material Classification Code"].ToString();
                        string _MaterialClassificationDesc = dr["Material Classification Des"].ToString();
                        string _SupplierVendorCode = dr["Supplier Vendor Code"].ToString();
                        var _MaterialClassification = db.MaterialClassifications.Where(x => x.MaterialClassificationCode == _MaterialClassificationCode && x.IsDeleted == false).FirstOrDefault();
                        var supplier = db.tblSuppliers.FirstOrDefault(x => x.Code == _SupplierVendorCode && x.IsDeleted == false);
                        if (_MaterialClassification != null)
                        {
                            _MaterialClassification.MaterialClassificationDesc = _MaterialClassificationDesc;
                            _MaterialClassification.Supplier_VendorCode = _SupplierVendorCode;
                            _update++;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_MaterialClassificationCode) && !string.IsNullOrEmpty(_MaterialClassificationDesc))
                            {
                                MaterialClassification _mat = db.MaterialClassifications.Where(x => x.MaterialClassificationCode == _MaterialClassificationCode && x.IsDeleted == false).FirstOrDefault();
                                if (_mat != null)
                                {
                                    _failed++;
                                }
                                else
                                {
                                    if (supplier != null)
                                    {
                                        db.MaterialClassifications.InsertOnSubmit(new MaterialClassification
                                        {
                                            MaterialClassificationCode = _MaterialClassificationCode,
                                            MaterialClassificationDesc = _MaterialClassificationDesc,
                                            Supplier_VendorCode = _SupplierVendorCode,
                                            IsDeleted = false
                                        });
                                        db.SubmitChanges();
                                        _success++;
                                    }
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

        //Delete:Material Classification record by id
        public bool Delete_MaterialClassification(int id)
        {
            bool status = false;
            MaterialClassification mat = db.MaterialClassifications.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (mat != null)
            {
                mat.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        }

        //Get:Material Classification by Id
        public MaterialClassification Get_MaterialClassificationById(int id)
        {
            MaterialClassification mat = db.MaterialClassifications.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return mat;
        }

        //Get:Material Classification List
        public IEnumerable<MaterialClassification> Get_MaterialClassification_List()
        {
            IEnumerable<MaterialClassification> list = db.MaterialClassifications.Where(x => x.IsDeleted == false);
            return list;
        } 

        //Get:Material Classification By Code
        public MaterialClassification Get_Material_ClassificationByCode(string code)
        {
            MaterialClassification mat = db.MaterialClassifications.FirstOrDefault(x => x.MaterialClassificationCode == code && x.IsDeleted == false);
            return mat;
        }   

        //Add:New Material Classification
        public bool Add_MaterialClassification(MaterialClassification mat)
        {
            bool status = false;
            if (mat != null)
            {
                db.MaterialClassifications.InsertOnSubmit(mat);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }
       
    }
}
