using QTKD.Api.DTOs;
using QTKD.Api.Repositories;

namespace QTKD.Api.Services;

public class CustomerService(ICustomerRepository repo) : ICustomerService
{
    public async Task<List<CustomerResponse>> GetAllAsync()
    {
        var customers = await repo.GetAllAsync();
        return customers.Select(CustomerResponse.From).ToList();
    }
}
