using AutoMapper;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Tests.Queries;
using Rideshare.Application.Responses;
using MediatR;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Exceptions;

namespace Rideshare.Application.Features.Movies.CQRS.Handlers
{
    public class GetTestEntityQueryHandler : IRequestHandler<GetTestEntityQuery, BaseResponse<TestEntityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTestEntityQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<TestEntityDto>> Handle(GetTestEntityQuery query, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<TestEntityDto>();
            var testEntity = await _unitOfWork.TestEntityRepository.Get(query.TestEntityID);
            if(testEntity == null)
                throw new NotFoundException($"TestEntity with ID {query.TestEntityID} does not exist");

            return new BaseResponse<TestEntityDto>{
                Success = true,
                Message = "TestEntity Retrieval Successful",
                Value = _mapper.Map<TestEntityDto>(testEntity)
            };
        }
    }
}
