using JNGH_IDENDITY.Areas.Identity.Pages.Account;
using JNGH_IDENDITY.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;

namespace JNGH_IDENDITY.Controllers
{
    [Authorize]
    public class AuthorityController : Controller
    {
        #region 初始化
        private readonly YXCloudCenterContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public AuthorityController(YXCloudCenterContext context, IWebHostEnvironment webHostEnvironment,
             UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #endregion

        #region 用户管理
        [HttpGet]
        public async Task<IActionResult> UserList(string EmpName, string PostId, string DeptId, 
            string IsEnable,int pageIndex=1,int pageSize=10)
        {
            SelectLists();

            List<SelectListItem> selList = new List<SelectListItem> {
             new SelectListItem { Text = "启用", Value = "启用"},
             new SelectListItem { Text = "禁用", Value = "禁用" } };
            ViewBag.IsEnableList = selList.AsEnumerable();

            List<ViewUser> list =   _context.ViewUser.OrderByDescending(u => u.CreateTime)
                .ToPagedList(pageIndex , pageSize);
            int dataCount =await  _context.ViewUser.CountAsync();
            if (!string.IsNullOrEmpty(EmpName))
            {
                list = list.Where(u => u.EmpName.Contains(EmpName)).ToPagedList(pageIndex , pageSize);
                dataCount = list.Where(u => u.EmpName.Contains(EmpName)).Count();
            }
            if (!string.IsNullOrEmpty(PostId))
            {
                list = list.Where(u => u.PostId == PostId).ToPagedList(pageIndex , pageSize);
                 dataCount = list.Where(u => u.PostId == PostId).Count();
            }
            if (!string.IsNullOrEmpty(DeptId))
            {
                list = list.Where(u => u.DeptId == DeptId).ToPagedList(pageIndex , pageSize);
                dataCount = list.Where(u => u.DeptId == DeptId).Count();
            }
            if (!string.IsNullOrEmpty(IsEnable))
            {
                list = list.Where(u => u.EnableTag == IsEnable).ToPagedList(pageIndex - 1, pageSize);
                dataCount = list.Where(u => u.EnableTag == IsEnable).Count();
            }
            return View(new PagedList<ViewUser>(list, 
                pageIndex, pageSize, dataCount) );
        }


        [HttpGet]
        public ActionResult CreateUser()
        {
            SelectList selEmpList = new SelectList(_context.SysEmployees.ToList(), "EmpNo", "EmpName");
            ViewBag.EmpList = selEmpList.AsEnumerable();
            SelectList selRoleList = new SelectList(_context.AspNetRoles.Where(u => !u.IsRemoved).Select(u => new
            {
                RoleId = u.Id,
                RoleName = u.Name
            }).ToList(), "RoleId", "RoleName");
            ViewBag.RoleList = selRoleList.AsEnumerable();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUser model)
        {
            SelectList selEmpList = new SelectList(_context.SysEmployees.ToList(), "EmpNo", "EmpName");
            ViewBag.EmpList = selEmpList.AsEnumerable();
            SelectList selRoleList = new SelectList(_context.AspNetRoles.Where(u => !u.IsRemoved).ToList(), "Id", "Name");
            ViewBag.RoleList = selRoleList.AsEnumerable();
            if (_context.AspNetUsers.Any(u => u.EmployNo == model.EmpNo))
            {
                ModelState.AddModelError("EmployNoUserExist", "该员工已存在账号！");
            }
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //写入员工编码、创建人、创建时间
                    var AspNetUsers = _context.AspNetUsers.Where(u => u.Id == user.Id)
                         .FirstOrDefault();
                    AspNetUsers.EmployNo = model.EmpNo;
                    AspNetUsers.Creator = User.Identity.Name;
                    AspNetUsers.CreateTime = DateTime.Now;

                    _context.Entry(AspNetUsers).State = EntityState.Modified;
                    //角色
                    foreach (var eachRole in model.RoleIds)
                    {
                        AspNetUserRoles userRole = new AspNetUserRoles()
                        {
                            RoleId = eachRole,
                            UserId = user.Id
                        };
                        _context.AspNetUserRoles.Add(userRole);
                    }
                    await _context.SaveChangesAsync();

                    // 添加UserClaim
                    IList<Claim> oldClaims = await _userManager.GetClaimsAsync(user);
                    if (oldClaims != null)
                    {
                        await _userManager.RemoveClaimsAsync(user, oldClaims);
                    }
                    ViewUser userView = _context.ViewUser.Where(u => u.Id == user.Id).FirstOrDefault();
                    ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("EmployNo", userView.EmployNo, ClaimValueTypes.String, "RemoteClaims"));
                    claims.Add(new Claim("EmpName", userView.EmpName, ClaimValueTypes.String, "RemoteClaims"));
                    claims.Add(new Claim("PostId", userView.PostId, ClaimValueTypes.String, "RemoteClaims"));
                    claims.Add(new Claim("DeptId", userView.DeptId, ClaimValueTypes.String, "RemoteClaims"));
                    claims.Add(new Claim("CompId", userView.CompId, ClaimValueTypes.String, "RemoteClaims"));
                    claims.Add(new Claim("PhoneNumber", userView.PhoneNumber, ClaimValueTypes.String, "RemoteClaims"));
                    claims.Add(new Claim("Email", userView.Email, ClaimValueTypes.String, "RemoteClaims"));
                    await _userManager.AddClaimsAsync(user, claims);

                    return RedirectToAction("UserList");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult EditUser(string id)
        {
            var list = _context.ViewUser.Where(u => u.Id == id)
               .FirstOrDefault();

            if (list == null)
            {
                return NotFound();
            }
            var aspNetRoles = _context.AspNetRoles.Where(u => !u.IsRemoved).Select(u => new
            {
                RoleId = u.Id,
                RoleName = u.Name
            }).ToList();

            List<SelectListItem> selRoleList = new List<SelectListItem>();

            foreach (var each in aspNetRoles)
            {
                if (Array.IndexOf(list.RoleId.Split(','), each.RoleId) >= 0)
                {
                    selRoleList.Add(new SelectListItem { Text = each.RoleName, Value = each.RoleId, Selected = true });
                }
                else
                {
                    selRoleList.Add(new SelectListItem { Text = each.RoleName, Value = each.RoleId, Selected = false });
                }
            }

            ViewBag.RoleList = selRoleList.AsEnumerable();
            return View(new EditUser()
            {
                UserId = id,
                UserName = list.UserName,
                PhoneNumber = list.PhoneNumber,
                Email = list.Email,
                EmpName = list.EmpName
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUser model)
        {
            SelectList selRoleList = new SelectList(_context.AspNetRoles.Where(u => !u.IsRemoved).Select(u => new
            {
                RoleId = u.Id,
                RoleName = u.Name
            }).ToList(), "RoleId", "RoleName");
            ViewBag.RoleList = selRoleList.AsEnumerable();
            if (ModelState.IsValid)
            {
                AspNetUsers user = _context.AspNetUsers.Where(u => u.Id == model.UserId).FirstOrDefault();

                if (user == null)
                {
                    return NotFound();
                }
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                _context.Entry(user).State = EntityState.Modified;
                //修改角色
                List<AspNetUserRoles> userRole = _context.AspNetUserRoles.Where(u => u.UserId == model.UserId).ToList();
                if (userRole != null)
                {
                    foreach (var each in userRole)
                    {
                        _context.Entry(each).State = EntityState.Deleted;
                    }
                    await _context.SaveChangesAsync();
                }
                foreach (var eachRole in model.RoleIds)
                {
                    AspNetUserRoles userRoleAdd = new AspNetUserRoles()
                    {
                        UserId = model.UserId,
                        RoleId = eachRole
                    };
                    _context.Add(userRoleAdd);
                }
                //修改AspNetUserClaim里的电话和邮箱
                AspNetUserClaims userEmailClaims =
                   _context.AspNetUserClaims.Where(u => u.UserId == model.UserId &&
                    u.ClaimType == "Email").FirstOrDefault();
                userEmailClaims.ClaimValue = model.Email;
                AspNetUserClaims userPhoneNumberClaims =
                  _context.AspNetUserClaims.Where(u => u.UserId == model.UserId &&
                 u.ClaimType == "PhoneNumber").FirstOrDefault();
                userPhoneNumberClaims.ClaimValue = model.PhoneNumber;
                _context.Entry(userEmailClaims).State = EntityState.Modified;
                _context.Entry(userPhoneNumberClaims).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return RedirectToAction("UserList");
            }
            else
            {
                return View(model);
            }
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            await DeleteUserAsyncDLL(id);
            return RedirectToAction("UserList");
        }

        public async Task DeleteUserAsyncDLL(string id)
        {
            var AspNetUsers = _context.AspNetUsers.FirstOrDefault(u => u.Id == id);
            List<AspNetUserRoles> aspNetUserRoles = _context.AspNetUserRoles.Where(u => u.UserId == id).ToList();
            if (AspNetUsers != null)
            {
                _context.AspNetUsers.Attach(AspNetUsers);
                _context.AspNetUsers.Remove(AspNetUsers);
                //删除声明
                IdentityUser user = new IdentityUser()
                {
                    UserName = AspNetUsers.UserName,
                    Email = AspNetUsers.Email,
                    PhoneNumber = AspNetUsers.PhoneNumber,
                    EmailConfirmed = true
                };
                IList<Claim> oldClaims = await _userManager.GetClaimsAsync(user);
                if (oldClaims != null)
                {
                    await _userManager.RemoveClaimsAsync(user, oldClaims);
                }
            }
            //删除用户角色关系
            if (aspNetUserRoles != null)
            {
                foreach (AspNetUserRoles each in aspNetUserRoles)
                {
                    _context.AspNetUserRoles.Attach(each);
                    _context.AspNetUserRoles.Remove(each);
                }
            }
            await _context.SaveChangesAsync();


        }
        /*
        [HttpGet]
        public ActionResult ResetPassword(string id, string userName)
        {
            ViewBag.userId = id;
            ViewBag.userName = userName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(Models.ResetPasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("UserList");
            }
            else
            {
                return View(model);
            }
        }*/

        //账号禁用/启用
        public async Task<IActionResult> ChangeLoginTag(string UserId)
        {
            AspNetUsers user = _context.AspNetUsers.Where(u => u.Id == UserId).FirstOrDefault();

            if (user != null)
            {
                if (user.LockoutEnd == null)
                {
                    ViewBag.text = "禁用";
                    user.LockoutEnd = DateTime.MaxValue;
                }
                else
                {
                    ViewBag.text = "启用";
                    user.LockoutEnd = null;
                }
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("UserList");
        }
        #endregion

        #region 角色管理
        [HttpGet]
        public async Task<IActionResult> RoleList(string roleName,int pageIndex = 1,int pageSize=10)
        {
            List<AspNetRoles> list = await _context.AspNetRoles.Where(u => !u.IsRemoved)
                .OrderByDescending(u => u.CreateTime)
                .ThenBy(u=>u.Id)
                .ToPagedListAsync(pageIndex, pageSize);
            int dataCount = _context.AspNetRoles.Where(u => !u.IsRemoved).Count();
            if (!string.IsNullOrEmpty(roleName))
            {
                list = list.Where(u => u.Name.ToLower().Contains(roleName.ToLower())).ToPagedList(pageIndex, pageSize);
                dataCount = list.Where(u => u.Name.ToLower().Contains(roleName.ToLower())).Count();
            }
            return View(new PagedList<AspNetRoles>(list,pageIndex,pageSize,dataCount));
        }

        [HttpGet]
        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(AspNetRoles model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.AspNetRoles.AnyAsync(u => u.Id == model.Id))
                {
                    ModelState.AddModelError("RoleIdIsExist", "该编码已存在，请重新设置！");
                    return View(model);
                }
                AspNetRoles role = new AspNetRoles()
                {
                    Creator = User.Identity.Name,
                    CreateTime = DateTime.Now,
                    Id = model.Id,
                    Name = model.Name,
                    RoleDescription = model.RoleDescription
                };
                _context.Add(role);

                await _context.SaveChangesAsync();
                return RedirectToAction("RoleList");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult EditRole(string id)
        {
            AspNetRoles role = _context.AspNetRoles.Where(u => u.Id == id).FirstOrDefault();
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(AspNetRoles model)
        {
            if (ModelState.IsValid)
            {
                AspNetRoles role = await _context.AspNetRoles.Where(u => u.Id == model.Id).FirstOrDefaultAsync();
                if (role == null)
                {
                    return NotFound();
                }
                role.RoleDescription = model.RoleDescription;
                if (role != null)
                {
                    role.RoleDescription = model.RoleDescription;
                    _context.Entry(role).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("RoleList");
            }
            else
            {
                return View(model);
            }
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            if (_context.AspNetUserRoles.Any(u => u.RoleId == id))
            {
                return Content("该角色已有对应账户，无法删除！");
                //return RedirectToAction("RoleList", new { msg = "该角色已有对应账户，无法删除！" });
            }
            else
            {
                AspNetRoles role = await _context.AspNetRoles.Where(u => u.Id == id).FirstOrDefaultAsync();
                if (role != null)
                {
                    _context.Entry(role).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("RoleList");
            }

        }
        #endregion

        #region 部门管理
        [HttpGet]
        public async Task<IActionResult> DeptList(string SearchDeptString
            ,int pageIndex=1,int pageSize=10)
        {
            List<ViewDepartments> list = await _context.SysDepartments
                .Include(u => u.SysCompanies)
               .OrderBy(u => u.DeptId)
                .ThenBy(u => u.DeptOrder)
                .Select(u => new ViewDepartments()
                {
                    DeptId = u.DeptId,
                    DeptName = u.DeptName,
                    CompId = u.CompId,
                    CompName = u.SysCompanies.FullName,
                    DeptLevel = u.DeptLevel,
                    DeptOrder = u.DeptOrder
                })
                .ToPagedListAsync(pageIndex, pageSize);
            int dataCount = _context.SysDepartments.Count();
            if (!string.IsNullOrEmpty(SearchDeptString))
            {
                list = list.Where(u => u.DeptName.Contains(SearchDeptString)).ToPagedList(pageIndex, pageSize);
                dataCount = list.Where(u => u.DeptName.Contains(SearchDeptString)).Count();
            }
            return View(new PagedList<ViewDepartments>(list, pageIndex, pageSize, dataCount) );
        }

        [HttpGet]
        public ActionResult CreateDept()
        {
            SelectList selList = new SelectList(_context.SysCompanies.ToList(), "CompId", "FullName");
            ViewBag.CompList = selList.AsEnumerable();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDept(SysDepartments model)
        {
            if (ModelState.IsValid)
            {
                if (!CheckDeptId(model))
                {
                    ModelState.AddModelError("deptIdExist", "该部门编码已存在，请重新输入！");
                }
                else
                {
                    SysDepartments dept = new SysDepartments()
                    {
                        DeptId = model.DeptId,
                        CompId = model.CompId,
                        DeptName = model.DeptName,
                        DeptOrder = model.DeptOrder,
                        DeptLevel = model.DeptLevel,
                        AddrFlag = 1,
                        Remarks = "",
                        ParentId = "-"
                    };
                    _context.Add(dept);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("DeptList");
                }
            }
            SelectList selList = new SelectList(_context.SysCompanies.ToList(), "CompId", "FullName");
            ViewBag.CompList = selList.AsEnumerable();
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> EditDept(string deptid, string compid)
        {
            var data = await _context.SysDepartments.Where(u => u.DeptId == deptid && u.CompId == compid).FirstAsync();
            if (data == null)
            {
                return NotFound();
            }
            SelectList selList = new SelectList(_context.SysCompanies.ToList(), "CompId", "FullName");
            ViewBag.CompList = selList.AsEnumerable();
            ViewBag.CompId = data.CompId;

            return View(new ViewDepartments()
            {
                DeptId = data.DeptId,
                DeptName = data.DeptName,
                DeptLevel = data.DeptLevel,
                DeptOrder = data.DeptOrder,
                AddrFlag = 1,
                Remarks = "",
                ParentId = "-"
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditDept(SysDepartments model)
        {
            if (ModelState.IsValid)
            {

                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return RedirectToAction("DeptList");
            }

            return View(model);

        }

        public ActionResult DeleteDept(string deptid, string compid)
        {
            if (_context.SysEmpPositions.Any(u => u.DeptId == deptid))
            {
                return Content("该部门已有对应职务或员工，无法删除！");
            }
            var sysDepartments = _context.SysDepartments.FirstOrDefault(u => u.DeptId == deptid && u.CompId == compid);
            if (sysDepartments == null)
            {
                return NotFound();
            }
            var sysPositionList = _context.SysPositions.Where(u => u.DeptId == deptid && u.CompId == compid);
            _context.SysDepartments.Attach(sysDepartments);
            _context.SysDepartments.Remove(sysDepartments);
            if (sysPositionList != null && sysPositionList.Any())
            {
                foreach (SysPositions each in sysPositionList)
                {
                    _context.SysPositions.Attach(each);
                    _context.SysPositions.Remove(each);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("DeptList");
        }
        #endregion

        #region  职务详情
        public async Task<IActionResult> SetPosition(string deptid, string compid,
            string deptName, string compName)
        {
            ViewBag.deptid = deptid;
            ViewBag.compid = compid;
            ViewBag.deptName = deptName;
            ViewBag.compName = compName;
            return View(await _context.SysPositions
                .Where(u => u.DeptId == deptid && u.CompId == compid)
                .OrderBy(u => u.PostOrder)
                .Select(u => new ViewPositions()
                {
                    PostId = u.PostId,
                    PostLevel = u.PostLevel,
                    PostName = u.PostName,
                    CompId = u.CompId,
                    FullName = compName,
                    DeptId = u.DeptId,
                    DeptName = deptName
                })
                .ToListAsync());
        }

        [HttpGet]
        public ActionResult CreatePosition(string deptid, string compid, string deptName, string compName)
        {
            return View(new ViewPositions()
            {
                DeptId = deptid,
                CompId = compid,
                DeptName = deptName,
                FullName = compName
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePosition(ViewPositions model)
        {

            if (ModelState.IsValid)
            {
                SysPositions sysPositions = new SysPositions()
                {
                    PostId = Guid.NewGuid().ToString(),
                    DeptId = model.DeptId,
                    CompId = model.CompId,
                    PostName = model.PostName,
                    PostLevel = model.PostLevel
                };
                _context.Add(sysPositions);
                await _context.SaveChangesAsync();
                return RedirectToAction("DeptList");
            }

            return View(model);
        }

        public ActionResult DeletePosition(string postId, string deptId, string CompId)
        {
            if (_context.SysEmpPositions.Any(u => u.PostId == postId))
            {
                return Content("该职务已有对应员工，无法删除！");
            }
            else
            {
                SysPositions positions = _context.SysPositions.Where(u => u.PostId == postId && u.DeptId == deptId && u.CompId == CompId).FirstOrDefault();
                if (positions == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Entry(positions).State = EntityState.Deleted;
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("DeptList");
        }
        #endregion

        #region 员工管理
        [HttpGet]
        public async Task<IActionResult> EmployeeList(string searchEmpName, string DeptId, 
            string PostId, string IsUser,int pageIndex=1,int pageSize=10)
        {
            SelectLists();
            List<SelectListItem> IsUserlist = new List<SelectListItem> {
             new SelectListItem { Text = "是", Value = "T"},
             new SelectListItem { Text = "否", Value = "F" } };
            ViewBag.IsUser = IsUserlist.AsEnumerable();
            List<ViewEmployees> list = await _context.ViewEmployees
                .OrderByDescending(u => u.EmpId)
                .ToPagedListAsync(pageIndex, pageSize);
            int dataCount = _context.ViewEmployees.Count();
            if (!string.IsNullOrEmpty(searchEmpName))
            {
                list = list.Where(u => u.EmpName.Contains(searchEmpName)).ToPagedList(pageIndex, pageSize);
                dataCount = list.Where(u => u.EmpName.Contains(searchEmpName)).Count();
            }
            if (!string.IsNullOrEmpty(DeptId))
            {
                list = list.Where(u => u.DeptId == DeptId).ToPagedList(pageIndex, pageSize);
                dataCount = list.Where(u => u.DeptId == DeptId).Count();
            }
            if (!string.IsNullOrEmpty(PostId))
            {
                list = list.Where(u => u.PostId == PostId).ToPagedList(pageIndex, pageSize);
                dataCount = list.Where(u => u.PostId == PostId).Count();
            }
            if (!string.IsNullOrEmpty(IsUser))
            {
                list = list.Where(u => IsUser == "T" ? u.IsUser : !u.IsUser).ToPagedList(pageIndex, pageSize);
                dataCount = list.Where(u => IsUser == "T" ? u.IsUser : !u.IsUser).Count();
            }
            return View(new PagedList<ViewEmployees>(list,pageIndex,pageSize,dataCount));
        }

        [HttpGet]
        public ActionResult CreateEmployee()
        {
            SelectLists();
            SelectRoleList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(CreateEmployee model)
        {
            SelectLists();
            SelectRoleList();
            if (ModelState.IsValid)
            {
                if (EmployeeIsExist(model.EmpNo))
                {
                    ModelState.AddModelError("empExist", "该员工编码已存在,请重新设置编码!");
                }
                else
                {
                    SysEmpPositions empPosition = new SysEmpPositions()
                    {
                        EmpId = model.EmpNo,
                        PostId = model.PostId,
                        DeptId = model.DeptId,
                        CompId = model.CompId
                    };
                    _context.Add(empPosition);
                    SysEmployees SysEmployees = new SysEmployees()
                    {
                        EmpId = model.EmpNo,
                        EmpNo = model.EmpNo,
                        EmpName = model.EmpName,
                        MobilePhone = model.MobilePhone,
                        Email = model.Email
                    };
                    _context.Add(SysEmployees);

                    //如果设为系统账户，就新建账号:账号默认为员工编码。
                    //并添加用户申明UserClaim
                    if (model.IsUser)
                    {
                        if (model.RoleIds == null)
                        {
                            ModelState.AddModelError("RoleIdsCannotNull", "请选择角色！");
                            return View(model);
                        }
                        if (model.Password == null)
                        {
                            ModelState.AddModelError("PassWordCannotNull", "密码不能为空！");
                            return View(model);
                        }
                        var user = new IdentityUser()
                        {
                            Id = model.EmpNo,
                            UserName = model.EmpNo,
                            Email = model.Email,
                            PhoneNumber = model.MobilePhone,
                            EmailConfirmed = true
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            //写入员工编码、创建人、创建时间
                            var AspNetUsers = _context.AspNetUsers.Where(u => u.Id == user.Id)
                                 .FirstOrDefault();
                            AspNetUsers.EmployNo = model.EmpNo;
                            AspNetUsers.Creator = User.Identity.Name;
                            AspNetUsers.CreateTime = DateTime.Now;

                            _context.Entry(AspNetUsers).State = EntityState.Modified;
                            //角色
                            foreach (var eachRoleId in model.RoleIds)
                            {
                                AspNetUserRoles userRole = new AspNetUserRoles()
                                {
                                    RoleId = eachRoleId,
                                    UserId = user.UserName
                                };
                                _context.AspNetUserRoles.Add(userRole);
                            }

                            // 添加UserClaim
                            IList<Claim> oldClaims = await _userManager.GetClaimsAsync(user);
                            if (oldClaims != null)
                            {
                                await _userManager.RemoveClaimsAsync(user, oldClaims);
                            }
                            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                            List<Claim> claims = new List<Claim>();
                            claims.Add(new Claim("EmployNo", model.EmpNo, ClaimValueTypes.String, "RemoteClaims"));
                            claims.Add(new Claim("EmpName", model.EmpName, ClaimValueTypes.String, "RemoteClaims"));
                            claims.Add(new Claim("PostId", model.PostId, ClaimValueTypes.String, "RemoteClaims"));
                            claims.Add(new Claim("DeptId", model.DeptId, ClaimValueTypes.String, "RemoteClaims"));
                            claims.Add(new Claim("CompId", model.CompId, ClaimValueTypes.String, "RemoteClaims"));
                            claims.Add(new Claim("PhoneNumber", model.MobilePhone, ClaimValueTypes.String, "RemoteClaims"));
                            claims.Add(new Claim("Email", model.Email, ClaimValueTypes.String, "RemoteClaims"));
                            await _userManager.AddClaimsAsync(user, claims);
                        }

                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("EmployeeList");
                }

            }
            return View(model);
        }

        //加载下拉列表
        public void SelectLists()
        {
            SelectList selList = new SelectList(_context.SysPositions.OrderBy(d => d.PostOrder).ToList(), "PostId", "PostName");
            ViewBag.PostList = selList.AsEnumerable();

            SelectList selDeptList = new SelectList(_context.SysDepartments.OrderBy(u => u.DeptOrder).ToList(), "DeptId", "DeptName");
            ViewBag.DeptList = selDeptList.AsEnumerable();

            SelectList selcompList = new SelectList(_context.SysCompanies.OrderBy(u => u.CompOrder).ToList(), "CompId", "ShortName");
            ViewBag.CompList = selcompList.AsEnumerable();

        }

        public void SelectRoleList()
        {
            SelectList selRoleList = new SelectList(_context.AspNetRoles.Where(u => !u.IsRemoved).Select(u => new
            {
                RoleId = u.Id,
                RoleName = u.Name
            }).ToList(), "RoleId", "RoleName");
            ViewBag.RoleList = selRoleList.AsEnumerable();
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(string empid, string deptid, string compid, string postid)
        {
            SelectLists();

            ViewEmployees ViewEmployees = await _context.ViewEmployees
                .Where(u => u.EmpId == empid && u.DeptId == deptid &&
                u.CompId == compid && u.PostId == postid)
                .FirstAsync();
            if (ViewEmployees == null)
            {
                return NotFound();
            }

            var aspNetRoles = _context.AspNetRoles.Where(u => !u.IsRemoved).Select(u => new
            {
                RoleId = u.Id,
                RoleName = u.Name
            }).ToList();

            List<SelectListItem> selRoleList = new List<SelectListItem>();

            foreach (var each in aspNetRoles)
            {
                if (ViewEmployees.RoleIds != null && Array.IndexOf(ViewEmployees.RoleIds.Split(','), each.RoleId) >= 0)
                {
                    selRoleList.Add(new SelectListItem { Text = each.RoleName, Value = each.RoleId, Selected = true });
                }
                else
                {
                    selRoleList.Add(new SelectListItem { Text = each.RoleName, Value = each.RoleId, Selected = false });
                }
            }
            ViewBag.RoleList = selRoleList.AsEnumerable();
            return View(new CreateEmployee()
            {
                EmpId = ViewEmployees.EmpId,
                EmpNo = ViewEmployees.EmpNo,
                EmpName = ViewEmployees.EmpName,
                MobilePhone = ViewEmployees.MobilePhone,
                Email = ViewEmployees.Email,
                PostId = ViewEmployees.PostId,
                PostName = ViewEmployees.PostName,
                DeptId = ViewEmployees.DeptId,
                DeptName = ViewEmployees.DeptName,
                CompId = ViewEmployees.CompId,
                ShortName = ViewEmployees.ShortName,
                IsUser = ViewEmployees.IsUser
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(CreateEmployee model)
        {
            SelectLists();
            SelectRoleList();
            if (ModelState.IsValid)
            {
                SysEmployees sysEmployeesModel = new SysEmployees()
                {
                    EmpId = model.EmpId,
                    EmpNo = model.EmpNo,
                    EmpName = model.EmpName,
                    MobilePhone = model.MobilePhone,
                    Email = model.Email
                };
                SysEmpPositions sysEmpPostModel = new SysEmpPositions()
                {
                    EmpId = model.EmpId,
                    DeptId = model.DeptId,
                    CompId = model.CompId,
                    PostId = model.PostId
                };

                List<SysEmpPositions> sysEmpPostDeleteModelList =
                _context.SysEmpPositions.Where(
                    u => u.EmpId == model.EmpId).ToList();
                foreach (SysEmpPositions each in sysEmpPostDeleteModelList)
                {
                    _context.Entry(each).State = EntityState.Deleted;
                }

                _context.Entry(sysEmployeesModel).State = EntityState.Modified;

                _context.Add(sysEmpPostModel);

                //1、原本不是系统账户，现在变为系统账户：添加账户，添加声明
                if (model.IsUser &&
                    !(await _context.AspNetUsers.AnyAsync(u => u.EmployNo == model.EmpNo)))
                {
                    if (model.RoleIds == null)
                    {
                        ModelState.AddModelError("RoleIdsCannotNull", "请选择角色！");
                        return View(model);
                    }
                    //if (model.Password == null)
                    //{
                    //    ModelState.AddModelError("PassWordCannotNull", "密码不能为空！");
                    //    return View(model);
                    //}
                    var user = new IdentityUser() { Id = model.EmpNo, UserName = model.EmpNo, Email = model.Email, PhoneNumber = model.MobilePhone, EmailConfirmed = true };
                    //默认密码12345678
                    var result = await _userManager.CreateAsync(user, "12345678");

                    if (result.Succeeded)
                    {
                        //写入员工编码、创建人、创建时间
                        var AspNetUsers = _context.AspNetUsers.Where(u => u.Id == user.Id)
                             .FirstOrDefault();
                        AspNetUsers.EmployNo = model.EmpNo;
                        AspNetUsers.Creator = User.Identity.Name;
                        AspNetUsers.CreateTime = DateTime.Now;

                        _context.Entry(AspNetUsers).State = EntityState.Modified;
                        //角色
                        foreach (var eachRoleId in model.RoleIds)
                        {
                            AspNetUserRoles userRole = new AspNetUserRoles()
                            {
                                RoleId = eachRoleId,
                                UserId = user.Id
                            };
                            _context.AspNetUserRoles.Add(userRole);
                        }

                        // 添加UserClaim
                        IList<Claim> oldClaims = await _userManager.GetClaimsAsync(user);
                        if (oldClaims != null)
                        {
                            await _userManager.RemoveClaimsAsync(user, oldClaims);
                        }
                        ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim("EmployNo", model.EmpNo, ClaimValueTypes.String, "RemoteClaims"));
                        claims.Add(new Claim("EmpName", model.EmpName, ClaimValueTypes.String, "RemoteClaims"));
                        claims.Add(new Claim("PostId", model.PostId, ClaimValueTypes.String, "RemoteClaims"));
                        claims.Add(new Claim("DeptId", model.DeptId, ClaimValueTypes.String, "RemoteClaims"));
                        claims.Add(new Claim("CompId", model.CompId, ClaimValueTypes.String, "RemoteClaims"));
                        claims.Add(new Claim("PhoneNumber", model.MobilePhone, ClaimValueTypes.String, "RemoteClaims"));
                        claims.Add(new Claim("Email", model.Email, ClaimValueTypes.String, "RemoteClaims"));
                        await _userManager.AddClaimsAsync(user, claims);
                    }
                }
                //2、原本是系统账户，现在不是系统账户：删除账户（删除声明）、删除角色
                if (await _context.AspNetUsers.AnyAsync(u => u.EmployNo == model.EmpNo)
                    && !model.IsUser)
                {
                    var AspNetUsers = _context.AspNetUsers.FirstOrDefault(u => u.EmployNo == model.EmpNo);
                    _context.AspNetUsers.Attach(AspNetUsers);
                    _context.AspNetUsers.Remove(AspNetUsers);

                    List<AspNetUserRoles> aspNetUserRoles = _context.AspNetUserRoles.Where(u => u.UserId == AspNetUsers.Id).ToList();
                    if (aspNetUserRoles != null)
                    {
                        foreach (AspNetUserRoles each in aspNetUserRoles)
                        {
                            _context.AspNetUserRoles.Attach(each);
                            _context.AspNetUserRoles.Remove(each);
                        }
                    }
                }
                //3、仍然是系统账户，但是有可能修改了角色和用户信息：修改账户信息（电话、邮箱、职务、部门、所属公司）、AspNetUserRole（角色）、声明
                if (model.IsUser && await _context.AspNetUsers.AnyAsync(u => u.EmployNo == model.EmpNo))
                {
                    if (model.RoleIds == null)
                    {
                        ModelState.AddModelError("RoleIdsCannotNull", "请选择角色！");
                        return View(model);
                    }
                    AspNetUsers aspNetUsers = _context.AspNetUsers.Where(u => u.EmployNo == model.EmpNo).FirstOrDefault();
                    aspNetUsers.Email = model.Email;
                    aspNetUsers.PhoneNumber = model.MobilePhone;
                    _context.Entry(aspNetUsers).State = EntityState.Modified;
                    //修改角色
                    List<AspNetUserRoles> userRole = _context.AspNetUserRoles.Where(u => u.UserId == aspNetUsers.Id).ToList();
                    if (userRole != null)
                    {
                        foreach (var each in userRole)
                        {
                            _context.Entry(each).State = EntityState.Deleted;
                        }
                        await _context.SaveChangesAsync();
                    }
                    foreach (var eachRole in model.RoleIds)
                    {
                        AspNetUserRoles userRoleAdd = new AspNetUserRoles()
                        {
                            UserId = aspNetUsers.Id,
                            RoleId = eachRole
                        };
                        _context.Add(userRoleAdd);
                    }
                    // 添加UserClaim
                    IdentityUser user = new IdentityUser() { UserName = model.EmpNo, Email = model.Email, PhoneNumber = model.MobilePhone, EmailConfirmed = true };
                    IList<Claim> oldClaims = await _userManager.GetClaimsAsync(user);
                    if (oldClaims != null)
                    {
                        await _userManager.RemoveClaimsAsync(user, oldClaims);
                    }
                    ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("EmployNo", model.EmpNo, ClaimValueTypes.String, "RemoteClaims"));
                    claims.Add(new Claim("EmpName", model.EmpName, ClaimValueTypes.String, "RemoteClaims"));
                    claims.Add(new Claim("PostId", model.PostId, ClaimValueTypes.String, "RemoteClaims"));
                    claims.Add(new Claim("DeptId", model.DeptId, ClaimValueTypes.String, "RemoteClaims"));
                    claims.Add(new Claim("CompId", model.CompId, ClaimValueTypes.String, "RemoteClaims"));
                    claims.Add(new Claim("PhoneNumber", model.MobilePhone, ClaimValueTypes.String, "RemoteClaims"));
                    claims.Add(new Claim("Email", model.Email, ClaimValueTypes.String, "RemoteClaims"));
                    await _userManager.AddClaimsAsync(user, claims);
                }
                //4、原来不是，现在也不是：不作处理

                await _context.SaveChangesAsync();
                return RedirectToAction("EmployeeList");
            }
            return View(model);
        }


        public async Task<IActionResult> DeleteEmployee(string id, string empNo)
        {
            //删除员工
            var employee = _context.SysEmployees.FirstOrDefault(u => u.EmpId == id);
            if (employee == null)
            {
                return NotFound();
            }
            _context.SysEmployees.Attach(employee);
            _context.SysEmployees.Remove(employee);
            //删除员工职位关系
            List<SysEmpPositions> sysEmpPostDeleteModelList =
           _context.SysEmpPositions.Where(
                   u => u.EmpId == id).ToList();
            if (sysEmpPostDeleteModelList.Count > 0)
            {
                foreach (SysEmpPositions each in sysEmpPostDeleteModelList)
                {
                    _context.Entry(each).State = EntityState.Deleted;
                }
                await _context.SaveChangesAsync();
            }
            //删除账号、删除账号角色关系、删除声明
            await DeleteUserAsyncDLL(employee.EmpNo);
            return RedirectToAction("EmployeeList");
        }

        #region 重置密码
        [HttpGet]
        public ActionResult ResetPassword(string empNo)
        {
            ViewBag.userId = empNo;
            ViewBag.userName = empNo;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(Models.ResetPasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("EmployeeList");
            }
            else
            {
                return View(model);
            }
        }
        #endregion
        #endregion

        #region 检查数据

        //检查部门编码是否已存在
        public bool CheckDeptId(SysDepartments model)
        {
            return _context.SysDepartments.Any(u => u.DeptId == model.DeptId) ? false : true;
        }


        //检查员工编码是否已存在
        public bool EmployeeIsExist(string strNo)
        {
            return _context.SysEmployees.Any(u => u.EmpNo == strNo) ? true : false;
        }
        #endregion

        #region  用户声明
        /*
         [Authorize]
         public ActionResult PersonalDetail()
         {
             ClaimsIdentity claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
             if (claimsIdentity == null)
             {
                 return View("Error", new string[] { "未找到声明" });
             }
             else
             {
                 return View(claimsIdentity.Claims);
             }
         }
         */
        [Authorize]
        public async Task<IActionResult> UserClaimList(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            return View(await _userManager.GetClaimsAsync(user));
        }
        #endregion

        #region 树
        // 加载节点
        public JsonResult GetTreeData()
        {
            var items = _context.ViewOrganizationTree
                 .Select(u => new
                 {
                     id = u.Id,
                     parent = u.ParentId.ToString() == "0" ? "#" : u.ParentId.ToString(), // root必须是# ！
                    text = u.Name,
                     type = u.DataType
                    // children = _context.SysDepartments.Any(p => p.ParentId == u.DeptId) // true|false 是否有子项
                }
                 );
            return Json(items);
        }

        public ActionResult DeptTree()
        {
            return View();
        }
        
        public JsonResult GetRightList(string clickId, string clickType)
        {

            if (clickType == "comp")//门店
            {
                if (clickId == "yxjt")
                {
                    var item = _context.SysCompanies.Where(u => u.ParentId == clickId)
                        .Select(u => new
                        {
                            fullName = u.FullName,
                            parentType = "yxjt"
                        }
                        );
                    return Json(item);
                }
                else
                {
                    var item2 = _context.SysDepartments.Where(u => u.ParentId == clickId)
                        .Select(u => new
                        {
                            deptOrder = u.DeptOrder,
                            deptName = u.DeptName,
                            deptLevel = u.DeptLevel,
                            parentType = "comp"
                        });
                    return Json(item2);
                }

            }
            else if (clickType == "dept")
            {
                var item3 =_context.ViewEmployees.Where(u => u.DeptId == clickId)
                       .Select(u => new
                       {
                           empNo = u.EmpNo,
                           empName = u.EmpName,
                           mobilePhone = u.MobilePhone,
                           email = u.Email,
                           postName = u.PostName,
                           parentType = "dept"
                       }
                       );
                return Json(item3);
            }
            else {
                var item4 = _context.ViewEmployees.Where(u => u.PostId == clickId)
                          .Select(u => new
                          {
                              empNo = u.EmpNo,
                              empName = u.EmpName,
                              mobilePhone = u.MobilePhone,
                              email = u.Email,
                              postName = u.PostName,
                              parentType = "post"
                          }
                          );
                return Json(item4);
            }
        } 
        #endregion
    }


}
