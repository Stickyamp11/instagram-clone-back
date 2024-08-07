using FluentResults;

namespace Instagram_Api.Application.Auth.Errors
{
    public class UserDoesNotExistError : IError
    {
        public List<IError> Reasons { get; set; } = new List<IError>();

        public string Message { get; set; } = "User guid provided does not match any user.";

        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
}
