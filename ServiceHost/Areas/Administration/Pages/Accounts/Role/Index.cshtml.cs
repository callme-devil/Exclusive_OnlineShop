using AccountManagement.Application.Contracts.Role;
using AccountManagement.Infrastructure.EFCore.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role
{
    [PermissionChecker(_0_Framework.Infrastructure.Roles.ManageRoles)]
    public class IndexModel : PageModel
    {
        private readonly IRoleApplication _roleApplication;

        //[TempData] 
        //public string Message { get; set; }

        public List<RoleViewModel> Roles;

        public IndexModel(IRoleApplication roleApplication)
        {
            _roleApplication = roleApplication;
        }

        public void OnGet(bool created = false, bool edited = false, bool activated = false, bool deactivated = false)
        {
            ViewData["Created"] = created;
            ViewData["Edited"] = edited;
            ViewData["Activated"] = activated;
            ViewData["DeActivated"] = deactivated;
            Roles = _roleApplication.List();
        }

        [PermissionChecker(_0_Framework.Infrastructure.Roles.DeleteRole)]
        public IActionResult OnGetDeActive(int id)
        {
            var result = _roleApplication.Remove(id);

            return RedirectToPage("./Index", new { DeActivated = "True" });

        }

        [PermissionChecker(_0_Framework.Infrastructure.Roles.DeleteRole)]
        public IActionResult OnGetActive(int id)
        {
            var result = _roleApplication.Restore(id);

            return RedirectToPage("./Index", new { Activated = "True" });
        }
    }
}
