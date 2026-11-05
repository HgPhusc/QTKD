using QTKD.Api.DTOs;
using QTKD.Api.Entities;
using QTKD.Api.Exceptions;
using QTKD.Api.Repositories;

namespace QTKD.Api.Services;

public class PriceListService(IPriceListRepository repo) : IPriceListService
{
    public async Task<List<PriceListResponse>> GetAllAsync()
    {
        var lists = await repo.GetAllAsync();
        return lists.Select(PriceListResponse.From).ToList();
    }

    public async Task<PriceListResponse> GetByIdAsync(int id)
    {
        var list = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Price list {id} not found.");
        return PriceListResponse.From(list);
    }

    public async Task<PriceListResponse> CreateAsync(CreatePriceListRequest request)
    {
        var list = new PriceList { Name = request.Name.Trim(), EffectiveDate = request.EffectiveDate!.Value };
        await repo.AddAsync(list);
        return PriceListResponse.From(list);
    }

    public async Task<List<PriceItemResponse>> GetItemsAsync(int priceListId)
    {
        if (!await repo.ExistsAsync(priceListId))
            throw new NotFoundException($"Price list {priceListId} not found.");
        var items = await repo.GetItemsAsync(priceListId);
        return items.Select(PriceItemResponse.From).ToList();
    }

    public async Task<PriceItemResponse> CreateItemAsync(int priceListId, CreatePriceItemRequest request)
    {
        // PriceItem phải gắn vào một PriceList có thật
        if (!await repo.ExistsAsync(priceListId))
            throw new NotFoundException($"Price list {priceListId} not found.");

        var code = request.ServiceCode.Trim().ToUpperInvariant();
        if (await repo.ItemExistsAsync(priceListId, code))
            throw new ConflictException($"Service code '{code}' already exists in price list {priceListId}.");

        var item = new PriceItem { PriceListId = priceListId, ServiceCode = code, UnitPrice = request.UnitPrice };
        await repo.AddItemAsync(item);
        return PriceItemResponse.From(item);
    }
}
