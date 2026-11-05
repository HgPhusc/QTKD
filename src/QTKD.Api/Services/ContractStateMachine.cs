using QTKD.Api.Entities;
using QTKD.Api.Exceptions;

namespace QTKD.Api.Services;

// Nơi duy nhất định nghĩa các bước chuyển trạng thái hợp lệ.
// CTR-01: chỉ Draft/RevisionRequested được sửa.
// CTR-03: không cho chuyển Draft thẳng sang Approved (và mọi bước không có trong bảng).
public static class ContractStateMachine
{
    private static readonly Dictionary<ContractStatus, ContractStatus[]> Allowed = new()
    {
        [ContractStatus.Draft] = new[] { ContractStatus.PendingApproval },
        [ContractStatus.RevisionRequested] = new[] { ContractStatus.PendingApproval },
        [ContractStatus.PendingApproval] = new[] { ContractStatus.Approved, ContractStatus.RevisionRequested },
        [ContractStatus.Approved] = Array.Empty<ContractStatus>()
    };

    public static bool IsEditable(ContractStatus status) =>
        status is ContractStatus.Draft or ContractStatus.RevisionRequested;

    public static bool CanTransition(ContractStatus from, ContractStatus to) =>
        Allowed[from].Contains(to);

    public static void EnsureCanTransition(ContractStatus from, ContractStatus to)
    {
        if (!CanTransition(from, to))
            throw new ConflictException($"Invalid status transition: {from} -> {to}.", "CTR-03");
    }
}
