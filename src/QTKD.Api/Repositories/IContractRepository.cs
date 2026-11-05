using QTKD.Api.Entities;

namespace QTKD.Api.Repositories;

public interface IContractRepository
{
    Task<List<Contract>> GetAllAsync(ContractStatus? status);
    Task<Contract?> GetByIdAsync(int id);              // chỉ đọc
    Task<Contract?> GetByIdForUpdateAsync(int id);     // được EF theo dõi, dùng khi sửa/gửi
    Task<bool> ContractNoExistsAsync(string contractNo);
    Task AddAsync(Contract contract);
    Task UpdateAsync(Contract contract);
}
