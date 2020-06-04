using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JNGH_IDENDITY.Models
{
    public class ResetPasswordModel
    {
         
        public string UserId { get; set; }
        [Display(Name = "账号")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}必须至少为{2}，并且最长为{1}个字符.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
