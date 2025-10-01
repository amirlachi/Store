using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;

namespace Store.Application.Services.Commands.RemoveUser
{
    public class RemoveUserService(IDataBaseContext context) : IRemoveUserService 
    {
        public ResultDto Execute(long UserId)
        {
            var user = context.Users.Find(UserId);
            if (user == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }
            user.RemoveTime = DateTime.Now;
            user.IsRemoved = true;
            context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "کاربر با موفقیت حذف شد"
            };
        }
    }
}
