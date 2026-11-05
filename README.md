# QTKD – Hệ thống Quản trị kinh doanh (Phase 2)

Web API ASP.NET Core + EF Core + PostgreSQL.
- **Tuần 05:** kết nối database, đọc Customer/Contract.
- **Tuần 06:** tạo/sửa Customer, Contract, PriceList/PriceItem; gửi hợp đồng trình duyệt (US03) với các quy tắc CTR-01..03.
- **Tuần 07:** kiểm thử (thư mục `tests/`).

Cấu trúc: `src/` (mã nguồn), `tests/` (kiểm thử), `docs/` (thiết kế, API, minh chứng), `README.md`.

## Tài liệu
- [docs/architecture.md](docs/architecture.md) – thành phần và mô hình dữ liệu
- [docs/api.md](docs/api.md) – danh sách API, request/response mẫu, mẫu phản hồi lỗi
- [docs/us03-rules.md](docs/us03-rules.md) – US03, CTR-01..03 và bảng đối chiếu với code
- [docs/demo-week06.md](docs/demo-week06.md) – luồng demo

## Yêu cầu
- .NET SDK 10, PostgreSQL, Git
- Công cụ EF: `dotnet tool install --global dotnet-ef` (nếu chưa có)

## Cấu hình database
1. Tạo database rỗng tên `qtkd` trong PostgreSQL.
2. Đặt connection string (có mật khẩu) bằng user-secrets, **không ghi vào file trong Git**:
   ```
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=qtkd;Username=postgres;Password=<mat_khau>" --project src/QTKD.Api
   ```

## Chạy migration
```
dotnet ef database update --project src/QTKD.Api
```
Tạo bảng, dữ liệu mẫu (2 Customer, Contract `HD-2026-018` Draft, 1 PriceList có 2 PriceItem) và các cột mới của Tuần 06 (`Content`, `SubmittedAt`).

## Khởi động API
```
dotnet run --project src/QTKD.Api
```
Mở `http://localhost:5000/swagger` (cần môi trường Development; đã cấu hình trong `Properties/launchSettings.json`).

## Endpoint chính
Xem đầy đủ trong [docs/api.md](docs/api.md).

| Nhóm | Endpoint |
|---|---|
| Customer | `GET/POST /api/customers`, `GET/PUT /api/customers/{id}` |
| Contract | `GET/POST /api/contracts`, `GET/PUT /api/contracts/{id}`, `POST /api/contracts/{id}/submit` |
| Pricing | `GET/POST /api/pricelists`, `GET /api/pricelists/{id}`, `GET/POST /api/pricelists/{id}/items` |

Lỗi trả dạng ProblemDetails có `rule` và `errors` khi vi phạm quy tắc nghiệp vụ.
