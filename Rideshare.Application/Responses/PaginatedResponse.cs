namespace Rideshare.Application.Responses;

public class PaginatedResponse<T>: BaseResponse<IReadOnlyList<T>>
{
    public int PageNumber  { get; set; }
    public int PageSize {get;set;}
    public int Count { get; set; }
}
