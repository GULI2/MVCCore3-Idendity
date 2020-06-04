using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JNGH_IDENDITY.Models
{
    public class CreateEmployee
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

        [Display(Name = "角色")]
        public string[] RoleIds { get; set; }

        /// <summary>
        /// 是否是系统用户
        /// </summary>
        public bool IsUser { get; set; }

        [StringLength(100, ErrorMessage = "密码必须至少为8，并且最长为100个字符", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配")]
        public string ConfirmPassword { get; set; }

    }
}
