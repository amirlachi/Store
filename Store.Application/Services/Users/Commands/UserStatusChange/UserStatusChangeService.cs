using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;

namespace Store.Application.Services.Users.Commands.UserStatusChange
{
    public class UserStatusChangeService(IDataBaseContext context) : IUserStatusChangeService
    {
        public ResultDto Execute(long UserId)
        {
            var user = context.Users.Find(UserId);
            if (user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }

            user.IsActive = !user.IsActive;
            context.SaveChanges();

            string userState = user.IsActive == true ? "فعال" : "غیرفعال";
            return new ResultDto()
            {
                IsSuccess = true,
                Message = $"کاربر با موفقیت {userState} شد",
            };
        }
    }
}
