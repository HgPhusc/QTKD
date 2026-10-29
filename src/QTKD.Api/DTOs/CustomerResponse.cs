using QTKD.Api.Entities;

namespace QTKD.Api.DTOs;

public record CustomerResponse(int Id, string Name, CustomerStatus Status)
{
    public static CustomerResponse From(Customer c) => new(c.Id, c.Name, c.Status);
}
