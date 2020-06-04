using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace JNGH_IDENDITY.Models
{
    public partial class FwContracts_Upload
    {
        public string ContNo { get; set; }
        public string ContName { get; set; }
        public string CompId { get; set; }
        public string DeptId { get; set; }
        public string Submitter { get; set; }

        public IFormFile FormFile { get; set; }
       
        public string SubMemo { get; set; }

    }
}
