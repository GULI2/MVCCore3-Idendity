using System;
using System.Collections.Generic;

namespace JNGH_IDENDITY.Models
{
    public partial class ViewAuditedContractsList
    {
        public string ShortName { get; set; }
        public string DeptName { get; set; }
        public long Id { get; set; }
        public string ContNo { get; set; }
        public string ContName { get; set; }
        public string Submitter { get; set; }
        public string Creater { get; set; }
        public string AuditStatusText { get; set; }
        public string AuditMemo { get; set; }
        public string ContFile { get; set; }
        public string FileUrl { get; set; }
        public int? SubOrder { get; set; }
        public string CompId { get; set; }
        public string DeptId { get; set; }
        public string SubMemo { get; set; }
        public DateTime? SubTime { get; set; }
        public long? SubmitId { get; set; }
        public long? AuditId { get; set; }
        public int ContStatus { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Checker { get; set; }
        public DateTime? CheckTime { get; set; }
    }
}
