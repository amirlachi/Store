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
    public class UsersController : Controller
    {
        private readonly IGetUsersService _getUsersService;
        private readonly IGetRolesService _getRolesService;
        private readonly IRegisterUserService _registerUserService;
        private readonly IRemoveUserService _removeUserService;
        private readonly IUserStatusChangeService _userStatusChangeService;
        private readonly IEditUserService _editUserService;
        public UsersController(IGetUsersService getUsersService, 
            IGetRolesService getRolesService, 
            IRegisterUserService registerUserService,
            IRemoveUserService removeUserService,
            IUserStatusChangeService userStatusChangeService,
            IEditUserService editUserService)
        {
            _getUsersService = getUsersService;
            _getRolesService = getRolesService;
            _registerUserService = registerUserService;
            _removeUserService = removeUserService;
            _userStatusChangeService = userStatusChangeService;
            _editUserService = editUserService;
        }

        public IActionResult Index(string searchkey, int page = 1)
        {
            return View(_getUsersService.Execute(new RequestGetUserDto
            {
                SearchKey = searchkey,
                Page = page
            }));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_getRolesService.Execute().Data, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(string Email, string FullName, long RoleId, string Password, string RePassword)
        {
            var result = _registerUserService.Execute(new RequestRegisterUserDto
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
            return Json(_removeUserService.Execute(UserId));
        }

        [HttpPost]
        public IActionResult UserStatusChange(int UserId)
        {
            return Json(_userStatusChangeService.Execute(UserId));
        }

        [HttpPost]
        public IActionResult Edit(long UserId, string Fullname)
        {
            return Json(_editUserService.Execute(new RequestEditUserDto
            {
                Fullname = Fullname,
                UserId = UserId,
            }));
        }
    }
}
