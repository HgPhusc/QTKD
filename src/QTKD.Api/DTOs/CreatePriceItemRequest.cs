using System.ComponentModel.DataAnnotations;

namespace QTKD.Api.DTOs;

public class CreatePriceItemRequest
{
    [Required(ErrorMessage = "ServiceCode is required.")]
    [StringLength(30, ErrorMessage = "ServiceCode must be at most 30 characters.")]
    public string ServiceCode { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "999999999999.99", ErrorMessage = "UnitPrice must be greater than 0.")]
    public decimal UnitPrice { get; set; }
}
