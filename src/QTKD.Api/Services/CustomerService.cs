using QTKD.Api.DTOs;
using QTKD.Api.Entities;
using QTKD.Api.Exceptions;
using QTKD.Api.Repositories;

namespace QTKD.Api.Services;

public class CustomerService(ICustomerRepository repo) : ICustomerService
{
    public async Task<List<CustomerResponse>> GetAllAsync()
    {
        var customers = await repo.GetAllAsync();
        return customers.Select(CustomerResponse.From).ToList();
    }

    public async Task<CustomerResponse> GetByIdAsync(int id)
    {
        var customer = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Customer {id} not found.");
        return CustomerResponse.From(customer);
    }

    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request)
    {
        var customer = new Customer { Name = request.Name.Trim(), Status = request.Status };
        await repo.AddAsync(customer);
        return CustomerResponse.From(customer);
    }

    public async Task<CustomerResponse> UpdateAsync(int id, UpdateCustomerRequest request)
    {
        var customer = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Customer {id} not found.");
        customer.Name = request.Name.Trim();
        customer.Status = request.Status;
        await repo.UpdateAsync(customer);
        return CustomerResponse.From(customer);
    }
}
