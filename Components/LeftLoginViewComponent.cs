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
            if (!string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                var curuser = await _userManager.FindByNameAsync(User.Identity.Name);
                var role = await _userManager.GetRolesAsync(curuser);
               
                //added by gl
                ViewUser userView = _context.ViewUser.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

                ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                if (identity.FindFirst("EmpName") != null)
                {
                    identity.RemoveClaim(identity.FindFirst("EmpName"));
                }
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("EmpName", userView.EmpName, ClaimValueTypes.String, "RemoteClaims"));
                identity.AddClaims(claims);
                //end of add
               
                if (role.Count > 0)
                {
                    //ViewBag.RoleName = role[0].ToString();
                    for (int i=0;i<role.Count; i++)
                    {
                        if (i==(role.Count-1))
                        {
                            ViewBag.RoleName += role[i].ToString();
                        }
                        else
                        {
                            ViewBag.RoleName += role[i].ToString()+",";
                        }
                    }
                }
                else
                {
                    ViewBag.RoleName = "";
                }
            }
            else
            {
                ViewBag.RoleName = "";
            }

            return View();//此处没有返回ViewName 对应的视图文件是Default.cshtml
                          //return View("D", pageList);//此处返回的ViewName 是“D” 对应的视图文件是D.cshtml
        }
    }
}
