using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JNGH_IDENDITY.Models
{
    public partial class SysPositions
    {
        public string PostId { get; set; }
        public string ParentId { get; set; }
        public string PostLevel { get; set; }
        public int? PostOrder { get; set; }       
        public string PostName { get; set; }
        public string Remarks { get; set; }

        public string CompId { get; set; }

        public string DeptId { get; set; }
    }
}
