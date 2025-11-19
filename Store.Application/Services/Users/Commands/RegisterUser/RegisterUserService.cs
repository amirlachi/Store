using FluentValidation;
using Store.Application.Interfaces.Contexts;
using Store.Common;
using Store.Common.Dto;
using Store.Domain.Entities.Users;

namespace Store.Application.Services.Users.Commands.RegisterUser
{
    public class RegisterUserService(IDataBaseContext context, IValidator<RequestRegisterUserDto> validator)
        : IRegisterUserService
    {
        public ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return new ResultDto<ResultRegisterUserDto>
                {
                    Data = new ResultRegisterUserDto { UserId = 0 },
                    IsSuccess = false,
                    Message = string.Join("، ", validationResult.Errors.Select(e => e.ErrorMessage))
                };
            }

            try
            {
                var passwordHasher = new PasswordHasher();
                var hashedPassword = passwordHasher.HashPassword(request.Password!);

                var user = new User
                {
                    Email = request.Email!,
                    FullName = request.FullName!,
                    Password = hashedPassword,
                    IsActive = true,
                };

                user.UserInRoles = request.roles!.Select(r =>
                {
                    var role = context.Roles.Find(r.Id)!;
                    return new UserInRole
                    {
                        Role = role,
                        RoleId = role.Id,
                        User = user
                    };
                }).ToList();

                context.Users.Add(user);
                context.SaveChanges();

                return new ResultDto<ResultRegisterUserDto>
                {
                    Data = new ResultRegisterUserDto { UserId = user.Id },
                    IsSuccess = true,
                    Message = "ثبت نام کاربر انجام شد"
                };
            }
            catch (Exception)
            {
                return new ResultDto<ResultRegisterUserDto>
                {
                    Data = new ResultRegisterUserDto { UserId = 0 },
                    IsSuccess = false,
                    Message = "ثبت نام انجام نشد"
                };
            }
        }
    }
}
