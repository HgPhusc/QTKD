using QTKD.Api.DTOs;

namespace QTKD.Api.Services;

public interface IPriceListService
{
    Task<List<PriceListResponse>> GetAllAsync();
    Task<PriceListResponse> GetByIdAsync(int id);
    Task<PriceListResponse> CreateAsync(CreatePriceListRequest request);
    Task<List<PriceItemResponse>> GetItemsAsync(int priceListId);
    Task<PriceItemResponse> CreateItemAsync(int priceListId, CreatePriceItemRequest request);
}
