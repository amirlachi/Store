using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.FacadPatterns;

namespace EndPoint.Site.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductFacad _productFacad;

        public ProductsController(IProductFacad productFacad)
        {
            _productFacad = productFacad;
        }

        public IActionResult Index(int Page = 1)
        {
            return View(_productFacad.GetProductForSiteService.Execute(Page).Data);
        }
    }
}
