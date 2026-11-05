using System.ComponentModel.DataAnnotations;

namespace QTKD.Api.DTOs;

public class CreateContractRequest
{
    [Required(ErrorMessage = "ContractNo is required.")]
    [StringLength(30, ErrorMessage = "ContractNo must be at most 30 characters.")]
    public string ContractNo { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be a positive number.")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "StartDate is required.")]
    public DateOnly? StartDate { get; set; }

    [Required(ErrorMessage = "EndDate is required.")]
    public DateOnly? EndDate { get; set; }

    // Có thể để trống khi lập Draft, nhưng bắt buộc khi gửi trình duyệt (CTR-02)
    [StringLength(2000, ErrorMessage = "Content must be at most 2000 characters.")]
    public string? Content { get; set; }
}
