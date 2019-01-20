﻿using Apps.Common;
using Apps.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System;
using System.IO;
using LinqToExcel;
using ClosedXML.Excel;
using Apps.Models.WMS;
using System.Linq.Expressions;

namespace Apps.BLL.WMS
{
    public partial class WMS_Inventory_DBLL
    {

        public override List<WMS_Inventory_DModel> GetListByParentId(ref GridPager pager, string queryStr, object parentId)
        {
            IQueryable<WMS_Inventory_D> queryData = null;
            int pid = Convert.ToInt32(parentId);
            if (pid != 0)
            {
                queryData = m_Rep.GetList(a => a.HeadId == pid);
            }
            else
            {
                queryData = m_Rep.GetList();
            }
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
                            a => (
                                    a.Remark.Contains(queryStr)
                                   || a.Attr1.Contains(queryStr)
                                   || a.Attr2.Contains(queryStr)
                                   || a.Attr3.Contains(queryStr)
                                   || a.Attr4.Contains(queryStr)
                                   || a.Attr5.Contains(queryStr)
                                   || a.CreatePerson.Contains(queryStr)
                                   || a.ModifyPerson.Contains(queryStr)
                                 )
                            );
            }
            pager.totalRows = queryData.Count();
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
        public override List<WMS_Inventory_DModel> CreateModelList(ref IQueryable<WMS_Inventory_D> queryData)
        {

            List<WMS_Inventory_DModel> modelList = (from r in queryData
                                                    select new WMS_Inventory_DModel
                                                    {
                                                        Attr1 = r.Attr1,
                                                        Attr2 = r.Attr2,
                                                        Attr3 = r.Attr3,
                                                        Attr4 = r.Attr4,
                                                        Attr5 = r.Attr5,
                                                        CreatePerson = r.CreatePerson,
                                                        CreateTime = r.CreateTime,
                                                        HeadId = r.HeadId,
                                                        Id = r.Id,
                                                        InventoryQty = r.InventoryQty,
                                                        InvId = r.InvId,
                                                        ModifyPerson = r.ModifyPerson,
                                                        ModifyTime = r.ModifyTime,
                                                        PartId = r.PartId,
                                                        Remark = r.Remark,
                                                        SubInvId = r.SubInvId,
                                                        Lot = r.Lot,
                                                        SnapshootQty = r.SnapshootQty,
                                                        Inventory_HName = r.WMS_Inventory_H.InventoryTitle,

                                                        PartCode = r.WMS_Part.PartCode,
                                                        PartName = r.WMS_Part.PartName,
                                                        InvCode = r.WMS_InvInfo.InvCode,
                                                        InvName = r.WMS_InvInfo.InvName,
                                                        SubInvName = r.WMS_SubInvInfo.SubInvName,
                                                    }).ToList();
            return modelList;
        }

