using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JNGH_IDENDITY.Models
{
    public partial class SysDepartments
    {
        [Required(ErrorMessage = "部门编码不能为空！"), StringLength(10)]
        public string DeptId { get; set; }
        public string ParentId { get; set; }
        [Required(ErrorMessage = "部门名称不能为空！"), StringLength(20)]
        public string DeptName { get; set; }
        public string DeptLevel { get; set; }
        public int? DeptOrder { get; set; }
        public string Remarks { get; set; }
        public int? AddrFlag { get; set; }
        [ForeignKey(nameof(SysCompanies))]
        public string CompId { get; set; }
        public SysCompanies SysCompanies { get; set; }
    }
}
