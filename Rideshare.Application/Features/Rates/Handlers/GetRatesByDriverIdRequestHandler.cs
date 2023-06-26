using System;
using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Responses;

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
            // var validator = new PaginationDtoValidator();

            // var validationResult = await validator.ValidateAsync(request.PaginationDto);

            // if (!validationResult.IsValid)
            //     throw new ValidationException(validationResult.Errors.Select(q => q.ErrorMessage).ToList().First());
            var rates = await _unitOfWork.RateRepository.GetRatesByDriverId(request.PageNumber, request.PageSize, request.DriverId);

           var rateDtos = _mapper.Map<List<RateDto>>(rates);

            response.Success = true;
            response.Message = "Fetch Successful";
            response.Value = rateDtos;
            return response;




        }
    }
}