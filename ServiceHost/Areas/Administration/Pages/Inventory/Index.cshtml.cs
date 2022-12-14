using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore.Security;
using InventoryManagement.Application.Contract.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Pages.Inventory
{
    [PermissionChecker(Roles.ManageInventory)]
    public class IndexModel : PageModel
    {
        private readonly IProductApplication _productApplication;
        private readonly IInventoryApplication _inventoryApplication;


        public InventorySearchModel SearchModel;
        public List<InventoryViewModel> Inventory;
        public SelectList Products;

        public IndexModel(IProductApplication productApplication, IInventoryApplication inventoryApplication)
        {
            _productApplication = productApplication;
            _inventoryApplication = inventoryApplication;
        }

        public void OnGet(InventorySearchModel searchModel , bool restored = false , bool removed = false)
        {
            ViewData["Restored"] = restored;
            ViewData["Removed"] = removed;
            Products = new SelectList(_productApplication.GetProducts(),"Id" , "Name");
            Inventory = _inventoryApplication.Search(searchModel);
        }

        [PermissionChecker(Roles.AddInventory)]
        public IActionResult OnGetCreate()
        {
            var command = new CreateInventory()
            {
                Products = _productApplication.GetProducts()
            };
            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(CreateInventory command)
        {
            var result = _inventoryApplication.Create(command);
            return new JsonResult(result);
        }

        [PermissionChecker(Roles.EditInventory)]
        public IActionResult OnGetEdit(int id)
        {
            var inventory  = _inventoryApplication.GetDetails(id);
            inventory.Products = _productApplication.GetProducts();
            return Partial("Edit", inventory);
        }

        public JsonResult OnPostEdit(EditInventory command)
        {
            var result = _inventoryApplication.Edit(command);

            return new JsonResult(result);
        }

        [PermissionChecker(Roles.IncreaseItemInventory)]
        public IActionResult OnGetIncrease(int id)
        {
            var command = new IncreaseInventory()
            {
                InventoryId = id
            };

            return Partial("Increase", command);
        }

        public JsonResult OnPostIncrease(IncreaseInventory command)
        {
            var result = _inventoryApplication.Increase(command);

            return new JsonResult(result);
        }

        [PermissionChecker(Roles.DecreaseItemInventory)]
        public IActionResult OnGetReduce(int id)
        {
            var command = new ReduceInventory()
            {
                InventoryId = id
            };

            return Partial("Reduce", command);
        }
        public JsonResult OnPostReduce(ReduceInventory command)
        {
            var result = _inventoryApplication.Reduce(command);

            return new JsonResult(result);
        }

        [PermissionChecker(Roles.ShowOperationHistory)]
        public IActionResult OnGetOperationLog(int id)
        {
            var operationLog = _inventoryApplication.GetOperationLog(id);

            return Partial("OperationLog", operationLog);
        }
    }
}
