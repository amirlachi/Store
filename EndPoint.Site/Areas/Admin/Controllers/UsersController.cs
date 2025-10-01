using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Application.Services.Commands.EditUser;
using Store.Application.Services.Commands.RegisterUser;
using Store.Application.Services.Commands.RemoveUser;
using Store.Application.Services.Commands.UserStatusChange;
using Store.Application.Services.Queries.GetRoles;
using Store.Application.Services.Queries.GetUsers;
using Store.Common.Dto;
using static Store.Application.Services.Queries.GetUsers.GetUsersService;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController(IGetUsersService getUsersService,
            IGetRolesService getRolesService,
            IRegisterUserService registerUserService,
            IRemoveUserService removeUserService,
            IUserStatusChangeService userStatusChangeService,
            IEditUserService editUserService) : Controller
    {

        public IActionResult Index(string searchkey, int page = 1)
        {
            return View(getUsersService.Execute(new RequestGetUserDto
            {
                SearchKey = searchkey,
                Page = page
            }));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(getRolesService.Execute().Data, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(string Email, string FullName, long RoleId, string Password, string RePassword)
        {
            var result = registerUserService.Execute(new RequestRegisterUserDto
            {
                Email = Email,
                FullName = FullName,
                roles = new List<RolesInRegisterUserDto>()
                {
                    new RolesInRegisterUserDto
                    {
                        Id = RoleId
                    }
                },
                Password = Password,
                RePassword = RePassword,
            });
            return Json(result);
        }

        [HttpPost]
        public IActionResult Delete(int UserId)
        {
            return Json(removeUserService.Execute(UserId));
        }

        [HttpPost]
        public IActionResult UserStatusChange(int UserId)
        {
            return Json(userStatusChangeService.Execute(UserId));
        }

        [HttpPost]
        public IActionResult Edit(long UserId, string Fullname)
        {
            return Json(editUserService.Execute(new RequestEditUserDto
            {
                Fullname = Fullname,
                UserId = UserId,
            }));
        }
    }
}
