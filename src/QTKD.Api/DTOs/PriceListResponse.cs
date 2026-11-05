using QTKD.Api.Entities;

namespace QTKD.Api.DTOs;

public record PriceListResponse(int Id, string Name, DateOnly EffectiveDate, List<PriceItemResponse> Items)
{
    public static PriceListResponse From(PriceList p) =>
        new(p.Id, p.Name, p.EffectiveDate,
            p.Items.OrderBy(i => i.ServiceCode).Select(PriceItemResponse.From).ToList());
}
