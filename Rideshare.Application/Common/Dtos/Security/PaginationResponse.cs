using Rideshare.Domain.Models;

namespace Rideshare.Application.Common.Dtos.Security;
public class PaginatedResponse
    {
        public int Count { get; set; }
        public List<ApplicationUser> PaginatedUsers { get; set; }
    }