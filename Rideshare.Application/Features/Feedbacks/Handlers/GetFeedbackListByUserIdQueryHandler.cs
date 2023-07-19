using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Feedbacks.Queries;

namespace Rideshare.Application.Features.Feedbacks.Handlers
{
    public class GetFeedbackListByUserIdQueryHandler : IRequestHandler<GetFeedbackListByUserIdQuery, BaseResponse<IReadOnlyList<FeedbackDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetFeedbackListByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<BaseResponse<IReadOnlyList<FeedbackDto>>> Handle(GetFeedbackListByUserIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IReadOnlyList<FeedbackDto>>();

            var user = await _userRepository.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {request.UserId} does not exist");
            }

            var feedbacks = await _unitOfWork.FeedbackRepository.GetAllByUserId(request.UserId, request.PageNumber, request.PageSize);
            response.Success = true;
            response.Value = _mapper.Map<IReadOnlyList<FeedbackDto>>(feedbacks);
            response.Message = "feedback fetched succesfully.";
            return response;
        }
    }
}
