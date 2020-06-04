using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JNGH_IDENDITY.Models
{
    public partial class SysEmpPositions
    {
         public string EmpId { get; set; }
        public string CompId { get; set; }
        public string PostId { get; set; }
        public string DeptId { get; set; }
        public int? PostOrder { get; set; }
        public string Remarks { get; set; }

       

       
    }
}
