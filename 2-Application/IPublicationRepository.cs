using Domain.Entities;

namespace Application
{
    public interface IPublicationRepository
    {
        Publication GetPublicationByPublicationId(int publicationId);
        List<Publication> GetPublicationsByUserGuid(string userGuid, int range_low, int range_max);
        Task<int> GetTotalPublicationsByUserGuid(string userGuid);
        void Add(Publication publication);
    }
    
}
