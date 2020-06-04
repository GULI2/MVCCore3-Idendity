using System;
using System.Collections.Generic;

namespace JNGH_IDENDITY.Models
{
    public partial class FwContractAudits
    {
        public long Id { get; set; }
        public long ContId { get; set; }
        public string AuditMemo { get; set; }
        public string Checker { get; set; }
        public DateTime? CheckTime { get; set; }
        public long? FileId { get; set; }
        public int AuditStatus { get; set; }
    }
}
