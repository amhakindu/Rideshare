using Rideshare.Domain.Models;

namespace Rideshare.Application.Common.Dtos.Security;
public class PaginatedResponse<T>
    {
        public int Count { get; set; }
        public IReadOnlyList<T> Paginated { get; set; }
    }