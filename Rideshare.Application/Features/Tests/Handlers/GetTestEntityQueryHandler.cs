using AutoMapper;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Tests.Queries;
using Rideshare.Application.Responses;
using MediatR;
using Rideshare.Application.Common.Dtos.Tests;

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
            var movie = await _unitOfWork.TestEntityRepository.Get(query.TestEntityID);
            response.Success = true;
            response.Message = "Movie retrieval Successful";
            response.Value = _mapper.Map<TestEntityDto>(movie);
            return response;
        }
    }
}
