namespace Application
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userGuid, string fullName);
    }
    
}
