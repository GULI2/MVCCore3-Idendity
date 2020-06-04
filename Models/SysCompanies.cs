using System;
using System.Collections.Generic;

namespace JNGH_IDENDITY.Models
{
    public partial class SysCompanies
    {
        public string CompId { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string PostCode { get; set; }
        public string Remarks { get; set; }
        public string ParentId { get; set; }
        public byte[] CompInfo { get; set; }
        public string CompLevel { get; set; }
        public int? CompOrder { get; set; }
    }
}
