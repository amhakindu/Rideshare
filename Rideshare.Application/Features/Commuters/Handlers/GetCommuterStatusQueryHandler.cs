using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Common.Dtos.Security;

namespace Rideshare.Application.Features.Commuters.Queries;
public class GetCommuterStatusQueryHandler : IRequestHandler<GetCommuterStatusQuery, BaseResponse<CommuterStatusDto>>
{
	private readonly IUserRepository _userRepository;

	public GetCommuterStatusQueryHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<BaseResponse<CommuterStatusDto>> Handle(GetCommuterStatusQuery request, CancellationToken cancellationToken)
	{
		var commuters = await _userRepository.GetUsersByRoleAsync("Commuter", 1, int.MaxValue);
		int ActiveCommuters = 0; int IdleCommuters = 0;
		
		if (commuters?.Value != null)
		{
			
			foreach (var commuter in commuters.Value)
			{
				if (commuter.LastLogin >= DateTime.Now.AddDays(-30))
				{
					ActiveCommuters +=1;
				}
				else 
				{
					IdleCommuters += 1;
				}
			};
		}
		

		var responseDto = new CommuterStatusDto
		{
			ActiveCommuters = ActiveCommuters,
			IdleCommuters = IdleCommuters
		};
		
		var response = new BaseResponse<CommuterStatusDto>();
		response.Success = true;
		response.Message = "Commuters status count fetched Successfully!";
		response.Value = responseDto;
		return response;

	}
}
