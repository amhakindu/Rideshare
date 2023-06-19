using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Feedbacks.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
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
    public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedBackCommand, BaseResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateFeedbackCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        

        public async Task<BaseResponse<int>> Handle(CreateFeedBackCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateFeedbackValidator(_unitOfWork);

            var validatorResult = await validator.ValidateAsync(request.feedbackDto);

            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors.Select(e => e.ErrorMessage).ToList().First());
            }

            var feedback = _mapper.Map<Feedback>(request.feedbackDto);
            var noOperations = await _unitOfWork.FeedbackRepository.Add(feedback);

            if (noOperations == 0)
            {
               throw new InternalServerErrorException("Unable to Save to Database");
            }
            return new BaseResponse<int>
            {
                Success = true,
                Message = "Feedback Creation Successful",
                Value = 1,
            };
        }
    }
}
