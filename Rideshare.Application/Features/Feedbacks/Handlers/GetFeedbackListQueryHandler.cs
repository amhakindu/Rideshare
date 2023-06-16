using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Feedbacks;
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
    public class GetFeedbackListQueryHandler: IRequestHandler<GetFeedbackListQuery, BaseResponse<List<FeedbackDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetFeedbackListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<List<FeedbackDto>>> Handle(GetFeedbackListQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<FeedbackDto>>();
            var feedbacks = await _unitOfWork.FeedbackRepository.GetAll();
            response.Success = true;
            response.Value = _mapper.Map<List<FeedbackDto>>(feedbacks);
            response.Message = "feedback fetched succesfully.";
            return response;
        }
    }
}
