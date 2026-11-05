# Tài liệu API – Tuần 06

Base URL khi chạy local: `http://localhost:5000` (Swagger: `/swagger`).

## Mẫu phản hồi thống nhất

**Thành công:** trả trực tiếp đối tượng (hoặc danh sách) dạng JSON, không bọc thêm lớp ngoài.
Tạo mới trả `201 Created` kèm header `Location`; xem/cập nhật/gửi trả `200 OK`.

**Lỗi:** luôn là `application/problem+json` (RFC 7807):

```json
{
  "status": 400,
  "title": "Business rule violated",
  "detail": "Contract cannot be submitted: required information is missing or invalid.",
  "instance": "/api/contracts/2/submit",
  "rule": "CTR-02",
  "errors": [ "Contract content is required." ]
}
```

`rule` (mã quy tắc) và `errors` (danh sách điều kiện chưa thỏa) chỉ xuất hiện khi có.
Lỗi sai định dạng dữ liệu đầu vào (DTO) do ASP.NET trả `400` dạng `ValidationProblemDetails`, có trường `errors` theo từng field.

| Status | Ý nghĩa | Ví dụ |
|---|---|---|
| 200 | Thành công | Xem, cập nhật, gửi trình duyệt |
| 201 | Tạo mới thành công | Tạo Customer, Contract, PriceList, PriceItem |
| 400 | Dữ liệu sai định dạng hoặc vi phạm điều kiện nghiệp vụ | Thiếu Name, Credits/UnitPrice không hợp lệ, CTR-02, Customer không hợp lệ |
| 404 | Không tìm thấy | Contract, Customer, PriceList không tồn tại |
| 409 | Trạng thái hiện tại không cho phép, hoặc trùng dữ liệu | CTR-01, CTR-03, gửi lặp, trùng ContractNo / ServiceCode |
| 500 | Lỗi không lường trước | Không lộ chi tiết nội bộ |

## Danh sách endpoint

### Customer
| Method | Endpoint | Mô tả | Kết quả |
|---|---|---|---|
| GET | `/api/customers` | Danh sách | 200 |
| GET | `/api/customers/{id}` | Chi tiết | 200 / 404 |
| POST | `/api/customers` | Tạo | 201 / 400 |
| PUT | `/api/customers/{id}` | Cập nhật | 200 / 400 / 404 |

### Contract
| Method | Endpoint | Mô tả | Kết quả |
|---|---|---|---|
| GET | `/api/contracts?status=Draft` | Danh sách (lọc theo trạng thái, tùy chọn) | 200 |
| GET | `/api/contracts/{id}` | Chi tiết | 200 / 404 |
| POST | `/api/contracts` | Tạo hợp đồng (luôn ở Draft) | 201 / 400 / 409 |
| PUT | `/api/contracts/{id}` | Cập nhật (CTR-01) | 200 / 400 / 404 / 409 |
| POST | `/api/contracts/{id}/submit` | Gửi trình duyệt (US03, CTR-02, CTR-03) | 200 / 400 / 404 / 409 |

### Pricing
| Method | Endpoint | Mô tả | Kết quả |
|---|---|---|---|
| GET | `/api/pricelists` | Danh sách bảng giá (kèm item) | 200 |
| GET | `/api/pricelists/{id}` | Chi tiết bảng giá | 200 / 404 |
| POST | `/api/pricelists` | Tạo bảng giá | 201 / 400 |
| GET | `/api/pricelists/{id}/items` | Xem các PriceItem | 200 / 404 |
| POST | `/api/pricelists/{id}/items` | Thêm PriceItem | 201 / 400 / 404 / 409 |

## Request/response mẫu

### Tạo Customer
`POST /api/customers`
```json
{ "name": "Công ty Bình Minh", "status": "Active" }
```
`201 Created`
```json
{ "id": 3, "name": "Công ty Bình Minh", "status": "Active" }
```
Thiếu `name` → `400` (ValidationProblemDetails, "Customer name is required.").

### Tạo Contract (Draft, chưa có nội dung)
`POST /api/contracts`
```json
{ "contractNo": "HD-2026-019", "customerId": 3, "startDate": "2026-12-01", "endDate": "2027-11-30" }
```
`201 Created`
```json
{
  "id": 2, "contractNo": "HD-2026-019", "customerId": 3, "customerName": "Công ty Bình Minh",
  "startDate": "2026-12-01", "endDate": "2027-11-30", "status": "Draft",
  "content": null, "submittedAt": null
}
```
Lỗi: `customerId` không tồn tại hoặc không Active → `400`; `endDate` không sau `startDate` → `400`; trùng `contractNo` → `409`.

### Cập nhật Contract (bổ sung nội dung)
`PUT /api/contracts/2`
```json
{ "customerId": 3, "startDate": "2026-12-01", "endDate": "2027-11-30", "content": "Cung cấp dịch vụ trọn gói 12 tháng." }
```
`200 OK` (trả Contract đã cập nhật). Nếu Contract không ở Draft/RevisionRequested:
```json
{ "status": 409, "title": "Conflict", "rule": "CTR-01",
  "detail": "Contract in status 'PendingApproval' cannot be edited. Only Draft or RevisionRequested contracts can be edited." }
```
Gửi `"status": "Approved"` cho hợp đồng Draft:
```json
{ "status": 409, "title": "Conflict", "rule": "CTR-03", "detail": "Invalid status transition: Draft -> Approved." }
```

### Gửi trình duyệt – thất bại (thiếu nội dung)
`POST /api/contracts/2/submit` → `400 Bad Request`
```json
{ "status": 400, "title": "Business rule violated", "rule": "CTR-02",
  "detail": "Contract cannot be submitted: required information is missing or invalid.",
  "errors": [ "Contract content is required." ] }
```
Contract giữ nguyên: vẫn `Draft`, `submittedAt` = null.

### Gửi trình duyệt – thành công
`POST /api/contracts/2/submit` → `200 OK`
```json
{ "id": 2, "contractNo": "HD-2026-019", "status": "PendingApproval",
  "submittedAt": "2026-11-05T08:30:00Z", "...": "..." }
```

### Gửi lặp
`POST /api/contracts/2/submit` lần hai → `409 Conflict`, `rule: "US03"`,
"Contract has already been submitted and is waiting for approval." `submittedAt` giữ nguyên.

### Thêm PriceItem
`POST /api/pricelists/1/items`
```json
{ "serviceCode": "srv-new", "unitPrice": 320000 }
```
`201 Created` → `{ "priceListId": 1, "serviceCode": "SRV-NEW", "unitPrice": 320000 }`
(`serviceCode` được chuẩn hóa thành chữ hoa). `unitPrice` ≤ 0 → `400`; trùng `serviceCode` trong cùng bảng giá → `409`; bảng giá không tồn tại → `404`.
