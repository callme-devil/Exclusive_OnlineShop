using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Infrastructure.EFCore.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role
{
    [PermissionChecker(Roles.CreateRole)]
    public class CreateModel : PageModel
    {
        private readonly IRoleApplication _roleApplication;

        public CreateRole Command;

        public List<PermissionViewModel> Permission;

        public CreateModel(IRoleApplication roleApplication)
        {
            _roleApplication = roleApplication;
        }

        public void OnGet()
        {
            Permission = _roleApplication.GetAllPermissions();
        }

        public IActionResult OnPost(CreateRole command , List<int> SelectedPermission)
        {
            var result = _roleApplication.Create(command , SelectedPermission);

            return RedirectToPage("./Index", new { Created = "True" });
        }
    }
}
