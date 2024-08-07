using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class User
    {
        public UserId Id {  get; set; }
        public UserGuid Guid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }

        [JsonIgnore]
        public ICollection<Publication> Publications { get; }
    }

    public record UserId {
        public UserId(int value)
        {
            Value = value;
        }
        public int Value { get; set; }
    };

    public record UserGuid {
        public UserGuid(Guid value)
        {
            Value = value;
        }
        public Guid Value { get; set; }
    }


}
