using System;
using System.Collections.Generic;

namespace JNGH_IDENDITY.Models
{
    public partial class TopNavBarInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public string PositionName { get; set; }

        public int ComplaintCount { get; set; }

        public string ComplaintTime { get; set; }
    }
}
