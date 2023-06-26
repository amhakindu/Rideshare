namespace Rideshare.Application.Common.Dtos.Security;
public class PaginatedUserList
    {
        public int PageNumber  { get; set; }
        public int PageSize {get;set;}
        public int Count { get; set; }
        public List<UserDtoForAdmin> PaginatedUsers { get; set; }
    }