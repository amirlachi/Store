using Store.Application.Interfaces.Contexts;
using Store.Common;
using Store.Common.Dto;
using Store.Domain.Entities.Users;
using System.Text.RegularExpressions;

namespace Store.Application.Services.Commands.RegisterUser
{
    public class RegisterUserService(IDataBaseContext context) : IRegisterUserService
    {
        public ResultDto<ResultRegisterUserDto>  Execute(RequestRegisterUserDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "ایمیل را وارد کنید"
                    };
                }
                if (string.IsNullOrWhiteSpace(request.FullName))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام را وارد کنید"
                    };
                }
                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "رمز را وارد کنید"
                    };
                }
                if (request.Password != request.RePassword)
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "رمزعبور و تکرار آن برابر نیست"
                    };
                }

                string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";

                var match = Regex.Match(request.Email, emailRegex, RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "ایمیل خودرا به درستی وارد نمایید"
                    };
                }

                var passwordHasher = new PasswordHasher();
                var HashedPassword = passwordHasher.HashPassword(request.Password);

                User user = new User()
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    Password = HashedPassword,
                    IsActive = true,
                };

                List<UserInRole> userInRoles = new List<UserInRole>();
                foreach (var item in request.roles!)
                {
                    var roles = context.Roles.Find(item.Id);
                    userInRoles.Add(new UserInRole
                    {
                        Role = roles!,
                        RoleId = roles!.Id,
                        User = user,
                        UserId = user.Id,
                    });
                }
                user.UserInRoles = userInRoles;

                context.Users.Add(user);
                context.SaveChanges();
                return new ResultDto<ResultRegisterUserDto>()
                {
                    Data = new ResultRegisterUserDto()
                    {
                        UserId = user.Id,
                    },
                    IsSuccess = true,
                    Message = "ثبت نام کاربر انجام شد",
                };
            }
            catch (Exception)
            {
                return new ResultDto<ResultRegisterUserDto>
                {
                    Data = new ResultRegisterUserDto()
                    {
                        UserId = 0,
                    },
                    IsSuccess = false,
                    Message = "ثبت نام انجام نشد"
                };
            }
        }
    }
}
