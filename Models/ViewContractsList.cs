using System;
using System.Collections.Generic;

namespace JNGH_IDENDITY.Models
{
    public partial class ViewContractsList
    {
        public string CompId { get; set; }
        public string ShortName { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public long Id { get; set; }
        public string ContNo { get; set; }
        public string ContName { get; set; }
        public string Submitter { get; set; }
        public string MobilePhone { get; set; }
        public long? FileId { get; set; }
        public string ContFile { get; set; }
        public string FileUrl { get; set; }
    }
}
