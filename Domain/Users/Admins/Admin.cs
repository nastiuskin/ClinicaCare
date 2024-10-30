using FluentResults;

namespace Domain.Users.Admins
{
    public class Admin : User
    {
        protected Admin() { }
        private Admin(UserParams userParams) : base(userParams) { }

        public Result<Admin> Create(UserParams adminParams)
        {
            return Result.Ok(new Admin(adminParams));
        }
    }
}
