using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace JNGH_IDENDITY.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RoleModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleModel> _logger;

        public RoleModel(
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            ILogger<RoleModel> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "角色名")]
            public string RoleName { get; set; }
            
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
             ReturnUrl =  returnUrl;
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(Input.RoleName);

                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new Role.");

                    return LocalRedirect(returnUrl);
                    //}
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
