using Application.Configuration.Commands;
using Application.SeedOfWork;
using Domain.Users;
using FluentResults;

namespace Application.UserAccountManagement
{
    public record DeleteUserCommand(Guid id)
       : ICommand<Result>;

    public class DeleteMedicalProcedureCommandHandler
        : ICommandHandler<DeleteUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;

        public DeleteMedicalProcedureCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(DeleteUserCommand command,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(new UserId(command.id));
            if (user == null)
                return Result.Fail(ResponseError.NotFound(nameof(user), command.id).Message);

            await _userRepository.DeleteAsync(user);
            return Result.Ok();
        }
    }

}


