using System.ComponentModel.DataAnnotations;
using QTKD.Api.Entities;

namespace QTKD.Api.DTOs;

// ContractNo không được sửa sau khi tạo.
public class UpdateContractRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be a positive number.")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "StartDate is required.")]
    public DateOnly? StartDate { get; set; }

    [Required(ErrorMessage = "EndDate is required.")]
    public DateOnly? EndDate { get; set; }

    [StringLength(2000, ErrorMessage = "Content must be at most 2000 characters.")]
    public string? Content { get; set; }

    // Không dùng để đổi trạng thái. Nếu gửi giá trị khác trạng thái hiện tại thì bị từ chối
    // (bước chuyển không hợp lệ -> CTR-03; bước hợp lệ phải đi qua /submit).
    public ContractStatus? Status { get; set; }
}
