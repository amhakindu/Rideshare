using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;


namespace Rideshare.Application.Features.Auth.Queries

{
	public class GetAllUsersQuery : IRequest<PaginatedResponse<UserDtoForAdmin>>
	{

		public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}