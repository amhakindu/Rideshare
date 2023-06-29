using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Features.Locations.Queries;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.testEntitys.CQRS.Handlers
{
    public class GetPopularDestinationsOfUserHandler: IRequestHandler<GetPopularDestinationsOfUserQuery, BaseResponse<IList<Dictionary<string, object>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapboxService _mapboxService;

        public GetPopularDestinationsOfUserHandler(IUnitOfWork unitOfWork, IMapper mapper, IMapboxService mapboxService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mapboxService = mapboxService;
        }

        public async Task<BaseResponse<IList<Dictionary<string, object>>>> Handle(GetPopularDestinationsOfUserQuery query, CancellationToken cancellationToken)
        {
            var driver = await _unitOfWork.DriverRepository.GetDriverByUserId(query.UserId);

            IReadOnlyList<GeographicalLocation> locations;
            if (driver != null){
                locations = await _unitOfWork.RideOfferRepository.GetPopularDestinationsOfDriver(driver.Id, query.Limit);
            }else{
                locations = await _unitOfWork.RideRequestRepository.GetPopularDestinationsOfCommuter(query.UserId, query.Limit);
            }

            return new BaseResponse<IList<Dictionary<string, object>>>{
                Success = true,
                Message = "RideOffers Fetching Successful",
                Value = locations.Select(location => getJsonForm(location)).ToList()
            };
        }

        private Dictionary<string, object> getJsonForm(GeographicalLocation location){
            return new Dictionary<string, object>(){
                {"Latitude", location.Coordinate.Y},
                {"Longitude", location.Coordinate.X},
                {"Name", location.Address}
            };
        }
    }
}