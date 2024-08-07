using Domain.Entities;

namespace Application
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        User GetUserByGuid(string guid);

        void Add(User user);
    }
    
}
