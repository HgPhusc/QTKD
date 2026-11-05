# Luồng demo Tuần 06 (Swagger)

Chạy `dotnet run --project src\QTKD.Api`, mở `http://localhost:5000/swagger`.

| # | Thao tác | Kết quả mong đợi |
|---|---|---|
| 1 | `POST /api/customers` `{"name":"Công ty Bình Minh","status":"Active"}` | 201, `id` = 3 |
| 2 | `POST /api/contracts` `{"contractNo":"HD-2026-019","customerId":3,"startDate":"2026-12-01","endDate":"2027-11-30"}` | 201, `Draft`, `content` null (ghi lại `id` hợp đồng, thường là 2) |
| 3 | `POST /api/contracts/{id}/submit` (thiếu nội dung) | **400**, `rule: CTR-02`, `errors: ["Contract content is required."]` |
| 4 | `GET /api/contracts/{id}` | Vẫn `Draft`, `submittedAt` null |
| 5 | `PUT /api/contracts/{id}` thêm `"content": "Cung cấp dịch vụ trọn gói 12 tháng."` (giữ nguyên các trường còn lại) | 200 |
| 6 | `POST /api/contracts/{id}/submit` | **200**, `PendingApproval`, `submittedAt` có giá trị |
| 7 | `POST /api/contracts/{id}/submit` lần nữa | **409**, `submittedAt` không đổi |
| 8 | `PUT /api/contracts/{id}` | **409**, `rule: CTR-01` |
| 9 | `PUT /api/contracts/1` (hợp đồng Draft có sẵn) với `"status":"Approved"` | **409**, `rule: CTR-03` |
| 10 | `POST /api/pricelists/1/items` `{"serviceCode":"SRV-NEW","unitPrice":320000}` | 201; thử lại `unitPrice` 0 → 400; trùng mã → 409 |
| 11 | pgAdmin | Xem SQL bên dưới |

```sql
SELECT "Id", "ContractNo", "Status", "Content", "SubmittedAt" FROM "Contracts" ORDER BY "Id";
SELECT * FROM "Customers" ORDER BY "Id";
SELECT * FROM "PriceItems" ORDER BY "PriceListId", "ServiceCode";
```
