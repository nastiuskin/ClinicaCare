using Domain.SeedWork;
using Domain.Validation;
using FluentResults;

namespace Domain.Admins
{
    public class Admin : User
    {
        protected Admin() { }
        private Admin(UserParams userParams) : base(userParams) { }

        public Result<Admin> Create(UserParams adminParams)
        {
            var validator = new UserCreateValidator();
            var validationResult = validator.Validate(adminParams);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(error => new FluentResults.Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);

            }
            return Result.Ok(new Admin(adminParams));
        }
    }
}
