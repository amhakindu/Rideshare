using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Feedbacks.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Feedbacks.Handlers
{
    public class GetFeedbackDetailQueryHandler : IRequestHandler<GetFeedbackDetailQuery, BaseResponse<FeedbackDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetFeedbackDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<FeedbackDto>> Handle(GetFeedbackDetailQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<FeedbackDto>();
            var feedback = await _unitOfWork.FeedbackRepository.Get(request.Id);

            if (feedback == null)
            {
                response.Success = false;
                response.Message = "feedback not found!";
            }
            else
            {
                response.Success = true;
                response.Value = _mapper.Map<FeedbackDto>(feedback);
                response.Message = "feedback fetched succesfully.";
            }
            return response;
        }
    }
}
