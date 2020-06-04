using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JNGH_IDENDITY.Models
{
    public partial class ViewPositions
    {
        public string CompId { get; set; }
        public string PostId { get; set; }
        public string DeptId { get; set; }
        
        [Display(Name = "职务等级")]
        public string PostLevel { get; set; }
        [Display(Name = "序号")]
        public int? PostOrder { get; set; }
        [Required(ErrorMessage = "职务不能为空！")]
        [Display(Name ="职务")]
        public string PostName { get; set; }
        [Display(Name = "所属部门")]
        public string DeptName { get; set; }

        [Display(Name = "门店")]
        public string FullName { get; set; }
 
    }
}
