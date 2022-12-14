using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductPicture;

namespace ServiceHost.Areas.Administration.Pages.Shop.ProductPictures
{
    [PermissionChecker(Roles.ManageProductPicture)]
    public class IndexModel : PageModel
    {
        private readonly IProductApplication _productApplication;
        private readonly IProductPictureApplication _productPictureApplication;

        public ProductPictureSearchModel SearchModel;
        public List<ProductPictureViewModel> ProductPictures;
        public SelectList Products;

        public IndexModel(IProductApplication productApplication, IProductPictureApplication productPictureApplication)
        {
            _productApplication = productApplication;
            _productPictureApplication = productPictureApplication;
        }

        public void OnGet(ProductPictureSearchModel searchModel , bool restored = false , bool removed = false)
        {
            ViewData["Restored"] = restored;
            ViewData["Removed"] = removed;
            Products = new SelectList(_productApplication.GetProducts(),"Id" , "Name");
            ProductPictures = _productPictureApplication.Search(searchModel);
        }

        [PermissionChecker(Roles.AddProductPicture)]
        public IActionResult OnGetCreate()
        {
            var command = new CreateProductPicture
            {
                Products = _productApplication.GetProducts()
            };
            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(CreateProductPicture command)
        {
            var result = _productPictureApplication.Create(command);
            return new JsonResult(result);
        }

        [PermissionChecker(Roles.EditProductPicture)]
        public IActionResult OnGetEdit(int id)
        {
            var ProductPicture = _productPictureApplication.GetDetails(id);
            ProductPicture.Products = _productApplication.GetProducts();
            return Partial("Edit", ProductPicture);
        }

        public JsonResult OnPostEdit(EditProductPicture command)
        {
            var result = _productPictureApplication.Edit(command);

            return new JsonResult(result);
        }

        [PermissionChecker(Roles.DeleteProductPicture)]
        public IActionResult OnGetRemove(int id)
        {
            var result = _productPictureApplication.Remove(id);
           
            return RedirectToPage("./Index" , new {Removed="True"});
            
        }

        [PermissionChecker(Roles.DeleteProductPicture)]
        public IActionResult OnGetRestore(int id)
        {
            var result = _productPictureApplication.Restore(id);

            return RedirectToPage("./Index", new { Restored = "True" });
        }
    }
}
