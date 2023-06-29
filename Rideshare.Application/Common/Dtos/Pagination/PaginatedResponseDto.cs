using System;

namespace Rideshare.Application.Common.Dtos.Pagination
{
    public class PaginatedResponseDto<T>
    {
        public int PageNumber  { get; set; }
        public int PageSize {get;set;}
        public int Count { get; set; }
        public IReadOnlyList<T> Paginated { get; set; }



    }
}