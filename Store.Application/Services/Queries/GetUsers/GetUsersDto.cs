namespace Store.Application.Services.Queries.GetUsers
{
    public class GetUsersDto
    {
        public long Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
