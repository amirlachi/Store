namespace Store.Application.Services.Queries.GetUsers
{
    public partial class GetUsersService
    {
        public class RequestGetUserDto
        {
            public required string SearchKey { get; set; }
            public int Page { get; set; }
        }
    }
}
