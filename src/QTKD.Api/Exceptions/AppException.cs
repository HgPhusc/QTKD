namespace QTKD.Api.Exceptions;

// Exception nghiệp vụ: Service ném ra, GlobalExceptionHandler đổi thành HTTP response thống nhất.
public abstract class AppException : Exception
{
    public int StatusCode { get; }
    public string? Rule { get; }                    // ví dụ "CTR-01"
    public IReadOnlyList<string> Errors { get; }    // danh sách điều kiện chưa thỏa

    protected AppException(string message, int statusCode, string? rule = null, IEnumerable<string>? errors = null)
        : base(message)
    {
        StatusCode = statusCode;
        Rule = rule;
        Errors = errors?.ToList() ?? new List<string>();
    }
}

// 404: không tìm thấy đối tượng
public class NotFoundException(string message)
    : AppException(message, StatusCodes.Status404NotFound);

// 409: trạng thái hiện tại không cho phép thao tác (CTR-01, CTR-03, gửi lặp, trùng dữ liệu)
public class ConflictException(string message, string? rule = null)
    : AppException(message, StatusCodes.Status409Conflict, rule);

// 400: dữ liệu/điều kiện nghiệp vụ không hợp lệ (CTR-02, Customer không hợp lệ...)
public class BusinessRuleException(string message, string? rule = null, IEnumerable<string>? errors = null)
    : AppException(message, StatusCodes.Status400BadRequest, rule, errors);
