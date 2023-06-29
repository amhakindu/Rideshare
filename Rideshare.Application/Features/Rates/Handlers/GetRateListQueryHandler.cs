using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Pagination;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Rates.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Rates.Handlers;
public class GetRateListQueryHandler : IRequestHandler<GetRateListQuery, BaseResponse<PaginatedResponseDto<RateDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetRateListQueryHandler(IMapper mapper, IUnitOfWork work)
    {
        _mapper = mapper;
        _unitOfWork = work;
    }

    public async Task<BaseResponse<PaginatedResponseDto<RateDto>>> Handle(GetRateListQuery request, CancellationToken cancellationToken)
    {

        var response = new BaseResponse<PaginatedResponseDto<RateDto>>();
        var result = await _unitOfWork.RateRepository.GetAll(request.PageNumber, request.PageSize);


        response.Success = true;
        response.Message = "Fetch Succesful";
        response.Value = new PaginatedResponseDto<RateDto>();
        response.Value.Count = result.Count;
        response.Value.PageNumber = request.PageNumber;
        response.Value.PageSize = request.PageSize;
        response.Value.Paginated = _mapper.Map<List<RateDto>>(result.Paginated);

        return response;


    }
}
