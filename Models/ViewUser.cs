using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JNGH_IDENDITY.Models
{
    public class ViewUser
    {
        public string Id { get; set; }
        [Required]
        [Display(Name ="用户名")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}必须至少为{2}，并且最长为{1}个字符.", MinimumLength = 8)]
        [Display(Name = "密码")]
        [DataType(dataType: DataType.Password)]
        public string PasswordHash { get; set; }
 
        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        [Display(Name = "创建者")]
        public string Creator { get; set; }//创建者

        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }//创建时间

        public string RoleId { get; set; }

        [Display(Name = "角色")]
        public string RoleName { get; set; }

        [Display(Name = "状态")]
        public bool IsRemoved { get; set; }

        public string EmployNo { get; set; }
        public string EmpId { get; set; }

        [Display(Name = "员工")]
        public string EmpName { get; set; }

        public string DeptId { get; set; }

        [Display(Name = "部门")]
        public string DeptName { get; set; }

        public string PostId { get; set; }

        [Display(Name = "职务")]
        public string PostName { get; set; }

        public string CompId { get; set; }

        [Display(Name = "所属公司")]
        public string ShortName { get; set; }

        [Display(Name = "状态")]
        public string EnableTag { get; set; }
        
    }
}
