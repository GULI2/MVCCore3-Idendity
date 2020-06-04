using System;
using System.Collections.Generic;

namespace JNGH_IDENDITY.Models
{
    public partial class ViewAuditingContractsList
    {
        public long Id { get; set; }
        public string ContNo { get; set; }
        public string ContName { get; set; }
        public string DeptId { get; set; }
        public string Submitter { get; set; }
        public DateTime? CreateTime { get; set; }
        public int ContStatus { get; set; }
        public int? SubOrder { get; set; }
        public string ShortName { get; set; }
        public string CompId { get; set; }
        public string DeptName { get; set; }
        public int? AuditStatus { get; set; }
        public string AuditMemo { get; set; }
        public string ContStatusText { get; set; }
        public string AuditStatusText { get; set; }
        public string Checker { get; set; }
        public DateTime? CheckTime { get; set; }
        public string ContFile { get; set; }
        public string FileUrl { get; set; }
        public string SubMemo { get; set; }
        public DateTime? SubTime { get; set; }
        public long? FileId { get; set; }
    }
}
