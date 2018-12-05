//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Models;
using Apps.Common;
using Unity.Attributes;
using System.Transactions;
using Apps.BLL.Core;
using Apps.Locale;
using LinqToExcel;
using System.IO;
using System.Text;
using Apps.IDAL.WMS;
using Apps.Models.WMS;
using Apps.IBLL.WMS;
using System.Linq.Expressions;

namespace Apps.BLL.WMS
{
	public partial class WMS_POBLL: Virtual_WMS_POBLL,IWMS_POBLL
	{
        

	}
	public class Virtual_WMS_POBLL
	{
        [Dependency]
        public IWMS_PORepository m_Rep { get; set; }

        [Dependency]
        public IWMS_PartRepository m_PartRep { get; set; }
        [Dependency]
        public IWMS_SupplierRepository m_SupplierRep { get; set; }

        public virtual List<WMS_POModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<WMS_PO> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								
								a=>a.PO.Contains(queryStr)
								
								
								
								
								
								|| a.POType.Contains(queryStr)
								|| a.Status.Contains(queryStr)
								|| a.Remark.Contains(queryStr)
								|| a.Attr1.Contains(queryStr)
								|| a.Attr2.Contains(queryStr)
								|| a.Attr3.Contains(queryStr)
								|| a.Attr4.Contains(queryStr)
								|| a.Attr5.Contains(queryStr)
								|| a.CreatePerson.Contains(queryStr)
								
