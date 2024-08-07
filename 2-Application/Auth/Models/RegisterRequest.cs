using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram_Api.Application.Auth.Models
{
    public record RegisterRequest(
        string FullName,
        string Email,
        string Password);
}
