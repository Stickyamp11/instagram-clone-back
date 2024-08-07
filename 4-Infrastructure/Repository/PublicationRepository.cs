using Application;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class PublicationRepository : IPublicationRepository
    {
        private readonly AppDbContext _dbContext;
        public PublicationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public PublicationRepository() { }

        public void Add(Publication publication)
        {
            _dbContext.Publications.Add(publication);
            _dbContext.SaveChanges();

        }

        public Publication GetPublicationByPublicationId(int publicationId)
        {
            return _dbContext.Publications.Where(pub => pub.Id.Value == publicationId).FirstOrDefault();
        }

        public List<Publication> GetPublicationsByUserGuid(string userGuid, int range_low, int range_max)
        {
            var user = _dbContext.Users.AsEnumerable().FirstOrDefault(u => u.Guid.Value == new Guid(userGuid));

            var queryForUser = _dbContext.Publications.Where(pub => pub.userId == user.Id);

            return queryForUser.Skip(range_low).Take(range_max - range_low).ToList();
        }

        public Task<int> GetTotalPublicationsByUserGuid(string userGuid)
        {
            var user = _dbContext.Users.AsEnumerable().FirstOrDefault(u => u.Guid.Value == new Guid(userGuid));

            return _dbContext.Publications.Where(pub => pub.userId == user.Id).CountAsync();
        }
    }
}
