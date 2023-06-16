
using AutoMapper;
using MediatR;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;

namespace Application.Security.Handlers.CommandHandlers;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BaseResponse<ApplicationUser>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<ApplicationUser>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.UserId);
        var response = new BaseResponse<ApplicationUser>();

        if (user == null)
        {
            throw new Exception("Failed to create user.");
        }






        var applicationUser = _mapper.Map<ApplicationUser>(request.User);


        var updatedUser = await _userRepository.UpdateUserAsync(request.UserId, applicationUser);

        response.Success = true;
        response.Message = "User Created Successfully";
        response.Value = updatedUser;
        return response;





    }
}