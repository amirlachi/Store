using Store.Application.Interfaces.Contexts;
using Store.Common;

namespace Store.Application.Services.Queries.GetUsers
{
    public partial class GetUsersService(IDataBaseContext context) : IGetUsersService
    {
        public ResultGetUserDto Execute(RequestGetUserDto request)
        {
            var users = context.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                users=users.Where(p=> p.FullName!.Contains(request.SearchKey) && p.Email!.Contains(request.SearchKey));
            }

            int rowsCount = 0;
            var userslist = users.ToPaged(request.Page, 20, out rowsCount).Select(p=> new GetUsersDto
            {
                Email = p.Email!,
                FullName = p.FullName!,
                Id = p.Id,
                IsActive = p.IsActive,
            }).ToList();

            return new ResultGetUserDto
            {
                Rows = rowsCount,
                Users = userslist,
            };
        }
    }
}
