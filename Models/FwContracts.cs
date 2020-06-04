using System;
using System.Collections.Generic;

namespace JNGH_IDENDITY.Models
{
    public partial class FwContracts
    {
        public long Id { get; set; }
        public string ContNo { get; set; }
        public string ContName { get; set; }
        public string CompId { get; set; }
        public string DeptId { get; set; }
        public string Submitter { get; set; }
        public string Creater { get; set; }
        public DateTime? CreateTime { get; set; }
        public int ContStatus { get; set; }
        public int? SubOrder { get; set; }
        public long? SubmitId { get; set; }
        public long? AuditId { get; set; }
    }
}
