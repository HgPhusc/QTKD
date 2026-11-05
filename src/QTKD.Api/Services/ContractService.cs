using QTKD.Api.DTOs;
using QTKD.Api.Entities;
using QTKD.Api.Exceptions;
using QTKD.Api.Repositories;

namespace QTKD.Api.Services;

public class ContractService(IContractRepository contracts, ICustomerRepository customers) : IContractService
{
    public async Task<List<ContractResponse>> GetAllAsync(ContractStatus? status)
    {
        var list = await contracts.GetAllAsync(status);
        return list.Select(ContractResponse.From).ToList();
    }

    public async Task<ContractResponse> GetByIdAsync(int id)
    {
        var contract = await contracts.GetByIdAsync(id)
            ?? throw new NotFoundException($"Contract {id} not found.");
        return ContractResponse.From(contract);
    }

    public async Task<ContractResponse> CreateAsync(CreateContractRequest request)
    {
        var contractNo = request.ContractNo.Trim();
        EnsureValidDates(request.StartDate!.Value, request.EndDate!.Value);
        var customer = await GetUsableCustomerAsync(request.CustomerId);

        if (await contracts.ContractNoExistsAsync(contractNo))
            throw new ConflictException($"Contract number '{contractNo}' already exists.");

        var contract = new Contract
        {
            ContractNo = contractNo,
            Customer = customer,
            StartDate = request.StartDate!.Value,
            EndDate = request.EndDate!.Value,
            Content = Normalize(request.Content),
            Status = ContractStatus.Draft
        };
        await contracts.AddAsync(contract);
        return ContractResponse.From(contract);
    }

    public async Task<ContractResponse> UpdateAsync(int id, UpdateContractRequest request)
    {
        var contract = await contracts.GetByIdForUpdateAsync(id)
            ?? throw new NotFoundException($"Contract {id} not found.");

        // CTR-01: chỉ sửa khi Draft hoặc RevisionRequested. Kiểm tra TRƯỚC mọi thay đổi để dữ liệu giữ nguyên khi bị từ chối.
        if (!ContractStateMachine.IsEditable(contract.Status))
            throw new ConflictException(
                $"Contract in status '{contract.Status}' cannot be edited. Only Draft or RevisionRequested contracts can be edited.",
                "CTR-01");

        // Không đổi trạng thái qua Update.
        // Bước không hợp lệ (ví dụ Draft -> Approved) bị từ chối bởi CTR-03; bước hợp lệ phải dùng /submit.
        if (request.Status.HasValue && request.Status.Value != contract.Status)
        {
            ContractStateMachine.EnsureCanTransition(contract.Status, request.Status.Value);
            throw new BusinessRuleException(
                "Status cannot be changed through update. Use POST /api/contracts/{id}/submit.");
        }

        EnsureValidDates(request.StartDate!.Value, request.EndDate!.Value);

        if (request.CustomerId != contract.CustomerId)
        {
            var customer = await GetUsableCustomerAsync(request.CustomerId);
            contract.Customer = customer;
            contract.CustomerId = customer.Id;
        }

        contract.StartDate = request.StartDate!.Value;
        contract.EndDate = request.EndDate!.Value;
        contract.Content = Normalize(request.Content);

        await contracts.UpdateAsync(contract);
        return ContractResponse.From(contract);
    }

    // US03 – Gửi hợp đồng trình duyệt
    public async Task<ContractResponse> SubmitAsync(int id)
    {
        var contract = await contracts.GetByIdForUpdateAsync(id)
            ?? throw new NotFoundException($"Contract {id} not found.");

        // Gửi lặp: không tạo thêm lần xử lý nào, không đổi dữ liệu.
        if (contract.Status == ContractStatus.PendingApproval)
            throw new ConflictException(
                "Contract has already been submitted and is waiting for approval.", "US03");

        if (contract.Status != ContractStatus.Draft)
            throw new ConflictException(
                $"Only Draft contracts can be submitted (current status: {contract.Status}).", "US03");

        // CTR-02: Customer, ngày hiệu lực, nội dung bắt buộc phải hợp lệ. Gom toàn bộ lỗi để báo một lần.
        var errors = new List<string>();
        if (contract.Customer is null || contract.Customer.Status != CustomerStatus.Active)
            errors.Add("Customer must exist and be Active.");
        if (contract.StartDate >= contract.EndDate)
            errors.Add("EndDate must be later than StartDate.");
        if (string.IsNullOrWhiteSpace(contract.Content))
            errors.Add("Contract content is required.");

        if (errors.Count > 0)
            throw new BusinessRuleException(
                "Contract cannot be submitted: required information is missing or invalid.",
                "CTR-02", errors);

        // CTR-03: chỉ đi qua bước chuyển hợp lệ Draft -> PendingApproval (không bao giờ nhảy thẳng sang Approved).
        ContractStateMachine.EnsureCanTransition(contract.Status, ContractStatus.PendingApproval);
        contract.Status = ContractStatus.PendingApproval;
        contract.SubmittedAt = DateTime.UtcNow;

        await contracts.UpdateAsync(contract);
        return ContractResponse.From(contract);
    }

    private async Task<Customer> GetUsableCustomerAsync(int customerId)
    {
        var customer = await customers.GetByIdAsync(customerId)
            ?? throw new BusinessRuleException($"Customer {customerId} does not exist.");
        if (customer.Status != CustomerStatus.Active)
            throw new BusinessRuleException($"Customer {customerId} is not Active.");
        return customer;
    }

    private static void EnsureValidDates(DateOnly start, DateOnly end)
    {
        if (start >= end)
            throw new BusinessRuleException("EndDate must be later than StartDate.");
    }

    private static string? Normalize(string? text) =>
        string.IsNullOrWhiteSpace(text) ? null : text.Trim();
}
