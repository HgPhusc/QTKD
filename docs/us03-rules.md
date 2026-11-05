# US03 – Gửi hợp đồng trình duyệt và các quy tắc CTR-01..03

> **Cần đối chiếu:** nội dung dưới đây được viết theo mô tả trong đề thực hành Tuần 06.
> Nếu US03, Acceptance Criteria hoặc sơ đồ trạng thái đã xác nhận ở Phase 1 khác, hãy sửa file này và ghi lý do.

## 1. User Story (theo đề)
Là Nhân viên Kinh doanh, tôi muốn gửi hợp đồng đã lập để trình duyệt, sau khi đã bổ sung đủ thông tin, để hợp đồng được chuyển sang bước duyệt.

## 2. Điều kiện để gửi (CTR-02)
Hợp đồng chỉ được gửi khi đồng thời thỏa:
1. Hợp đồng đang ở trạng thái **Draft**.
2. **Customer** tồn tại và đang **Active**.
3. **Ngày hiệu lực hợp lệ:** `EndDate` sau `StartDate`.
4. **Nội dung hợp đồng** (`Content`) không rỗng.

## 3. Trạng thái trước và sau
| Trường hợp | Trạng thái trước | Kết quả | Trạng thái sau |
|---|---|---|---|
| Gửi hợp lệ | Draft | 200 | **PendingApproval**, ghi `SubmittedAt` |
| Thiếu/sai điều kiện CTR-02 | Draft | 400, kèm danh sách lỗi | Draft, dữ liệu không đổi |
| Gửi lại hợp đồng đã gửi | PendingApproval | 409 | PendingApproval, `SubmittedAt` không đổi |
| Gửi hợp đồng ở trạng thái khác Draft | RevisionRequested / Approved | 409 | Không đổi |
| Hợp đồng không tồn tại | – | 404 | – |

## 4. Các trường hợp phải từ chối
- Thiếu một điều kiện CTR-02 → từ chối, báo **tất cả** điều kiện chưa thỏa trong một lần (`errors`), không lưu gì.
- Đang PendingApproval → từ chối (gửi lặp).
- Không bao giờ chuyển Draft thẳng sang Approved (CTR-03).

## 5. Cách mô phỏng việc gửi và giới hạn của phiên bản hiện tại
- **Mô phỏng:** "gửi" = chuyển `Draft → PendingApproval` và ghi `SubmittedAt` (UTC) trên Contract. Chưa có người duyệt, hàng đợi hay thông báo.
- **Không tạo lần xử lý trùng:** lần gửi thứ hai bị chặn bởi kiểm tra trạng thái nên không đổi dữ liệu.
- **Giới hạn:**
  - Chưa có module Approval: chưa có API duyệt/yêu cầu sửa lại; trạng thái `Approved`, `RevisionRequested` mới chỉ nằm trong bảng chuyển trạng thái.
  - Hai request gửi **đồng thời** có thể cùng vượt qua kiểm tra trạng thái (chưa dùng khóa lạc quan/`xmin`). Có thể bổ sung ở tuần 07 nếu cần.
  - Chưa có tệp đính kèm; "nội dung bắt buộc" mô phỏng bằng trường `Content`.

## 6. Bảng đối chiếu US03 / CTR với đoạn xử lý

| Mã | Nội dung quy tắc | Nơi xử lý | Cách kiểm chứng |
|---|---|---|---|
| US03 | Gửi hợp đồng trình duyệt | `ContractsController.Submit` → `ContractService.SubmitAsync` | Demo: thiếu nội dung → 400; bổ sung → 200 `PendingApproval` |
| CTR-01 | Chỉ sửa khi Draft hoặc RevisionRequested | `ContractService.UpdateAsync` (gọi `ContractStateMachine.IsEditable`), kiểm tra trước mọi thay đổi | `PUT` hợp đồng PendingApproval → 409 `CTR-01`, dữ liệu giữ nguyên |
| CTR-02 | Chỉ gửi khi Customer, ngày hiệu lực, nội dung hợp lệ | `ContractService.SubmitAsync` (khối gom `errors`) | Thiếu nội dung → 400 `CTR-02`, vẫn Draft |
| CTR-03 | Không chuyển Draft thẳng Approved | `ContractStateMachine.EnsureCanTransition`; được gọi từ `SubmitAsync` và `UpdateAsync` | `PUT` với `"status":"Approved"` trên Draft → 409 `CTR-03`; submit chỉ ra `PendingApproval` |

## 7. Giả định cần xác nhận với Phase 1
- "Nội dung/tệp bắt buộc" = trường `Content` không rỗng (chưa có danh sách tệp).
- "Ngày hiệu lực hợp lệ" = `EndDate` sau `StartDate`.
- Bảng chuyển trạng thái: Draft → PendingApproval; RevisionRequested → PendingApproval; PendingApproval → Approved/RevisionRequested; Approved không chuyển tiếp.
- Chỉ hợp đồng **Draft** được gửi (RevisionRequested gửi lại sẽ làm khi có module Approval).
- `ContractNo` không sửa được sau khi tạo.
