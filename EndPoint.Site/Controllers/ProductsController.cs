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

        public IActionResult Index(long? CategoryId = null, int Page = 1)
        {
            return View(_productFacad.GetProductForSiteService.Execute(CategoryId, Page).Data);
        }

        public IActionResult Detail(long Id)
        {
            return View(_productFacad.GetProductDetailForSiteService.Execute(Id).Data);
        }
    }
}
