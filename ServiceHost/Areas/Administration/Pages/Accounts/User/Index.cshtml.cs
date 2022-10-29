using AccountManagement.Application.Contracts.Account;
using AccountManagement.Application.Contracts.Account.Admin;
using AccountManagement.Application.Contracts.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Accounts.User
{
    [Authorize(Roles = _0_Framework.Infrastructure.Roles.Administrator)]
    public class IndexModel : PageModel
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IRoleApplication _roleApplication;

        //[TempData] 
        //public string Message { get; set; }

        public AccountSearchModel SearchModel;
        public List<AccountViewModel> Accounts;
        public SelectList Roles;

        public IndexModel(IAccountApplication accountApplication, IRoleApplication roleApplication)
        {
            _accountApplication = accountApplication;
            _roleApplication = roleApplication;
        }

        public void OnGet(AccountSearchModel searchModel , bool removed = false , bool restored = false)
        {
            ViewData["Removed"] = removed;
            ViewData["Restored"] = restored;
            Roles = new SelectList(_roleApplication.List(), "Id", "Name");
            Accounts = _accountApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            var command = new RegisterAccount
            {
                Roles = _roleApplication.List()
            };
            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(RegisterAccount command)
        {
            var account = _accountApplication.Create(command);
            return new JsonResult(account);
        }

        public IActionResult OnGetEdit(int id)
        {
            var account = _accountApplication.GetDetails(id);
            account.Roles = _roleApplication.List();
            return Partial("Edit", account);
        }

        public JsonResult OnPostEdit(EditAccount command)
        {
            var account = _accountApplication.Edit(command);

            return new JsonResult(account);
        }

        public IActionResult OnGetRemove(int id)
        {
            var result = _accountApplication.Remove(id);

            return RedirectToPage("./Index", new { Removed = "True" });

        }

        public IActionResult OnGetRestore(int id)
        {
            var result = _accountApplication.Restore(id);

            return RedirectToPage("./Index", new { Restored = "True" });
        }

        public IActionResult OnGetChangePassword(int id)
        {
            var command = new ChangePasswordViewModel { Id = id };
            return Partial("ChangePassword", command);
        }

        public JsonResult OnPostChangePassword(ChangePasswordViewModel command)
        {
            var result = _accountApplication.ChangePassword(command);

            return new JsonResult(result);
        }
    }
}
