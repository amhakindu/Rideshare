using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;

namespace Rideshare.Application.Features.Auth.Queries;
public class GetCommuterStatusQueryHandler : IRequestHandler<GetCommuterStatusQuery, BaseResponse<CommuterStatusDto>>
{
	private readonly IUserRepository _userRepository;

	public GetCommuterStatusQueryHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<BaseResponse<CommuterStatusDto>> Handle(GetCommuterStatusQuery request, CancellationToken cancellationToken)
	{
        List<int> counts = await _userRepository.CountActiveCommuterAsync();
		int activeCount = counts[0];
		int idleCount = counts[1];

		var responseDto = new CommuterStatusDto
		{
			ActiveCommuters = activeCount,
			IdleCommuters = idleCount
		};
		
		var response = new BaseResponse<CommuterStatusDto>();
		response.Success = true;
		response.Message = "Fetched In Successfully";
		response.Value = responseDto;
		return response;

	}
}
