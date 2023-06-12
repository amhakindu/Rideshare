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
    public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedBackCommand, BaseResponse<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateFeedbackCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        

        public async Task<BaseResponse<Unit>> Handle(CreateFeedBackCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Unit>();
            var validator = new CreateFeedbackValidator();

            var validatorResult = await validator.ValidateAsync(request.feedbackDto);

            var resultReponse = new ResultReponse();

            if (!validatorResult.IsValid)
            {
                return resultReponse.ValidationErrorReponse(validatorResult.Errors.Select(q => q.ErrorMessage).ToList());
            }

            var feedback = _mapper.Map<Feedback>(request.feedbackDto);
            var noOperations = await _unitOfWork.FeedbackRepository.Update(feedback);

            if (noOperations > 0)
            {
                return resultReponse.SuccessReponse();
            }
            else
            {
                return resultReponse.FailureReponse();
            }
        }
    }
}
