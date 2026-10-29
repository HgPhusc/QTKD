namespace QTKD.Api.Entities;

// Khóa chính kép (PriceListId, ServiceCode): một mã dịch vụ chỉ xuất hiện một lần trong một bảng giá.
public class PriceItem
{
    public int PriceListId { get; set; }
    public string ServiceCode { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }

    public PriceList? PriceList { get; set; }
}
