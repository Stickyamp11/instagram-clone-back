using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Publication
    {
        public PublicationId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserId userId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}

public record PublicationId {
    public PublicationId(int value)
    {
        Value = value;
    }
    public int Value { get; set;}
}