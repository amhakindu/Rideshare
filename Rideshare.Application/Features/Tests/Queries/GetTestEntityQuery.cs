using MediatR;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Tests.Queries;

public class GetTestEntityQuery: IRequest<BaseResponse<TestEntityDto>>
{
    public int TestEntityID { get; set; }
}
