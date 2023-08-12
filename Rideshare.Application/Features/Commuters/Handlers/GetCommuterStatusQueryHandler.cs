using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Common.Dtos.Security;
using AutoMapper;

namespace Rideshare.Application.Features.Commuters.Queries;
public class GetCommuterStatusQueryHandler : IRequestHandler<GetCommuterStatusQuery, BaseResponse<StatusDto>>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;


	public GetCommuterStatusQueryHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;

	}

	public async Task<BaseResponse<StatusDto>> Handle(GetCommuterStatusQuery request, CancellationToken cancellationToken)
	{

		
		var commuterStatus = await _userRepository.GetCommuterStatusCountAsync();

		var response = new BaseResponse<StatusDto>
		{
			Success = true,
			Message = "Commuters status count fetched successfully!",
			Value = commuterStatus
		};

		return response;

	}
}
