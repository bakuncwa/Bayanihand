using Bayanihand.DataModel;
using Bayanihand.Repository.Repository;
using Bayanihand.Repository.Repository;
using System;

namespace Bayanihand.App.Models.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context) { 
        }
    }
}
