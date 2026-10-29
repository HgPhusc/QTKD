using Microsoft.EntityFrameworkCore;
using QTKD.Api.Data;
using QTKD.Api.Entities;

namespace QTKD.Api.Repositories;

public class ContractRepository(QtkdDbContext db) : IContractRepository
{
    public Task<Contract?> GetByIdAsync(int id) =>
        db.Contracts.AsNoTracking()
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(c => c.Id == id);
}
