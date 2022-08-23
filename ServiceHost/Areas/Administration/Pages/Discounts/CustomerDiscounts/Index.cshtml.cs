using DiscountManagement.Application.Contract.CustomerDiscount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Pages.Discounts.CustomerDiscounts
{
    public class IndexModel : PageModel
    {
        private readonly IProductApplication _productApplication;
        private readonly ICustomerDiscountApplication _customerDiscountApplication;


        public CustomerDiscountSearchModel SearchModel;
        public List<CustomerDiscountViewModel> CustomerDiscounts;
        public SelectList Products;

        public IndexModel(IProductApplication productApplication, ICustomerDiscountApplication customerDiscountApplication)
        {
            _productApplication = productApplication;
            _customerDiscountApplication = customerDiscountApplication;
        }

        public void OnGet(CustomerDiscountSearchModel searchModel , bool inStock = false , bool emptyStock = false)
        {
            ViewData["InStock"] = inStock;
            ViewData["EmptyStock"] = emptyStock;
            Products = new SelectList(_productApplication.GetProducts(),"Id" , "Name");
            CustomerDiscounts = _customerDiscountApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            var command = new DefineCustomerDiscount
            {
                Products = new SelectList(_productApplication.GetProducts(),"Id" , "Name")
            };
            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(DefineCustomerDiscount command)
        {
            var result = _customerDiscountApplication.Define(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(int id)
        {
            var product = _productApplication.GetDetails(id);
            product.Categories = _productCategoryApplication.GetProductsCategories();
            return Partial("Edit", product);
        }

        public JsonResult OnPostEdit(EditProduct command)
        {
            var result = _productApplication.Edit(command);

            return new JsonResult(result);
        }

        public IActionResult OnGetEmptyStock(int id)
        {
            var result = _productApplication.EmptyStock(id);
           
            return RedirectToPage("./Index" , new {EmptyStock="True"});
            

            //Message = result.Message;
            //return RedirectToPage("./Index");
        }

        public IActionResult OnGetIsInStock(int id)
        {
            var result = _productApplication.InStock(id);

            return RedirectToPage("./Index", new { InStock = "True" });
        }
    }
}
