using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JNGH_IDENDITY.Models
{
    public partial class EditUser
    {
        public string UserId { get; set; }
        [Display(Name = "账号")]
        public string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请选择角色！")]
        [Display(Name = "角色")]
        public string[] RoleIds { get; set; }
 
        [Display(Name = "员工")]
        public string EmpName { get; set; }

        [Phone]
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }
        /*
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        */
    }
}
