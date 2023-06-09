using MediatR;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Tests.Commands;

public class CreateTestEntityCommand: IRequest<BaseResponse<int>>
{
    public TestEntityDto TestDto { get; set; }  
}
