using System.ComponentModel.DataAnnotations;
using QTKD.Api.Entities;

namespace QTKD.Api.DTOs;

public class CreateCustomerRequest
{
    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(200, ErrorMessage = "Customer name must be at most 200 characters.")]
    public string Name { get; set; } = string.Empty;

    public CustomerStatus Status { get; set; } = CustomerStatus.Active;
}