        public bool ImportExcelData(string oper, string filePath, ref ValidationErrors errors)
        {
            bool rtn = true;

            var targetFile = new FileInfo(filePath);

            if (!targetFile.Exists)
            {
                errors.Add("导入的数据文件不存在");
                return false;
            }

            var excelFile = new ExcelQueryFactory(filePath);

            using (XLWorkbook wb = new XLWorkbook(filePath))
            {
                //第一个Sheet
                using (IXLWorksheet wws = wb.Worksheets.First())
                {
                    //对应列头
                    excelFile.AddMapping<WMS_Inventory_DModel>(x => x.Inventory_HName, "盘点名称");
                    excelFile.AddMapping<WMS_Inventory_DModel>(x => x.PartCode, "物料编码");
                    excelFile.AddMapping<WMS_Inventory_DModel>(x => x.InventoryQty, "盘点数量");
                    excelFile.AddMapping<WMS_Inventory_DModel>(x => x.InvName, "库房名称");
                    //excelFile.AddMapping<WMS_Inventory_DModel>(x => x.SubInvId, "子库存");
                    excelFile.AddMapping<WMS_Inventory_DModel>(x => x.Lot, "批次号");
                    excelFile.AddMapping<WMS_Inventory_DModel>(x => x.Remark, "备注");
                    //excelFile.AddMapping<WMS_Inventory_DModel>(x => x.Attr1, "");
                    //excelFile.AddMapping<WMS_Inventory_DModel>(x => x.Attr2, "");
                    //excelFile.AddMapping<WMS_Inventory_DModel>(x => x.Attr3, "");
                    //excelFile.AddMapping<WMS_Inventory_DModel>(x => x.Attr4, "");
                    //excelFile.AddMapping<WMS_Inventory_DModel>(x => x.Attr5, "");
                    //excelFile.AddMapping<WMS_Inventory_DModel>(x => x.CreatePerson, "创建人");
                    //excelFile.AddMapping<WMS_Inventory_DModel>(x => x.CreateTime, "创建时间");
                    //excelFile.AddMapping<WMS_Inventory_DModel>(x => x.ModifyPerson, "修改人");
                    //excelFile.AddMapping<WMS_Inventory_DModel>(x => x.ModifyTime, "修改时间");

                    //SheetName，第一个Sheet
                    var excelContent = excelFile.Worksheet<WMS_Inventory_DModel>(0);

                    //开启事务
                    using (DBContainer db = new DBContainer())
                    {
                        var tran = db.Database.BeginTransaction();  //开启事务
                        int rowIndex = 0;

                        //检查数据正确性
                        foreach (var row in excelContent)
                        {
                            rowIndex += 1;
                            string errorMessage = String.Empty;
                            var model = new WMS_Inventory_DModel();
                            model.Id = row.Id;
                            model.Inventory_HName = row.Inventory_HName;
                            model.PartCode = row.PartCode;
                            model.InventoryQty = row.InventoryQty;
                            model.InvName = row.InvName;
                            model.Lot = row.Lot;
                            //model.SubInvId = row.SubInvId;
                            model.Remark = row.Remark;
                            //model.Attr1 = row.Attr1;
                            //model.Attr2 = row.Attr2;
                            //model.Attr3 = row.Attr3;
                            //model.Attr4 = row.Attr4;
                            //model.Attr5 = row.Attr5;
                            //model.CreatePerson = row.CreatePerson;
                            //model.CreateTime = row.CreateTime;
                            //model.ModifyPerson = row.ModifyPerson;
                            //model.ModifyTime = row.ModifyTime;

                            if (!String.IsNullOrEmpty(errorMessage))
                            {
                                rtn = false;
                                errors.Add(string.Format("第 {0} 列发现错误：{1}{2}", rowIndex, errorMessage, "<br/>"));
                                wws.Cell(rowIndex + 1, excelFile.GetColumnNames("Sheet1").Count()).Value = errorMessage;
                                continue;
                            }

                            //执行额外的数据校验
                            try
                            {
                                AdditionalCheckExcelData(db, ref model);
                            }
                            catch (Exception ex)
                            {
                                rtn = false;
                                errorMessage = ex.Message;
                                errors.Add(string.Format("第 {0} 列发现错误：{1}{2}", rowIndex, errorMessage, "<br/>"));
                                wws.Cell(rowIndex + 1, excelFile.GetColumnNames("Sheet1").Count()).Value = errorMessage;
                                continue;
                            }

                            //写入数据库
                            Expression<Func<WMS_Inventory_D, bool>> exp = x => x.PartId == x.PartId && x.InvId == model.InvId && x.Lot == model.Lot;
                            WMS_Inventory_D entity;
                            entity = db.WMS_Inventory_D.FirstOrDefault(exp);
                            //WMS_Inventory_D entity1 = m_Rep.GetSingleWhere(model.Id);
                            if (entity != null)
                            {
                                entity.InventoryQty = model.InventoryQty;
                            }
                            else
                            {
                                entity = new WMS_Inventory_D();
                                entity.Id = model.Id;
                                entity.HeadId = model.HeadId;
                                entity.PartId = model.PartId;
                                entity.SnapshootQty = 0;
                                entity.InventoryQty = model.InventoryQty;
                                entity.Lot = model.Lot;
                                entity.InvId = model.InvId;
                                entity.SubInvId = model.SubInvId;
                                entity.Remark = model.Remark;
                                //entity.Attr1 = model.Attr1;
                                //entity.Attr2 = model.Attr2;
                                //entity.Attr3 = model.Attr3;
                                //entity.Attr4 = model.Attr4;
                                //entity.Attr5 = model.Attr5;
                                //entity.CreatePerson = model.CreatePerson;
                                //entity.CreateTime = model.CreateTime;
                                //entity.ModifyPerson = model.ModifyPerson;
                                //entity.ModifyTime = model.ModifyTime;
                                entity.CreatePerson = oper;
                                entity.CreateTime = DateTime.Now;
                                entity.ModifyPerson = oper;
                                entity.ModifyTime = DateTime.Now;
                                db.WMS_Inventory_D.Add(entity);
                            }                            
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                rtn = false;
                                //将当前报错的entity状态改为分离，类似EF的回滚（忽略之前的Add操作）
                                db.Entry(entity).State = System.Data.Entity.EntityState.Detached;
                                errorMessage = ex.InnerException.InnerException.Message;
                                errors.Add(string.Format("第 {0} 列发现错误：{1}{2}", rowIndex, errorMessage, "<br/>"));
                                wws.Cell(rowIndex + 1, excelFile.GetColumnNames("Sheet1").Count()).Value = errorMessage;
                            }
                        }

                        if (rtn)
                        {
                            tran.Commit();  //必须调用Commit()，不然数据不会保存
                        }
                        else
                        {
                            tran.Rollback();    //出错就回滚       
                        }
                    }
                }
                wb.Save();
            }

