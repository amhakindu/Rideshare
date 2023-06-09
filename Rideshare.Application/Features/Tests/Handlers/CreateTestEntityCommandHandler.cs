using System.Data;
using AutoMapper;
using Rideshare.Application.Contracts.Persistence;
using MediatR;
using Rideshare.Application.Common.Dtos.Tests.Validators;
using Rideshare.Application.Features.Tests.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

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
            var response = new BaseResponse<int>();
            var validator = new TestEntityDtoValidator();
            var validationResult = await validator.ValidateAsync(command.TestDto);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Test Entity Creation Failed";
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            }
            else
            {
                var testEntity = _mapper.Map<TestEntity>(command.TestDto);

                var dbOperations = await _unitOfWork.TestEntityRepository.Add(testEntity);

                if (dbOperations > 0)
                {
                    response.Success = true;
                    response.Message = "Test Entity Creation Successful";
                    response.Value = testEntity.Id;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Test Entity Creation Failed";
                }

            }

            return response;
        }
    }
}
