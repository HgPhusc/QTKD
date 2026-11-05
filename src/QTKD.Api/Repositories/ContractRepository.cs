using Microsoft.EntityFrameworkCore;
using QTKD.Api.Data;
using QTKD.Api.Entities;

namespace QTKD.Api.Repositories;

public class ContractRepository(QtkdDbContext db) : IContractRepository
{
    public Task<List<Contract>> GetAllAsync(ContractStatus? status)
    {
        var query = db.Contracts.AsNoTracking().Include(c => c.Customer).AsQueryable();
        if (status.HasValue) query = query.Where(c => c.Status == status.Value);
        return query.OrderBy(c => c.Id).ToListAsync();
    }

    public Task<Contract?> GetByIdAsync(int id) =>
        db.Contracts.AsNoTracking()
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(c => c.Id == id);

    public Task<Contract?> GetByIdForUpdateAsync(int id) =>
        db.Contracts
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(c => c.Id == id);

    public Task<bool> ContractNoExistsAsync(string contractNo) =>
        db.Contracts.AnyAsync(c => c.ContractNo == contractNo);

    public async Task AddAsync(Contract contract)
    {
        db.Contracts.Add(contract);
        await db.SaveChangesAsync();
    }

    public Task UpdateAsync(Contract contract) => db.SaveChangesAsync();
}
