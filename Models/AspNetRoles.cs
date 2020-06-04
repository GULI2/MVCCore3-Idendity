using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JNGH_IDENDITY.Models
{
    public partial class AspNetRoles
    {
        public AspNetRoles()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaims>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
        }
        [Required(ErrorMessage = "角色编码不能为空！")]
        public string Id { get; set; }
        [Required(ErrorMessage = "角色名称不能为空！")]
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        public string RoleDescription { get; set; }//角色描述
        public string Creator { get; set; }//创建者
        public DateTime? CreateTime { get; set; }//创建时间
        public bool IsRemoved { get; set; }//是否删除

        public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}
