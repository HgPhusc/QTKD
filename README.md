# QTKD – Hệ thống Quản trị kinh doanh (Phase 2, Tuần 05)

Web API ASP.NET Core + EF Core + PostgreSQL. Tuần 05: kết nối database và đọc dữ liệu
Customer, Contract (kèm PriceList/PriceItem trong database). Tuần 06 và 07 tiếp tục trên cùng repository này.

Cấu trúc: `src/` (mã nguồn), `tests/` (kiểm thử, bổ sung ở tuần 07), `docs/` (thiết kế), `README.md`.
Sơ đồ thành phần và mô hình dữ liệu: [docs/architecture.md](docs/architecture.md).

## Yêu cầu
- .NET SDK 10, PostgreSQL, Git
- Công cụ EF: `dotnet tool install --global dotnet-ef` (nếu chưa có)

## Cấu hình database
1. Tạo database rỗng tên `qtkd` trong PostgreSQL (pgAdmin hoặc `CREATE DATABASE qtkd;`).
2. Đặt connection string (có mật khẩu) bằng user-secrets, **không ghi vào file trong Git**:
   ```
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=qtkd;Username=postgres;Password=<mat_khau>" --project src/QTKD.Api
   ```

## Chạy migration
```
dotnet ef database update --project src/QTKD.Api
```
Lệnh này tạo bảng Customers, Contracts, PriceLists, PriceItems và thêm dữ liệu mẫu
(2 Customer, 1 Contract Draft `HD-2026-018`, 1 PriceList có 2 PriceItem).

## Khởi động API
```
dotnet run --project src/QTKD.Api
```
Mở Swagger tại `http://localhost:<port>/swagger` (port in ra trong terminal).

## Thử hai endpoint
| Method | Endpoint | Kết quả mong đợi |
|---|---|---|
| GET | `/api/customers` | 200, danh sách 2 khách hàng |
| GET | `/api/contracts/1` | 200, hợp đồng `HD-2026-018` trạng thái `Draft` |
| GET | `/api/contracts/999` | 404 |

Đối chiếu với database:
```sql
SELECT * FROM "Customers";
SELECT c.*, cu."Name" FROM "Contracts" c JOIN "Customers" cu ON cu."Id" = c."CustomerId" WHERE c."Id" = 1;
SELECT * FROM "PriceItems";
```
