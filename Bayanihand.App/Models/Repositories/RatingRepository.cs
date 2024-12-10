using Bayanihand.DataModel;
using Bayanihand.Repository.Repository;

namespace Bayanihand.App.Models.Repositories
{
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        public RatingRepository(AppDbContext context) : base(context)
        {
        }
    }
}
