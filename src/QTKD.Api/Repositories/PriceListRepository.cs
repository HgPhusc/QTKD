using Microsoft.EntityFrameworkCore;
using QTKD.Api.Data;
using QTKD.Api.Entities;

namespace QTKD.Api.Repositories;

public class PriceListRepository(QtkdDbContext db) : IPriceListRepository
{
    public Task<List<PriceList>> GetAllAsync() =>
        db.PriceLists.AsNoTracking().Include(p => p.Items).OrderBy(p => p.Id).ToListAsync();

    public Task<PriceList?> GetByIdAsync(int id) =>
        db.PriceLists.AsNoTracking().Include(p => p.Items).FirstOrDefaultAsync(p => p.Id == id);

    public Task<bool> ExistsAsync(int id) => db.PriceLists.AnyAsync(p => p.Id == id);

    public Task<List<PriceItem>> GetItemsAsync(int priceListId) =>
        db.PriceItems.AsNoTracking()
            .Where(i => i.PriceListId == priceListId)
            .OrderBy(i => i.ServiceCode)
            .ToListAsync();

    public Task<bool> ItemExistsAsync(int priceListId, string serviceCode) =>
        db.PriceItems.AnyAsync(i => i.PriceListId == priceListId && i.ServiceCode == serviceCode);

    public async Task AddAsync(PriceList priceList)
    {
        db.PriceLists.Add(priceList);
        await db.SaveChangesAsync();
    }

    public async Task AddItemAsync(PriceItem item)
    {
        db.PriceItems.Add(item);
        await db.SaveChangesAsync();
    }
}
