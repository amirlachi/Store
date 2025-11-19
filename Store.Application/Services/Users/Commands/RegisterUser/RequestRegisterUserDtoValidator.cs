using FluentValidation;

namespace Store.Application.Services.Users.Commands.RegisterUser
{
    public class RequestRegisterUserDtoValidator : AbstractValidator<RequestRegisterUserDto>
    {
        public RequestRegisterUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("ایمیل را وارد کنید")
                .EmailAddress().WithMessage("ایمیل خودرا به درستی وارد نمایید");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("نام را وارد کنید");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("رمز عبور را وارد کنید")
                .MinimumLength(8).WithMessage("رمز عبور باید حداقل 8 کاراکتر باشد");

            RuleFor(x => x.RePassword)
                .Equal(x => x.Password).WithMessage("رمزعبور و تکرار آن برابر نیست");
        }
    }
}
