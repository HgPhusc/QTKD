using Microsoft.EntityFrameworkCore;
using QTKD.Api.Data;
using QTKD.Api.Entities;

namespace QTKD.Api.Repositories;

public class CustomerRepository(QtkdDbContext db) : ICustomerRepository
{
    public Task<List<Customer>> GetAllAsync() =>
        db.Customers.AsNoTracking().OrderBy(c => c.Id).ToListAsync();

    // Được EF theo dõi để Update/tham chiếu từ Contract dùng tiếp
    public async Task<Customer?> GetByIdAsync(int id) => await db.Customers.FindAsync(id);

    public async Task AddAsync(Customer customer)
    {
        db.Customers.Add(customer);
        await db.SaveChangesAsync();
    }

    public Task UpdateAsync(Customer customer) => db.SaveChangesAsync();
}
