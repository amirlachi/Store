using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;

namespace Store.Application.Services.Commands.UserStatusChange
{
    public class UserStatusChangeService : IUserStatusChangeService
    {
        private IDataBaseContext _context;
        public UserStatusChangeService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(long UserId)
        {
            var user = _context.Users.Find(UserId);
            if (user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }

            user.IsActive = !user.IsActive;
            _context.SaveChanges();

            string userState = user.IsActive == true ? "فعال" : "غیرفعال";
            return new ResultDto()
            {
                IsSuccess = true,
                Message = $"کاربر با موفقیت {userState} شد",
            };
        }
    }
}
