using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Rates.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Rates.Handlers
{
    public class DeleteRateCommandHandler : IRequestHandler<DeleteRateCommand, BaseResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteRateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse<int>> Handle(DeleteRateCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteRateValidator(_unitOfWork);
            var validatorResult = await validator.ValidateAsync(request.RateId);

            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors.Select(e => e.ErrorMessage).ToList().First());
            }

            var rate = await _unitOfWork.RateRepository.Get(request.RateId);
            if (rate == null)
            {
                throw new NotFoundException($"Rate With ID {request.RateId} does not exist");
            }
            var operations =  await _unitOfWork.RateRepository.Delete(rate);
            if (operations == 0)
            {
                throw new InternalServerErrorException("Unable to Save to Database");
            }
            return new BaseResponse<int>
            {
                Success = true,
                Message = $"Rate With Id {request.RateId} Deleted Successfully",
                Value = request.RateId,
            };
        }
    }
}