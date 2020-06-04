using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JNGH_IDENDITY.Models
{
    public partial class SysEmployees
    {
        public string EmpId { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public int? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string IdcardNo { get; set; }
        public string Telephone { get; set; }
        public string InsidePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string HomeAddr { get; set; }
        public string PostCode { get; set; }
        public string Wxcode { get; set; }
        public string Qqcode { get; set; }
        public string Remarks { get; set; }
        public int? OnFlag { get; set; }
        public int? AddrFlag { get; set; }
        public string AttenceNo { get; set; }
        
    }
}
