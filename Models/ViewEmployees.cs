using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JNGH_IDENDITY.Models
{
    public partial class ViewEmployees
    {
        public string EmpId { get; set; }

        [Required(ErrorMessage = "员工编号不能为空！")]
        [Display(Name = "员工编号")]
        public string EmpNo { get; set; }

        [Required(ErrorMessage = "员工姓名不能为空！")]
        [Display(Name = "员工姓名")]
        public string EmpName { get; set; }

        [Phone]
        [Display(Name = "手机")]
        public string MobilePhone { get; set; }

        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Display(Name = "职务")]
        public string PostName { get; set; }
        [Required(ErrorMessage = "请选择职务！")]
        public string PostId { get; set; }

        [Display(Name = "部门")]
        public string DeptName { get; set; }
        [Required(ErrorMessage = "请选择部门！")]
        public string DeptId { get; set; }


        [Display(Name = "所属公司")]
        public string ShortName { get; set; }

        public string CompId { get; set; }

       
        [Display(Name = "是否是系统用户")]
        public string IsUserName { get; set; }
        public bool IsUser { get; set; }

 
        [Display(Name = "角色")]
        public string RoleNames { get; set; }
        public string RoleIds { get; set; }

    }
}