								|| a.ModifyPerson.Contains(queryStr)
								
								);
            }
            else
            {
                queryData = m_Rep.GetList();
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }

		public virtual List<WMS_POModel> GetListByUserId(ref GridPager pager, string userId,string queryStr)
		{
			return new List<WMS_POModel>();
		}
		
		public virtual List<WMS_POModel> GetListByParentId(ref GridPager pager, string queryStr,object parentId)
        {
			return new List<WMS_POModel>();
		}

        public virtual List<WMS_POModel> CreateModelList(ref IQueryable<WMS_PO> queryData)
        {

            List<WMS_POModel> modelList = (from r in queryData
                                              select new WMS_POModel
                                              {
													Id = r.Id,
													PO = r.PO,
													PODate = r.PODate,
													SupplierId = r.SupplierId,
													PartId = r.PartId,
													QTY = r.QTY,
													PlanDate = r.PlanDate,
													POType = r.POType,
													Status = r.Status,
													Remark = r.Remark,
													Attr1 = r.Attr1,
													Attr2 = r.Attr2,
													Attr3 = r.Attr3,
													Attr4 = r.Attr4,
													Attr5 = r.Attr5,
													CreatePerson = r.CreatePerson,
													CreateTime = r.CreateTime,
													ModifyPerson = r.ModifyPerson,
													ModifyTime = r.ModifyTime,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, WMS_POModel model)
        {
            try
            {
                WMS_PO entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new WMS_PO();
               				entity.Id = model.Id;
				entity.PO = model.PO;                
                entity.PODate = model.PODate;

                //把供应商简称转为ID
                var supplierShortName = model.SupplierShortName;
                Expression<Func<WMS_Supplier, bool>> exp_s = x => x.SupplierShortName == supplierShortName;
                var supplier = m_SupplierRep.GetSingleWhere(exp_s);
                //entity.SupplierId = model.SupplierId;
                if (supplier == null)
                {
                    throw new Exception("供应商简称不存在！");
                }
                else
                {
                    entity.SupplierId = supplier.Id;
                }               

                //把物料编码转为ID
                var partCode = model.PartCode;
                Expression<Func<WMS_Part, bool>> exp_p = x => x.PartCode == partCode;
                var part = m_PartRep.GetSingleWhere(exp_p);
                //entity.PartId = model.PartId;
                if (part == null)
                {
                    throw new Exception("物料编码不存在！");
                }
                else
                {
                    entity.PartId = part.Id;
                }      
                
                entity.QTY = model.QTY;
				entity.PlanDate = model.PlanDate;
				entity.POType = model.POType;
				entity.Status = model.Status;
				entity.Remark = model.Remark;
				entity.Attr1 = model.Attr1;
				entity.Attr2 = model.Attr2;
				entity.Attr3 = model.Attr3;
				entity.Attr4 = model.Attr4;
				entity.Attr5 = model.Attr5;
				entity.CreatePerson = model.CreatePerson;
				entity.CreateTime = model.CreateTime;
				entity.ModifyPerson = model.ModifyPerson;
				entity.ModifyTime = model.ModifyTime;
  

                if (m_Rep.Create(entity))
                {
                    return true;
                }
                else
                {
                    errors.Add(Resource.InsertFail);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }



         public virtual bool Delete(ref ValidationErrors errors, object id)
        {
            try
            {
                if (m_Rep.Delete(id) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

        public virtual bool Delete(ref ValidationErrors errors, object[] deleteCollection)
        {
            try
            {
                if (deleteCollection != null)
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        if (m_Rep.Delete(deleteCollection) == deleteCollection.Length)
                        {
                            transactionScope.Complete();
                            return true;
                        }
                        else
                        {
                            Transaction.Current.Rollback();
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

		
       

        public virtual bool Edit(ref ValidationErrors errors, WMS_POModel model)
        {
            try
            {
                WMS_PO entity = m_Rep.GetById(model.Id);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.Id = model.Id;
				entity.PO = model.PO;
				entity.PODate = model.PODate;
				entity.SupplierId = model.SupplierId;                
                //把供应商简称转为ID
                //var supplierShortName = model.SupplierShortName;
                //Expression<Func<WMS_Supplier, bool>> exp_s = x => x.SupplierShortName == supplierShortName;
                //var supplier = m_SupplierRep.GetSingleWhere(exp_s);
                ////entity.SupplierId = model.SupplierId;
                //if (supplier == null)
                //{
                //    throw new Exception("供应商简称不存在！");
                //}
                //else
                //{
                //    entity.SupplierId = supplier.Id;
                //}
                entity.PartId = model.PartId;
                //把物料编码转为ID
                //var partCode = model.PartCode;
                //Expression<Func<WMS_Part, bool>> exp_p = x => x.PartCode == partCode;
                //var part = m_PartRep.GetSingleWhere(exp_p);
                ////entity.PartId = model.PartId;
                //if (part == null)
                //{
                //    throw new Exception("物料编码不存在！");
                //}
                //else
                //{
                //    entity.PartId = part.Id;
                //}
                entity.QTY = model.QTY;
				entity.PlanDate = model.PlanDate;
				entity.POType = model.POType;
				entity.Status = model.Status;
				entity.Remark = model.Remark;
				entity.Attr1 = model.Attr1;
				entity.Attr2 = model.Attr2;
				entity.Attr3 = model.Attr3;
				entity.Attr4 = model.Attr4;
				entity.Attr5 = model.Attr5;
				entity.CreatePerson = model.CreatePerson;
				entity.CreateTime = model.CreateTime;
				entity.ModifyPerson = model.ModifyPerson;
				entity.ModifyTime = model.ModifyTime;
 


                if (m_Rep.Edit(entity))
                {
                    return true;
                }
                else
                {
                    errors.Add(Resource.NoDataChange);
                    return false;
                }

            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

      

        public virtual WMS_POModel GetById(object id)
        {
            if (IsExists(id))
            {
                WMS_PO entity = m_Rep.GetById(id);
                WMS_POModel model = new WMS_POModel();
                              				model.Id = entity.Id;
				model.PO = entity.PO;
				model.PODate = entity.PODate;
				model.SupplierId = entity.SupplierId;
				model.PartId = entity.PartId;
				model.QTY = entity.QTY;
				model.PlanDate = entity.PlanDate;
				model.POType = entity.POType;
				model.Status = entity.Status;
				model.Remark = entity.Remark;
				model.Attr1 = entity.Attr1;
				model.Attr2 = entity.Attr2;
				model.Attr3 = entity.Attr3;
				model.Attr4 = entity.Attr4;
				model.Attr5 = entity.Attr5;
				model.CreatePerson = entity.CreatePerson;
				model.CreateTime = entity.CreateTime;
				model.ModifyPerson = entity.ModifyPerson;
				model.ModifyTime = entity.ModifyTime;
 
                return model;
            }
            else
            {
                return null;
            }
        }


		 /// <summary>
        /// 校验Excel数据,这个方法一般用于重写校验逻辑
        /// </summary>
        public virtual bool CheckImportData(string fileName, List<WMS_POModel> list,ref ValidationErrors errors )
        {
          
            var targetFile = new FileInfo(fileName);

            if (!targetFile.Exists)
            {

                errors.Add("导入的数据文件不存在");
                return false;
            }

            var excelFile = new ExcelQueryFactory(fileName);

            //对应列头
			 				 excelFile.AddMapping<WMS_POModel>(x => x.PO, "采购订单");
				 excelFile.AddMapping<WMS_POModel>(x => x.PODate, "采购日期");
				 excelFile.AddMapping<WMS_POModel>(x => x.SupplierId, "供应商编码");
				 excelFile.AddMapping<WMS_POModel>(x => x.PartId, "物料编码");
				 excelFile.AddMapping<WMS_POModel>(x => x.QTY, "数量");
				 excelFile.AddMapping<WMS_POModel>(x => x.PlanDate, "计划到货日期");
				 excelFile.AddMapping<WMS_POModel>(x => x.POType, "采购订单类型");
				 excelFile.AddMapping<WMS_POModel>(x => x.Status, "状态");
				 excelFile.AddMapping<WMS_POModel>(x => x.Remark, "说明");
				 excelFile.AddMapping<WMS_POModel>(x => x.Attr1, "Attr1");
				 excelFile.AddMapping<WMS_POModel>(x => x.Attr2, "Attr2");
				 excelFile.AddMapping<WMS_POModel>(x => x.Attr3, "Attr3");
				 excelFile.AddMapping<WMS_POModel>(x => x.Attr4, "Attr4");
				 excelFile.AddMapping<WMS_POModel>(x => x.Attr5, "Attr5");
				 excelFile.AddMapping<WMS_POModel>(x => x.CreatePerson, "创建人");
				 excelFile.AddMapping<WMS_POModel>(x => x.CreateTime, "创建时间");
				 excelFile.AddMapping<WMS_POModel>(x => x.ModifyPerson, "修改人");
				 excelFile.AddMapping<WMS_POModel>(x => x.ModifyTime, "修改时间");
 
            //SheetName
            var excelContent = excelFile.Worksheet<WMS_POModel>(0);
            int rowIndex = 1;
            //检查数据正确性
            foreach (var row in excelContent)
            {
                var errorMessage = new StringBuilder();
                var entity = new WMS_POModel();
						 				  entity.Id = row.Id;
				  entity.PO = row.PO;
				  entity.PODate = row.PODate;
				  entity.SupplierId = row.SupplierId;
				  entity.PartId = row.PartId;
				  entity.QTY = row.QTY;
				  entity.PlanDate = row.PlanDate;
				  entity.POType = row.POType;
				  entity.Status = row.Status;
				  entity.Remark = row.Remark;
				  entity.Attr1 = row.Attr1;
				  entity.Attr2 = row.Attr2;
				  entity.Attr3 = row.Attr3;
				  entity.Attr4 = row.Attr4;
				  entity.Attr5 = row.Attr5;
				  entity.CreatePerson = row.CreatePerson;
				  entity.CreateTime = row.CreateTime;
				  entity.ModifyPerson = row.ModifyPerson;
				  entity.ModifyTime = row.ModifyTime;
 
                //=============================================================================
                if (errorMessage.Length > 0)
                {
                    errors.Add(string.Format(
                        "第 {0} 列发现错误：{1}{2}",
                        rowIndex,
                        errorMessage,
                        "<br/>"));
                }
                list.Add(entity);
                rowIndex += 1;
            }
            if (errors.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public virtual void SaveImportData(IEnumerable<WMS_POModel> list)
        {
            try
            {
                using (DBContainer db = new DBContainer())
                {
                    foreach (var model in list)
                    {
                        WMS_PO entity = new WMS_PO();
                       						entity.Id = 0;
						entity.PO = model.PO;
						entity.PODate = model.PODate;
						entity.SupplierId = model.SupplierId;
						entity.PartId = model.PartId;
						entity.QTY = model.QTY;
						entity.PlanDate = model.PlanDate;
						entity.POType = model.POType;
						entity.Status = model.Status;
						entity.Remark = model.Remark;
						entity.Attr1 = model.Attr1;
						entity.Attr2 = model.Attr2;
						entity.Attr3 = model.Attr3;
						entity.Attr4 = model.Attr4;
						entity.Attr5 = model.Attr5;
						entity.CreatePerson = model.CreatePerson;
						entity.CreateTime = ResultHelper.NowTime;
						entity.ModifyPerson = model.ModifyPerson;
						entity.ModifyTime = model.ModifyTime;
 
                        db.WMS_PO.Add(entity);
                    }
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }
		public virtual bool Check(ref ValidationErrors errors, object id,int flag)
        {
			return true;
		}

        public virtual bool IsExists(object id)
        {
            return m_Rep.IsExist(id);
        }
		
		public void Dispose()
        { 
            
        }

	}
}
