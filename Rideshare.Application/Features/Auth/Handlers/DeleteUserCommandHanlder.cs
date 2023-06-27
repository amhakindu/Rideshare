
using AutoMapper;
using MediatR;
using Rideshare.Domain.Models;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Security;
using System.Text;
using Rideshare.Application.Contracts.Services;

namespace Rideshare.Application.Features.Auth.Handlers;

public class DeleltUserCommandHandler : IRequestHandler<DeleteUserCommand, BaseResponse<Double>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISmsSender _smsSender;
    private readonly IMapper _mapper;
    private readonly IResourceManager _resourceManager;
    private static Random random = new Random();

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