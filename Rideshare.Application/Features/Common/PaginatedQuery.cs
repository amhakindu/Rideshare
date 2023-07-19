using Rideshare.Application.Exceptions;

namespace Rideshare.Application.Features.Common;

public class PaginatedQuery
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
