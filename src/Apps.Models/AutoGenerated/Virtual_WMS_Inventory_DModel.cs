//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using Apps.Models;
using System;
using System.ComponentModel.DataAnnotations;
namespace Apps.Models.WMS
{

	public partial class WMS_Inventory_DModel:Virtual_WMS_Inventory_DModel
	{
		
	}
	public class Virtual_WMS_Inventory_DModel
	{
		[Display(Name = "未设置")]
		public virtual int Id { get; set; }
		[Display(Name = "未设置")]
		public virtual int HeadId { get; set; }
		[Display(Name = "物料")]
		public virtual int PartId { get; set; }
		[Display(Name = "盘点数量")]
		public virtual decimal InventoryQty { get; set; }
		[Display(Name = "库存")]
		public virtual int InvId { get; set; }
		[Display(Name = "子库存")]
		public virtual Nullable<int> SubInvId { get; set; }
		[Display(Name = "批次号：YYYYMM")]
		public virtual string Lot { get; set; }
		[Display(Name = "备注")]
		public virtual string Remark { get; set; }
		[Display(Name = "未设置")]
		public virtual string Attr1 { get; set; }
		[Display(Name = "未设置")]
		public virtual string Attr2 { get; set; }
		[Display(Name = "未设置")]
		public virtual string Attr3 { get; set; }
		[Display(Name = "未设置")]
		public virtual string Attr4 { get; set; }
		[Display(Name = "未设置")]
		public virtual string Attr5 { get; set; }
		[Display(Name = "创建人")]
		public virtual string CreatePerson { get; set; }
		[Display(Name = "创建时间")]
		public virtual Nullable<System.DateTime> CreateTime { get; set; }
		[Display(Name = "修改人")]
		public virtual string ModifyPerson { get; set; }
		[Display(Name = "修改时间")]
		public virtual Nullable<System.DateTime> ModifyTime { get; set; }
		}
}
