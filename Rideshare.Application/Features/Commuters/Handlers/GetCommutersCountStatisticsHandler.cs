using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Commuters.Queries;
using Microsoft.AspNetCore.Identity;
using Rideshare.Domain.Models;
using Rideshare.Application.Exceptions;

namespace Rideshare.Application.Features.Commuters.Handlers;

public class GetCommutersCountStatisticsHandler : IRequestHandler<GetCommutersCountStatisticsQuery, BaseResponse<Dictionary<int, int>>>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;
	public UserManager<ApplicationUser> UserManager { get; set; }

	public GetCommutersCountStatisticsHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<BaseResponse<Dictionary<int, int>>> Handle(GetCommutersCountStatisticsQuery request, CancellationToken cancellationToken)
	{
		
		var validator = new GetCommutersCountStatisticsQueryValidator();
		var validationResult = await validator.ValidateAsync(request);
		
		if (!validationResult.IsValid)
			throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());			
		
		var history = await _userRepository.GetCommuterStatistics(request.Year, request.Month);
		return new BaseResponse<Dictionary<int, int>>
		{
			Success = true,
			Message = $"Fetched In Successfully",
			Value = history
		};
	}
}
