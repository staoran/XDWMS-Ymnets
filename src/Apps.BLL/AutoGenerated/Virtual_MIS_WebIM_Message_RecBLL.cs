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
using Apps.IDAL.MIS;
using Apps.Models.MIS;
using Apps.IBLL.MIS;
namespace Apps.BLL.MIS
{
	public partial class MIS_WebIM_Message_RecBLL: Virtual_MIS_WebIM_Message_RecBLL,IMIS_WebIM_Message_RecBLL
	{
        

	}
	public class Virtual_MIS_WebIM_Message_RecBLL
	{
        [Dependency]
        public IMIS_WebIM_Message_RecRepository m_Rep { get; set; }

		public virtual List<MIS_WebIM_Message_RecModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<MIS_WebIM_Message_Rec> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.MessageId.Contains(queryStr)
								|| a.receiver.Contains(queryStr)
								
								
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

		public virtual List<MIS_WebIM_Message_RecModel> GetListByUserId(ref GridPager pager, string userId,string queryStr)
		{
			return new List<MIS_WebIM_Message_RecModel>();
		}
		
		public virtual List<MIS_WebIM_Message_RecModel> GetListByParentId(ref GridPager pager, string queryStr,object parentId)
        {
			return new List<MIS_WebIM_Message_RecModel>();
		}

        public virtual List<MIS_WebIM_Message_RecModel> CreateModelList(ref IQueryable<MIS_WebIM_Message_Rec> queryData)
        {

            List<MIS_WebIM_Message_RecModel> modelList = (from r in queryData
                                              select new MIS_WebIM_Message_RecModel
                                              {
													MessageId = r.MessageId,
													receiver = r.receiver,
													State = r.State,
													RecDt = r.RecDt,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, MIS_WebIM_Message_RecModel model)
        {
            try
            {
                MIS_WebIM_Message_Rec entity = m_Rep.GetById(model.MessageId);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new MIS_WebIM_Message_Rec();
               				entity.MessageId = model.MessageId;
				entity.receiver = model.receiver;
				entity.State = model.State;
				entity.RecDt = model.RecDt;
  

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

		
       

        public virtual bool Edit(ref ValidationErrors errors, MIS_WebIM_Message_RecModel model)
        {
            try
            {
                MIS_WebIM_Message_Rec entity = m_Rep.GetById(model.MessageId);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.MessageId = model.MessageId;
				entity.receiver = model.receiver;
				entity.State = model.State;
				entity.RecDt = model.RecDt;
 


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

      

        public virtual MIS_WebIM_Message_RecModel GetById(object id)
        {
            if (IsExists(id))
            {
                MIS_WebIM_Message_Rec entity = m_Rep.GetById(id);
                MIS_WebIM_Message_RecModel model = new MIS_WebIM_Message_RecModel();
                              				model.MessageId = entity.MessageId;
				model.receiver = entity.receiver;
				model.State = entity.State;
				model.RecDt = entity.RecDt;
 
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
        public virtual bool CheckImportData(string fileName, List<MIS_WebIM_Message_RecModel> list,ref ValidationErrors errors )
        {
          
            var targetFile = new FileInfo(fileName);

            if (!targetFile.Exists)
            {

                errors.Add("导入的数据文件不存在");
                return false;
            }

            var excelFile = new ExcelQueryFactory(fileName);

            //对应列头
			 				 excelFile.AddMapping<MIS_WebIM_Message_RecModel>(x => x.MessageId, "MessageId");
				 excelFile.AddMapping<MIS_WebIM_Message_RecModel>(x => x.receiver, "receiver");
				 excelFile.AddMapping<MIS_WebIM_Message_RecModel>(x => x.State, "0未读，1已读");
				 excelFile.AddMapping<MIS_WebIM_Message_RecModel>(x => x.RecDt, "RecDt");
 
            //SheetName
            var excelContent = excelFile.Worksheet<MIS_WebIM_Message_RecModel>(0);
            int rowIndex = 1;
            //检查数据正确性
            foreach (var row in excelContent)
            {
                var errorMessage = new StringBuilder();
                var entity = new MIS_WebIM_Message_RecModel();
						 				  entity.MessageId = row.MessageId;
				  entity.receiver = row.receiver;
				  entity.State = row.State;
				  entity.RecDt = row.RecDt;
 
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
        public virtual void SaveImportData(IEnumerable<MIS_WebIM_Message_RecModel> list)
        {
            try
            {
                using (DBContainer db = new DBContainer())
                {
                    foreach (var model in list)
                    {
                        MIS_WebIM_Message_Rec entity = new MIS_WebIM_Message_Rec();
                       						entity.MessageId = model.MessageId;
						entity.receiver = model.receiver;
						entity.State = model.State;
						entity.RecDt = model.RecDt;
 
                        db.MIS_WebIM_Message_Rec.Add(entity);
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
