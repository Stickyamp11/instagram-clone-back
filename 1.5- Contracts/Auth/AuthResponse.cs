namespace Instagram_Api.Contracts.Auth
{
   public record AuthResponse(
        Guid UserGuid,
        string Token);

}
