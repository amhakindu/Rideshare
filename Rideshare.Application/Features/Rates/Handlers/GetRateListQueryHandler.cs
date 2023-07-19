using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Rates.Queries;

namespace Rideshare.Application.Features.Rates.Handlers;

public class GetRateListQueryHandler : IRequestHandler<GetRateListQuery, PaginatedResponse<RateDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetRateListQueryHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<PaginatedResponse<RateDto>> Handle(GetRateListQuery request, CancellationToken cancellationToken)
    {

        var response = new PaginatedResponse<RateDto>();
        var result = await _unitOfWork.RateRepository.GetAll(request.PageNumber, request.PageSize);

        response.Success = true;
        response.Message = "Fetch Succesful";
        response.Value = _mapper.Map<List<RateDto>>(result.Value);
        response.Count = result.Count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;

        return response;
    }
}