            return rtn;
        }

        public void AdditionalCheckExcelData(DBContainer db, ref WMS_Inventory_DModel model)
        {
            //获取数量
            if (model.InventoryQty< 0)
                {
                    throw new Exception("盘点数量不能为负！");
                }
            
            //获取总成物料ID
            if (!String.IsNullOrEmpty(model.PartCode))
            {
                var partCode = model.PartCode;
                Expression<Func<WMS_Part, bool>> exp = x => x.PartCode == partCode;

                //var part = m_PartRep.GetSingleWhere(exp);
                var part = db.WMS_Part.FirstOrDefault(exp);
                if (part == null)
                {
                    throw new Exception("物料编码不存在！");
                }
                else
                {
                    model.PartId = part.Id;
                }
            }
            else
            {
                throw new Exception("物料编码不能为空！");
            }

            //获取库房ID
            if (!String.IsNullOrEmpty(model.InvName))
            {
                var invName = model.InvName;
                Expression<Func<WMS_InvInfo, bool>> exp = x => x.InvName == invName;

                //var supplier = m_SupplierRep.GetSingleWhere(exp);
                var invInfo = db.WMS_InvInfo.FirstOrDefault(exp);
                if (invInfo == null)
                {
                    throw new Exception("库房不存在！");
                }
                else
                {
                    model.InvId = invInfo.Id;
                }
            }
            else
            {
                throw new Exception("库房不能为空！");
            }
            //获取盘点头ID
            if (!String.IsNullOrEmpty(model.Inventory_HName))
            {
                var inventory_HName = model.Inventory_HName;
                Expression<Func<WMS_Inventory_H, bool>> exp = x => x.InventoryTitle == inventory_HName;

                //var supplier = m_SupplierRep.GetSingleWhere(exp);
                var inventoryH = db.WMS_Inventory_H.FirstOrDefault(exp);
                if (inventoryH == null)
                {
                    throw new Exception("盘点名称不存在！");
                }
                else
                {
                    model.HeadId = inventoryH.Id;
                }
            }
            else
            {
                throw new Exception("盘点名称不能为空！");
            }
        }

        public List<WMS_Inventory_DModel> GetListByWhere(ref GridPager pager, string where)
        {
            IQueryable<WMS_Inventory_D> queryData = null;
            queryData = m_Rep.GetList().Where(where);
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
    }
}

