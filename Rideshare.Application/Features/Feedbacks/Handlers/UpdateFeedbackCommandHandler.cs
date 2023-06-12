using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Feedbacks.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Feedbacks.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Feedbacks.Handlers
{
    public class UpdateFeedbackCommandHandler: IRequestHandler<UpdateFeedbackCommand, BaseResponse<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateFeedbackCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<Unit>> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateFeedbackValidator(_unitOfWork);
            var validatorResult = await validator.ValidateAsync(request.feedbackDto);
            var resultResponse = new ResultReponse();

            if (!validatorResult.IsValid)
            {
                return resultResponse.ValidationErrorReponse(validatorResult.Errors.Select(q => q.ErrorMessage).ToList());
            }

            var feedback = _mapper.Map<Feedback>(request.feedbackDto);
            var noOperations = await _unitOfWork.FeedbackRepository.Add(feedback);
            if (noOperations > 0)
            {
                return resultResponse.SuccessReponse();
            }
            else
            {
                return resultResponse.FailureReponse();
            }
        }

    }
}
