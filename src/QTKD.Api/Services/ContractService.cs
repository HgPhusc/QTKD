using QTKD.Api.DTOs;
using QTKD.Api.Repositories;

namespace QTKD.Api.Services;

// Tuần 05 mới chỉ đọc dữ liệu. Các quy tắc CTR-01..03 và thao tác submit sẽ thêm vào service này ở tuần 06.
public class ContractService(IContractRepository repo) : IContractService
{
    public async Task<ContractResponse?> GetByIdAsync(int id)
    {
        var contract = await repo.GetByIdAsync(id);
        return contract is null ? null : ContractResponse.From(contract);
    }
}
