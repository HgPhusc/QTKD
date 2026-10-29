namespace QTKD.Api.Entities;

// Giả định theo sơ đồ trạng thái Phase 1 – cần đối chiếu lại với bản đã xác nhận.
public enum ContractStatus
{
    Draft,
    RevisionRequested,
    PendingApproval,
    Approved
}
