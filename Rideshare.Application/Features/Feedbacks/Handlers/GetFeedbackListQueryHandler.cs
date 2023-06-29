using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Common.Dtos.Pagination;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Feedbacks.Queries;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Feedbacks.Handlers
{
    public class GetFeedbackListQueryHandler: IRequestHandler<GetFeedbackListQuery, BaseResponse<PaginatedResponseDto<FeedbackDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetFeedbackListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<PaginatedResponseDto<FeedbackDto>>> Handle(GetFeedbackListQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<PaginatedResponseDto<FeedbackDto>>();


            var result = await _unitOfWork.FeedbackRepository.GetAll(request.PageNumber, request.PageSize);

            response.Value = new PaginatedResponseDto<FeedbackDto>();
            response.Success = true;
            response.Value.Paginated = _mapper.Map<IReadOnlyList<FeedbackDto>>(result.Paginated);
            response.Value.Count = result.Count;
            response.Message = "feedback fetched succesfully.";
            return response;
        }
    }
}
