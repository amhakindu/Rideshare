

// Query Handler
using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Handlers;

public sealed class GetCommuterCountQueryHandler : IRequestHandler<GetCommuterCountQuery, BaseResponse<CommuterCountDto>>
{
    private readonly IUserRepository _userRepository;

    public GetCommuterCountQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<BaseResponse<CommuterCountDto>> Handle(GetCommuterCountQuery request , CancellationToken cancellationToken)
    {
        DateTime endDate = DateTime.UtcNow.Date;
        DateTime startDate = endDate.AddDays(-7); // Last week

        int currentCount = await _userRepository.GetCommuterCount(endDate);
        int previousCount = await _userRepository.GetCommuterCount(startDate);

        double percentageChange = CalculatePercentageChange(currentCount, previousCount);

        var response = new BaseResponse<CommuterCountDto> ();

        var commuterCount = new CommuterCountDto
        {
            Name="Count",
            CurrentCount = currentCount,
            PercentageChange = percentageChange
        };

        response.Success = true;
        response.Message ="Fetched the Count and Statstics of Commuter";
        response.Value = commuterCount;

        return response;
    }

    private double CalculatePercentageChange(int currentCount, int previousCount)
    {
        if (previousCount == 0)
            return 0;

        return (currentCount - previousCount) / (double)previousCount * 100;
    }
}
