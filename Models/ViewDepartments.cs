using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JNGH_IDENDITY.Models
{
    public class ViewDepartments
    {
        [Display(Name = "部门编码")]
        [Required(ErrorMessage = "部门编码不能为空！")]
        public string DeptId { get; set; }
        public string ParentId { get; set; }
        [Display(Name = "部门名称")]
        [Required(ErrorMessage = "部门名称不能为空！")]
        public string DeptName { get; set; }
        [Display(Name = "部门等级")]
        public string DeptLevel { get; set; }
        [Display(Name = "部门序号")]
        public int? DeptOrder { get; set; }
                 
        public string CompId { get; set; }
        [Display(Name = "所属公司")]
        [Required(ErrorMessage = "所属公司不能为空！")]
        public string CompName { get; set; }
        public string Remarks { get; set; }
        public int? AddrFlag { get; set; }
    }
}
