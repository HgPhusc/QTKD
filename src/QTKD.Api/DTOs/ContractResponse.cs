using QTKD.Api.Entities;

namespace QTKD.Api.DTOs;

public record ContractResponse(
    int Id, string ContractNo, int CustomerId, string CustomerName,
    DateOnly StartDate, DateOnly EndDate, ContractStatus Status,
    string? Content, DateTime? SubmittedAt)
{
    public static ContractResponse From(Contract c) =>
        new(c.Id, c.ContractNo, c.CustomerId, c.Customer?.Name ?? string.Empty,
            c.StartDate, c.EndDate, c.Status, c.Content, c.SubmittedAt);
}
