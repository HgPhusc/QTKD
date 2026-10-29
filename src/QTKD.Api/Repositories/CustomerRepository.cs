using Microsoft.EntityFrameworkCore;
using QTKD.Api.Data;
using QTKD.Api.Entities;

namespace QTKD.Api.Repositories;

public class CustomerRepository(QtkdDbContext db) : ICustomerRepository
{
    public Task<List<Customer>> GetAllAsync() =>
        db.Customers.AsNoTracking().OrderBy(c => c.Id).ToListAsync();
}
