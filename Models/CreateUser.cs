using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JNGH_IDENDITY.Models
{
    public class CreateUser
    {
        public string UserId { get; set; }
        [Required(ErrorMessage ="账号不能为空！")]
        [Display(Name = "账号")]
        public string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请选择角色！")]
        [Display(Name = "角色")]
        public string[] RoleIds { get; set; }

        [Required(ErrorMessage = "密码不能为空！")]
        [StringLength(100, ErrorMessage = "密码必须至少为8，并且最长为100个字符", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "请选择员工！")]
        [Display(Name = "员工")]
        public string EmpNo { get; set; }

        [Phone]
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        public string Creator { get; set; }//创建者

        public DateTime? CreateTime { get; set; }//创建时间
    }
}
