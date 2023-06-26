using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RideRequests.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.RideRequests.Handlers
{
    public class GetTop5CommuterQueryHandler: IRequestHandler<GetTop5CommuterQuery, BaseResponse<List<CommuterWithRideRequestCntDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;


        public GetTop5CommuterQueryHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<CommuterWithRideRequestCntDto>>> Handle(GetTop5CommuterQuery request, CancellationToken cancellationToken)
        {
            var top5Commuters = await _unitOfWork.RideRequestRepository.GetTop5Commuter();
            var commuterTasks = top5Commuters.Select(async commuter =>
                new CommuterWithRideRequestCntDto
                {
                    Commuter = await _userRepository.FindByIdAsync(commuter.Key),
                    RideRequestCount = commuter.Value
                }).ToList();

            var commuterDtos = await Task.WhenAll(commuterTasks);

            var response = new BaseResponse<List<CommuterWithRideRequestCntDto>>
            {
                Value = commuterDtos.ToList(),
            };

            return response;
        }
    }
}
