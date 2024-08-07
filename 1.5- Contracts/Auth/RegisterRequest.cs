namespace Instagram_Api.Contracts.Auth
{
    public record RegisterRequest(
        string FullName,
        string Email,
        string Password);
}
