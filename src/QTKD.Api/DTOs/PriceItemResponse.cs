using QTKD.Api.Entities;

namespace QTKD.Api.DTOs;

public record PriceItemResponse(int PriceListId, string ServiceCode, decimal UnitPrice)
{
    public static PriceItemResponse From(PriceItem i) => new(i.PriceListId, i.ServiceCode, i.UnitPrice);
}
