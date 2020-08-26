using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Avery_Weigh.Model;

namespace Avery_Weigh.Repository
{
    public class MaterialRepository
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public IEnumerable<Model_Materials> GetModelMaterials()
        {
            var data = (from t in db.tblMaterials
                        select new Model_Materials
                        {
                            MaterialCode = t.MaterialCode,
                            Name = t.MaterialDesc + "( " + t.MaterialCode + " )"
                        }).ToList();
            return data;
        }  

        //Get:MaterialMaster DataTable
        public DataTable GetMaterialDataTable()
        {
            IEnumerable<Model_Materials> Materials = (from material in db.tblMaterials
                                                      join packing in db.PackingMasters on material.PackingCodeId equals packing.PackingCode
                                                      join matclassification in db.MaterialClassifications on material.MaterialClassificationCodeId equals matclassification.MaterialClassificationCode
                                                      select new Model_Materials
                                                      {
                                                          Id = material.Id,
                                                          MaterialCode = material.MaterialCode,
                                                          MaterialDesc = material.MaterialDesc,                                                      
                                                          PackingCode = packing.PackingCode,                                                          
                                                          MaterialClassificationCode = matclassification.MaterialClassificationCode
                                                      }).OrderBy(x => x.Id).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr No");
            dt.Columns.Add("Material Code");
            dt.Columns.Add("Material Desc");
            dt.Columns.Add("Packing Code");          
            dt.Columns.Add("Material Classification Code");
            int index = 1;
            foreach (var item in Materials)
            {
                DataRow dr = dt.NewRow();
                dr["Sr No"] = index;
                dr["Material Code"] = item.MaterialCode;
                dr["Material Desc"] = item.MaterialDesc;
                dr["Packing Code"] = item.PackingCode;                      
                dr["Material Classification Code"] = item.MaterialClassificationCode;
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        //Get:Not In Use
        //public IEnumerable<Model_Materials> NOTINUSEGet_MaterialsList()
        //{
        //    IEnumerable<Model_Materials> _Materials = (from mat in db.tblMaterials
        //                                               join pack in db.PackingMasters on mat.PackingCodeId equals pack.PackingCode
        //                                               join matclassi in db.MaterialClassifications
        //                                               on mat.MaterialClassificationCodeId equals matclassi.MaterialClassificationCode
        //                                               where mat.IsDeleted == false
        //                                               select new Model_Materials
        //                                               {
        //                                                   Id = mat.Id,
        //                                                   MaterialCode = mat.MaterialCode,
        //                                                   MaterialDesc = mat.MaterialDesc,
        //                                                   PackingCode = pack.PackingCode,
        //                                                   MaterialClassificationCode = matclassi.MaterialClassificationCode
        //                                               }).ToList();
        //    return _Materials;
        //}

        //Get:Material By Material Code      

        public Model_Materials GetmaterialByCode(string code)
        {
            var data = (from t in db.tblMaterials
                        join p in db.PackingMasters on t.PackingCodeId equals p.PackingCode
                        join mc in db.MaterialClassifications on t.MaterialClassificationCodeId equals mc.MaterialClassificationCode
                        where t.MaterialCode == code && t.IsDeleted == false
                        select new { t.Id, t.MaterialCode, t.MaterialDesc, p.PackingCode, mc.MaterialClassificationCode }).FirstOrDefault();
            Model_Materials _mat = new Model_Materials();
            if (data != null)
            {
                _mat.Id = data.Id;
                _mat.MaterialCode = data.MaterialCode;
                _mat.MaterialDesc = data.MaterialDesc;
                _mat.PackingCode = data.PackingCode;
                _mat.MaterialClassificationCode = data.MaterialClassificationCode;
            }
            return _mat;
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
                        string _PackingCode = dr["Packing Code"].ToString();
                        string _Matclascode = dr["Material Classification Code"].ToString();
                        string _MaterialCode = dr["Material Code"].ToString();
                        string _MaterialDesc = dr["Material Desc"].ToString();
                        MaterialClassification mc = db.MaterialClassifications.FirstOrDefault(x => x.MaterialClassificationCode == _Matclascode && x.IsDeleted == false);
                        PackingMaster pm = db.PackingMasters.Where(x => x.PackingCode == _PackingCode && x.IsDeleted == false).FirstOrDefault();
                        var data = db.tblMaterials.Where(x => x.MaterialCode == _MaterialCode && x.IsDeleted == false).FirstOrDefault();
                        if (data != null)
                        {
                            if (string.IsNullOrEmpty(_PackingCode) && string.IsNullOrEmpty(_Matclascode) && string.IsNullOrEmpty(_MaterialDesc))
                            {
                                _failed++;
                            }
                            else
                            {
                                data.MaterialDesc = _MaterialDesc;
                                data.PackingCode = _PackingCode;
                                data.MaterialClassificationCodeId = _Matclascode;
                                db.SubmitChanges();
                                _update++;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_MaterialCode) && !string.IsNullOrEmpty(_MaterialDesc) && !string.IsNullOrEmpty(_PackingCode) && !string.IsNullOrEmpty(_Matclascode))
                            {
                                tblMaterial _tblmaterial = db.tblMaterials.Where(x => x.MaterialCode == _MaterialCode && x.IsDeleted == false).FirstOrDefault();
                                if (_tblmaterial != null)
                                {
                                    _failed++;
                                }
                                else
                                {
                                    db.tblMaterials.InsertOnSubmit(new tblMaterial
                                    {
                                        MaterialCode = _MaterialCode,
                                        MaterialDesc = _MaterialDesc,
                                        PackingCodeId = _PackingCode,
                                        MaterialClassificationCodeId = _Matclascode,
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

        //Delete:Material by id
        public bool Delete_Material(int id)
        {
            bool status = false;
            tblMaterial mat = db.tblMaterials.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (mat != null)
            {
                mat.IsDeleted = true;
                db.SubmitChanges();
                status = true;
            }
            return status;
        } 

        //Get: Material By Id
        public tblMaterial Get_MaterialById(int id)
        {
            tblMaterial mat = db.tblMaterials.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return mat;
        }

        //Get:Material Code
        public IEnumerable<Model_Materials> Get_MaterialCode()
        {
            IEnumerable<Model_Materials> model = (from t in db.tblMaterials
                                                  where t.IsDeleted == false
                                                  select new Model_Materials
                                                  {
                                                      MaterialCode = t.MaterialCode,
                                                      Name = t.MaterialDesc + "( " + t.MaterialCode + " )"
                                                  }).ToList();
            return model;
        }

        //Get:Materials List
        public IEnumerable<tblMaterial> Get_MaterialList()
        {
            IEnumerable<tblMaterial> list = db.tblMaterials.Where(x => x.IsDeleted == false).ToList();
            return list;
        } 

        //Get:Material List
        public IEnumerable<Model_Materials> Get_Model_MaterialList()
        {
            //var data = db.tblMaterials.Where(x => x.IsDeleted == false).ToList();

            //return data;

            IEnumerable<Model_Materials> list = (from material in db.tblMaterials
                                                 
                                                 where material.IsDeleted == false
                                                 select new Model_Materials
                                                 {
                                                     Id = material.Id,
                                                     MaterialCode = material.MaterialCode,
                                                     MaterialDesc = material.MaterialDesc,
                                                     PackingCode = material.PackingCodeId,
                                                    MaterialClassificationCode = material.MaterialClassificationCodeId

                                                 }).OrderBy(x => x.Id).ToList();

            //IEnumerable<Model_Materials> list = (from material in db.tblMaterials
            //                                     join packing in db.PackingMasters on material.PackingCodeId equals packing.PackingCode
            //                                     join materialclassification in db.MaterialClassifications
            //                                     on material.MaterialClassificationCodeId equals materialclassification.MaterialClassificationCode
            //                                     where material.IsDeleted == false
            //                                     select new Model_Materials
            //                                     {
            //                                        Id = material.Id,
            //                                        MaterialCode = material.MaterialCode,
            //                                        MaterialDesc = material.MaterialDesc,
            //                                        PackingCode = packing.PackingCode,
            //                                        MaterialClassificationCode = materialclassification.MaterialClassificationCode
            //                                     }).OrderBy(x => x.Id).ToList();
            return list;
        }

        //Get:Material By material code
        public tblMaterial Get_tblMaterialByCode(string code)
        {
            tblMaterial mat = db.tblMaterials.FirstOrDefault(x => x.MaterialCode == code && x.IsDeleted == false);
            return mat;
        }

        //Add:New Material
        public bool Add_Material(tblMaterial mat)
        {
            bool status = false;
            if (mat != null)
            {
                db.tblMaterials.InsertOnSubmit(mat);
                db.SubmitChanges();
                status = true;
            }
            return status;
        }
    }
}