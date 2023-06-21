using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Feedbacks.Validators;
using Rideshare.Application.Contracts.Identity;
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
    public class DeleteFeedbackCommandHandler : IRequestHandler<DeleteFeedbackCommand, BaseResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public DeleteFeedbackCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async Task<BaseResponse<int>> Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteFeedbackValidator(_unitOfWork);
            var validatorResult = await validator.ValidateAsync(request.Id);

            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors.Select(e => e.ErrorMessage).ToList().First());
            }

            var feedback = await _unitOfWork.FeedbackRepository.Get(request.Id);
            
            if (feedback == null)
            {
                throw new NotFoundException($"Feedback with ID {request.Id} does not exist");
            }
            var user = await _userRepository.FindByIdAsync(feedback.UserId);

            if (user == null)
            {
                throw new NotFoundException($"User with ID {request.Id} does not exist");
            }
            var ops =  await _unitOfWork.FeedbackRepository.Delete(feedback);
            if (ops == 0)
            {
                throw new InternalServerErrorException("Unable to Save to Database");
            }
            return new BaseResponse<int>
            {
                Success = true,
                Message = "Feedback delete Successful",
                Value = request.Id,
            };
        }
    }
}
