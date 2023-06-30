using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;

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
		var commuters = await _userRepository.GetUsersByRoleAsync("Commuter", 1, (int)429496729);
		int ActiveCommuters = 0; int IdleCommuters = 0;
		foreach (var commuter in commuters.Paginated)
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
