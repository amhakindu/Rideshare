using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Features.Auth.Commands;

namespace Rideshare.Application.Features.Auth.Handlers;

public class DeleltUserCommandHandler : IRequestHandler<DeleteUserCommand, BaseResponse<Double>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IResourceManager _resourceManager;

    public DeleltUserCommandHandler(IUserRepository userRepository, IMapper mapper, IResourceManager resourceManager)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _resourceManager = resourceManager;
    }

    public async Task<BaseResponse<Double>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {

        await _userRepository.DeleteUserAsync(request.UserId);

        var response = new BaseResponse<Double>();
        response.Success = true;
        response.Message = "User Deleted Successfully";
        response.Value = 1;
        return response;
    }
}