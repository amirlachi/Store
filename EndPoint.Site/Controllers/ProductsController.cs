using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.FacadPatterns;
using Store.Application.Services.Products.Queries.GetProductForSite;

namespace EndPoint.Site.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductFacad _productFacad;

        public ProductsController(IProductFacad productFacad)
        {
            _productFacad = productFacad;
        }

        public IActionResult Index(Ordering ordering, string SearchKey,long? CategoryId = null, int Page = 1, int PageSize = 20)
        {
            return View(_productFacad.GetProductForSiteService.Execute(ordering, SearchKey, CategoryId, Page, PageSize).Data);
        }

        public IActionResult Detail(long Id)
        {
            return View(_productFacad.GetProductDetailForSiteService.Execute(Id).Data);
        }
    }
}
