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
    
    public partial class WMS_SubInvInfo
    {
        public int Id { get; set; }
        public string SubInvCode { get; set; }
        public string SubInvName { get; set; }
        public int InvId { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string CreatePerson { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
    
        public virtual WMS_InvInfo WMS_InvInfo { get; set; }
    }
}