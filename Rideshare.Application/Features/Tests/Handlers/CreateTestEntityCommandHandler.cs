using System.Data;
using AutoMapper;
using Rideshare.Application.Contracts.Persistence;
using MediatR;
using Rideshare.Application.Common.Dtos.Tests.Validators;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using Rideshare.Application.Exceptions;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class CreateTestEntityCommandHandler: IRequestHandler<CreateTestEntityCommand, BaseResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateTestEntityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<int>> Handle(CreateTestEntityCommand command, CancellationToken cancellationToken)
        {
            var validator = new TestEntityDtoValidator();
            var validationResult = await validator.ValidateAsync(command.TestDto);

            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList().First());

            var testEntity = _mapper.Map<TestEntity>(command.TestDto);

            var dbOperations = await _unitOfWork.TestEntityRepository.Add(testEntity);

            if (dbOperations == 0)
                throw new InternalServerErrorException("Unable to Save to Database");

            return new BaseResponse<int>{
                Success = true,
                Message = "TestEntity Creation Successful",
                Value = testEntity.Id
            };
        }
    }
}
