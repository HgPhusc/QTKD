namespace QTKD.Api.Entities;

public class Contract
{
    public int Id { get; set; }
    public string ContractNo { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public ContractStatus Status { get; set; } = ContractStatus.Draft;

    public Customer? Customer { get; set; }
}
