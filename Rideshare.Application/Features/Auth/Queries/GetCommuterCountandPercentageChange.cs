// Query
using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

public class GetCommuterCountQuery : IRequest<BaseResponse<CommuterCountDto>>
{

}
