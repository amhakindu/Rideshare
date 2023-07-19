using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Rideshare.Application.Features.User;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.Statistics;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Features.Vehicles.Queries;
using Rideshare.Application.Features.Commuters.Queries;
using Rideshare.Application.Features.RideOffers.Queries;
using Rideshare.Application.Features.RideRequests.Queries;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/statistics")]
public class StatisticsController : BaseApiController
{
    public StatisticsController(IMediator mediator, IUserAccessor userAccessor): base(mediator, userAccessor)
    {
    }

    [HttpGet("week/percentage-change")]
    [ProducesResponseType(typeof(BaseResponse<IList<EntityCountChangeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPercentageChangeFromLastWeek()
    {
        var result = await _mediator.Send(new GetPercentageChangeFromLastWeekQuery{});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<IList<EntityCountChangeDto>>>(status, result);
    } 

    // Commuters Statistics
    [HttpGet("commuters/top-five")]
    [ProducesResponseType(typeof( List<CommuterWithRideRequestCntDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopFiveCommuter()
    {
        var result = await _mediator.Send(new GetTop5CommuterQuery { });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

	[HttpGet("commuters/status-count")]
	public async Task<IActionResult> GetCommuterStatusCount()
	{
		var result = await _mediator.Send(new GetCommuterStatusQuery());

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}
	
	[HttpGet("commuters/time-series")]
	public async Task<IActionResult> GetCommuterTimeseries([FromQuery] string option, [FromQuery] int? year, [FromQuery] int? month)
	{
        var result = await _mediator.Send(new GetCommutersCountStatisticsQuery { Year = year, Month = month });
        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
	} 

    // Drivers Statistics
    [HttpGet("drivers/time-series")]
    public async Task<IActionResult> GetDriversTimeseries([FromQuery]string timeFrame, [FromQuery]int? year, [FromQuery]int? month)
    {
        var result = await _mediator.Send(new GetDriversStatisticsRequest {Year = year, Month = month });
        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet("drivers/status-count")]
    public async Task<IActionResult> GetDriversStatusCount()
    {
        var result = await _mediator.Send(new GetCountByStatusRequest { });
        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
    
    [HttpGet("drivers/top-five")]
    [ProducesResponseType(typeof(BaseResponse<List<DriverStatsDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopFiveDrivers()
    {
        var result = await _mediator.Send(new GetTopDriversWithStatsRequest { });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
    
    // RideOffers Statistics
    [HttpGet("rideoffers/time-series")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<int, int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRideOffersTimeseries([FromQuery] string timeframe, [FromQuery]int? Year, [FromQuery] int? Month)
    {
        var result = await _mediator.Send(new GetRideOfferStatsQuery { Year=Year, Month=Month });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<int, int>>>(status, result);
    }

    [HttpGet("rideoffers/time-series-for-each-status")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<int, int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRideOffersTimeseriesForEachStatus([FromQuery] string timeframe, [FromQuery]int? Year, [FromQuery] int? Month)
    {
        var result = await _mediator.Send(new GetRideOfferStatsWithStatusQuery{ Year= Year, Month= Month });

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<string, Dictionary<int, int>>>>(status, result);
    } 

    [HttpGet("rideoffers/status-count")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<string, int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRideOffersStatusCount()
    {
        var result = await _mediator.Send(new GetCountForEachStatusQuery{});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<string, int>>>(status, result);
    } 

    [HttpGet("rideoffers/top-ten-model-count")]
    [ProducesResponseType(typeof(BaseResponse<IReadOnlyList<ModelAndCountDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNumberOfVihcle()
    {
        var result = await _mediator.Send(new GetNoTopModelRideOffferQuery { });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    // RideRequests Statistics
    [HttpGet("riderequests/time-series")]
    public async Task<IActionResult> GetRideRequestsTimeseries([FromQuery] string timeframe, [FromQuery]int? Year, [FromQuery] int? Month)
    {
        var result = await _mediator.Send(new GetRideRequestByTimeQuery{Year= Year, Month= Month});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet("riderequests/time-series-for-each-status")]
    public async Task<IActionResult> GetRideRequestsTimeseriesForEachStatus([FromQuery] string timeframe, [FromQuery]int? Year, [FromQuery] int? Month)
    {
        var result = await _mediator.Send(new GetRideRequestStatusStatsticsQuery{ Year= Year, Month= Month, option= timeframe});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    [HttpGet("riderequests/status-count")]
    public async Task<IActionResult> GetRideRequestsStatusCount()
    {
        var result = await _mediator.Send(new GetRideRequestAllStatusStatsticsQuery{ });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    // Vehicles Statistics
    [HttpGet("vehicles/time-series")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetVehiclesTimeseries([FromQuery] string option, [FromQuery] int? year, [FromQuery] int? month)
    {
        var result = await _mediator.Send(new GetNumberOfVehicleQuery { Year = year, Month = month });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

}
