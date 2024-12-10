using Bayanihand.DataModel;
using Bayanihand.Repository.Repository;

namespace Bayanihand.App.Models.Repositories
{
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(AppDbContext context) : base(context)
        {
        }
    }
}
