//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Apps.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WMS_Inv_Adjust
    {
        public int Id { get; set; }
        public string InvAdjustBillNum { get; set; }
        public int PartId { get; set; }
        public decimal AdjustQty { get; set; }
        public string AdjustType { get; set; }
        public Nullable<int> InvId { get; set; }
        public Nullable<int> SubInvId { get; set; }
        public string Remark { get; set; }
        public string Attr1 { get; set; }
        public string Attr2 { get; set; }
        public string Attr3 { get; set; }
        public string Attr4 { get; set; }
        public string Attr5 { get; set; }
        public string CreatePerson { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
        public string Lot { get; set; }
    
        public virtual WMS_InvInfo WMS_InvInfo { get; set; }
        public virtual WMS_Part WMS_Part { get; set; }
        public virtual WMS_SubInvInfo WMS_SubInvInfo { get; set; }
    }
}
