using Bayanihand.DataModel;
using Bayanihand.Repository.Repository;

namespace Bayanihand.App.Models.Repositories
{
    public class HandymanRepository : GenericRepository<Handyman>, IHandymanRepository
    {
        public HandymanRepository(AppDbContext context) : base(context)
        {
        }
    }
}
