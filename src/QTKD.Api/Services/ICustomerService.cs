using QTKD.Api.DTOs;

namespace QTKD.Api.Services;

public interface ICustomerService
{
    Task<List<CustomerResponse>> GetAllAsync();
}
