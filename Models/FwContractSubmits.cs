using System;
using System.Collections.Generic;

namespace JNGH_IDENDITY.Models
{
    public partial class FwContractSubmits
    {
        public long Id { get; set; }
        public long ContId { get; set; }
        public string SubMemo { get; set; }
        public string Submiter { get; set; }
        public DateTime? SubTime { get; set; }
        public long? FileId { get; set; }
        public int SubStatus { get; set; }
    }
}
