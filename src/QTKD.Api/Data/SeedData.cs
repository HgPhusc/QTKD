using Microsoft.EntityFrameworkCore;
using QTKD.Api.Entities;

namespace QTKD.Api.Data;

// Dữ liệu mẫu được ghi vào database bởi migration (HasData), API chỉ đọc lại từ database.
public static class SeedData
{
    public static void SeedSampleData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, Name = "Công ty Minh Phát", Status = CustomerStatus.Active },
            new Customer { Id = 2, Name = "Công ty An Khang", Status = CustomerStatus.Active });

        modelBuilder.Entity<Contract>().HasData(
            new Contract
            {
                Id = 1,
                ContractNo = "HD-2026-018",
                CustomerId = 1,
                StartDate = new DateOnly(2026, 11, 1),
                EndDate = new DateOnly(2027, 10, 31),
                Status = ContractStatus.Draft
            });

        modelBuilder.Entity<PriceList>().HasData(
            new PriceList { Id = 1, Name = "Bảng giá dịch vụ 2026", EffectiveDate = new DateOnly(2026, 1, 1) });

        modelBuilder.Entity<PriceItem>().HasData(
            new PriceItem { PriceListId = 1, ServiceCode = "SRV-STD", UnitPrice = 150000m },
            new PriceItem { PriceListId = 1, ServiceCode = "SRV-PRM", UnitPrice = 275000m });
    }
}
