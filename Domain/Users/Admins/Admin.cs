using FluentResults;

namespace Domain.Users.Admins
{
    public class Admin : User
    {
        protected Admin() { }

        private Admin(string email) 
        {
            Id = new UserId(Guid.NewGuid());
            Email = email;
            UserName = email;
        }
        public static Result<Admin> Create(string email)
        {
            return Result.Ok(new Admin(email));
        }
    }
}
