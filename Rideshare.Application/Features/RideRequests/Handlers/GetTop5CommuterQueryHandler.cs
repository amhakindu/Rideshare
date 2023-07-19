using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Features.RideRequests.Queries;

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
            var commuterTasks = top5Commuters.Select( commuter => map(commuter).GetAwaiter().GetResult()).ToList();

            var response = new BaseResponse<List<CommuterWithRideRequestCntDto>>
            {
                Value = commuterTasks
            };

            return response;
        }
        async Task<CommuterWithRideRequestCntDto> map(KeyValuePair<string, int> data){
            return new CommuterWithRideRequestCntDto
            {
                Commuter = _mapper.Map<UserDtoForAdmin>(await _userRepository.FindByIdAsync(data.Key)),
                RideRequestCount = data.Value
            };
        }
    }
}
