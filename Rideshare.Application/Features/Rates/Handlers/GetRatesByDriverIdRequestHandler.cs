using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Queries;

namespace Rideshare.Application.Features.Rates.Handlers
{
    public class GetRatesByDriverIdRequestHandler : IRequestHandler<GetRatesByDriverIdRequest, BaseResponse<List<RateDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetRatesByDriverIdRequestHandler(IMapper mapper, IUnitOfWork work)
        {
            _mapper = mapper;
            _unitOfWork = work;
        }

        public async Task<BaseResponse<List<RateDto>>> Handle(GetRatesByDriverIdRequest request, CancellationToken cancellationToken)
        {

            var response = new BaseResponse<List<RateDto>>();
            
            var rates = await _unitOfWork.RateRepository.GetRatesByDriverId(request.PageNumber, request.PageSize, request.DriverId);

           var rateDtos = _mapper.Map<List<RateDto>>(rates);

            response.Success = true;
            response.Message = "Fetch Successful";
            response.Value = rateDtos;
            return response;




        }
    }
}