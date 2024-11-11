using Domain.SeedWork;
using FluentResults;
using Microsoft.AspNetCore.Identity;
namespace Domain.Users
{
    public abstract class User : IdentityUser<UserId>, IAggregateRoot
    {
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public DateOnly? DateOfBirth { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }
        protected User() { }
        protected User(UserParams user) 
        {
            Id = new UserId(Guid.NewGuid());
            FirstName = user.FirstName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            LastName = user.LastName;
            DateOfBirth = user.DateOfBirth;
            UserName = user.Email;
        }

        protected Result ChangePhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
            return Result.Ok();
        }

        protected Result ChangeEmail(string email)
        {
            Email = email;
            return Result.Ok();

        }

        public Result SetRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime) 
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
            return Result.Ok();
        }

        public Result DeleteRefreshToken()
        {
            RefreshToken = null;
            RefreshTokenExpiryTime = null;
            return Result.Ok();
        }

    }

}


