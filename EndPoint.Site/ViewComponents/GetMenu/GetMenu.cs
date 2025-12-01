using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Common.Queries.GetMenuItem;

namespace EndPoint.Site.ViewComponents.GetMenu
{
    public class GetMenu : ViewComponent
    {
        private readonly IGetMenuItemService _getMenuItem;

        public GetMenu(IGetMenuItemService getMenuItem)
        {
            _getMenuItem = getMenuItem;
        }

        public IViewComponentResult Invoke()
        {
            var menuItem = _getMenuItem.Execute();
            return View(viewName: "GetMenu", menuItem.Data);
        }
    }
}
