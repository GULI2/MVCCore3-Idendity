using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JNGH_IDENDITY.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ILogger<MenuViewComponent> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public MenuViewComponent(ILogger<MenuViewComponent> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var curuser = await _userManager.FindByNameAsync(User.Identity.Name);
            bool ret = await _userManager.IsInRoleAsync(curuser, "Admin") ;
            bool ret1 = await _userManager.IsInRoleAsync(curuser, "法务审核");

            int priv = 0;
            if (ret)
            {
                priv = 2;
            }

            if (ret1)
            {
                priv = 1;
            }
            //List<tbl_page> pageList = new List<tbl_page>();
            //for (int i = 0; i < 10; i++)
            //{
            //    pageList.Add(new tbl_page()
            //    {
            //        page_no = i.ToString(),
            //        page_name = i.ToString()
            //    });
            //}
            return View(priv);//此处没有返回ViewName 对应的视图文件是Default.cshtml
                                  //return View("D", pageList);//此处返回的ViewName 是“D” 对应的视图文件是D.cshtml
        }
    }
}
