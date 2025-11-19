using Store.Application.Services.Users.Queries.GetUsers;

namespace Store.Application.Services.Queries.GetUsers
{
    public partial class GetUsersService
    {
        public class ResultGetUserDto
        {
            public required List<GetUsersDto> Users { get; set; }
            public int Rows {  get; set; }
        }
    }
}
