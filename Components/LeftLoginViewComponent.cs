using JNGH_IDENDITY.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JNGH_IDENDITY.Components
{
    public class LeftLoginViewComponent : ViewComponent
    {
        private readonly ILogger<MenuViewComponent> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly YXCloudCenterContext _context;

        public LeftLoginViewComponent(ILogger<MenuViewComponent> logger, 
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signinManager
            , YXCloudCenterContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signinManager = signinManager;
            _context = context;
        }
      

        public async Task<IViewComponentResult> InvokeAsync()
        {
            TopNavBarInfo info = new TopNavBarInfo();
                if (_signinManager.IsSignedIn((System.Security.Claims.ClaimsPrincipal)User))
                {
                    var loginuser = await _context.ViewEmployees.FirstOrDefaultAsync(e => e.EmpId == User.Identity.Name);

                    if (loginuser == null)
                    {
                        info.UserId = User.Identity.Name;
                        info.UserName = User.Identity.Name;
                        info.DeptName = "";
                        info.PositionName = "";
                    }
                    else
                    {
                        info.UserId = loginuser.EmpId;
                        info.UserName = loginuser.EmpName;
                        info.DeptName = loginuser.DeptName;
                        info.PositionName = loginuser.PostName;
                    }
                }
                else
                {
                    info.UserId = "";
                    info.UserName = "";
                    info.DeptName = "";
                    info.PositionName = "";
                    info.ComplaintCount = 0;
                    info.ComplaintTime = "";
                }
             
                return View(info);//此处没有返回ViewName 对应的视图文件是Default.cshtml
                          //return View("D", pageList);//此处返回的ViewName 是“D” 对应的视图文件是D.cshtml
        }
    }
}
