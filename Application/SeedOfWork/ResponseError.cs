using FluentResults;

namespace Application.SeedOfWork
{
    public class ResponseError : IError
    {
        public List<IError> Reasons { get; private set; } = new List<IError>();

        public string Message { get; private set; }
        public FailureTypes FailureType { get; private set; }


        public Dictionary<string, object> Metadata { get; private set; } = new Dictionary<string, object>();

        // Constructor for creating an error with a message
        public ResponseError(string message)
        {
            Message = message;
        }

        public ResponseError(string message, List<IError> reasons)
        {
            Message = message;
            Reasons = reasons ?? new List<IError>();
        }

        public ResponseError(string message, List<IError> reasons, Dictionary<string, object> metadata)
        {
            Message = message;
            Reasons = reasons ?? new List<IError>();
            Metadata = metadata ?? new Dictionary<string, object>();
        }

        public ResponseError(FailureTypes failureTypes, string message)
        {
            Message = message;
        }

        public static ResponseError NotFound(string entityName, Guid id)
            => new(FailureTypes.NOT_FOUND, $"The {entityName} with {id} not found");

        public static ResponseError Dublicated(string entityName)
            => new(FailureTypes.DUBLICATED, $"The {entityName} already exists");
    }
}
