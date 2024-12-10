using Bayanihand.DataModel;
using Bayanihand.Repository.Repository;

namespace Bayanihand.App.Models.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(AppDbContext context) : base(context)
        {
        }
    }
}
