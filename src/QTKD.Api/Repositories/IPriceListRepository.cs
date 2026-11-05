using QTKD.Api.Entities;

namespace QTKD.Api.Repositories;

public interface IPriceListRepository
{
    Task<List<PriceList>> GetAllAsync();
    Task<PriceList?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<List<PriceItem>> GetItemsAsync(int priceListId);
    Task<bool> ItemExistsAsync(int priceListId, string serviceCode);
    Task AddAsync(PriceList priceList);
    Task AddItemAsync(PriceItem item);
}
