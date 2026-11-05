using System.ComponentModel.DataAnnotations;

namespace QTKD.Api.DTOs;

public class CreatePriceListRequest
{
    [Required(ErrorMessage = "Price list name is required.")]
    [StringLength(200, ErrorMessage = "Price list name must be at most 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "EffectiveDate is required.")]
    public DateOnly? EffectiveDate { get; set; }
}
