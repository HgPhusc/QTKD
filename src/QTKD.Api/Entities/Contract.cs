namespace QTKD.Api.Entities;

public class Contract
{
    public int Id { get; set; }
    public string ContractNo { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public ContractStatus Status { get; set; } = ContractStatus.Draft;

    // Tuần 06: nội dung hợp đồng (giả định là nội dung bắt buộc để gửi trình duyệt – CTR-02)
    public string? Content { get; set; }
    // Tuần 06: thời điểm gửi trình duyệt (mô phỏng việc gửi)
    public DateTime? SubmittedAt { get; set; }

    public Customer? Customer { get; set; }
}
