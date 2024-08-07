using Application;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        public UserRepository(AppDbContext dbContext) {
            _dbContext = dbContext;
        }
        public void Add(User user)
        {
            _dbContext.Add(user);
            _dbContext.SaveChanges();
        }

        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserByGuid(string guid)
        {
            return _dbContext.Users.AsEnumerable()
                .SingleOrDefault(u => u.Guid.Value.ToString() == guid);
        }
    }
}
