using QTKD.Api.DTOs;
using QTKD.Api.Entities;

namespace QTKD.Api.Services;

public interface IContractService
{
    Task<List<ContractResponse>> GetAllAsync(ContractStatus? status);
    Task<ContractResponse> GetByIdAsync(int id);
    Task<ContractResponse> CreateAsync(CreateContractRequest request);
    Task<ContractResponse> UpdateAsync(int id, UpdateContractRequest request);
    Task<ContractResponse> SubmitAsync(int id);
}
