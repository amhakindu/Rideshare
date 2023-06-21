
using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;

namespace Application.Security.Handlers.CommandHandlers;

public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, BaseResponse<List<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserListQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<UserDto>>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<ApplicationUser> users = (await _userRepository.GetUsersAsync()).ToList();
        var userDtos = (List<UserDto>)_mapper.Map<IReadOnlyList<ApplicationUser>, IReadOnlyList<UserDto>>(users);
	
    	return new BaseResponse<List<UserDto>>()
		{
			Success = true,
			Value = userDtos,
			Message = "Users Fetched Successfully!"
		};

    }
}