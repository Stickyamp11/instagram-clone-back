using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram_Api.Application.Auth.Errors
{
    public class DuplicateEmailError : IError
    {
        public List<IError> Reasons { get; set; } = new List<IError>();

        public string Message { get; set; } = "The email already exist.";

        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
}
