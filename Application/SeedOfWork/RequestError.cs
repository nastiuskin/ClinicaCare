using FluentResults;

namespace Application.SeedOfWork
{
    public class RequestError : IError
    {
        public List<IError> Reasons { get; private set; } = new List<IError>();

        public string Message { get; private set; }
        public FailureTypes FailureType { get; private set; }


        public Dictionary<string, object> Metadata { get; private set; } = new Dictionary<string, object>();

        // Constructor for creating an error with a message
        public RequestError(string message)
        {
            Message = message;
        }

        public RequestError(string message, List<IError> reasons)
        {
            Message = message;
            Reasons = reasons ?? new List<IError>();
        }

        public RequestError(string message, List<IError> reasons, Dictionary<string, object> metadata)
        {
            Message = message;
            Reasons = reasons ?? new List<IError>();
            Metadata = metadata ?? new Dictionary<string, object>();
        }

        public RequestError(FailureTypes failureTypes, string message)
        {
            Message = message;
        }

        public static RequestError NotFound(Guid aggregateId)
            => new(FailureTypes.NOT_FOUND, "The resource not found");

        public static RequestError Dublicate()
            => new(FailureTypes.DUBLICATE, "The resource already exists");
    }
}
