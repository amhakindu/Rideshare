using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Feedbacks.Queries;

namespace Rideshare.Application.Features.Feedbacks.Handlers
{
    public class GetFeedbackListQueryHandler: IRequestHandler<GetFeedbackListQuery, PaginatedResponse<FeedbackDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetFeedbackListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResponse<FeedbackDto>> Handle(GetFeedbackListQuery request, CancellationToken cancellationToken)
        {
            var response = new PaginatedResponse<FeedbackDto>();

            var result = await _unitOfWork.FeedbackRepository.GetAll(request.PageNumber, request.PageSize);

            response.Success = true;
            response.Message = "Feedbacks Fetched Succesfully.";
            response.Value = _mapper.Map<IReadOnlyList<FeedbackDto>>(result.Value);
            response.Count = result.Count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }
    }
}
