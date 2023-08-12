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
using Rideshare.Application.Common.Dtos.Security;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/statistics")]
public class StatisticsController : BaseApiController
{
    public StatisticsController(IMediator mediator, IUserAccessor userAccessor): base(mediator, userAccessor)
    {
    }

    /// <summary>
    /// Get percentage change in count from the last week.
    /// </summary>
    /// <remarks>Retrieves a list of entity count changes for some of the entities in the system from the last week.
    /// This endpoint is accessible to admins only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Entity count changes retrieved successfully",
    ///    "value": [
    ///        {
    ///            "name": "rideoffers",
    ///            "currentCount": 100,
    ///            "percentageChange": 10.5
    ///        },
    ///        {
    ///            "name": "riderequests",
    ///            "currentCount": 200,
    ///            "percentageChange": -5.2
    ///        },
    ///        ...
    ///    ]
    /// }
    /// </remarks>
    /// <response code="200">Indicates that the entity count changes were successfully retrieved</response>
    /// <returns>
    /// A response containing the retrieved entity count changes.
    /// If successful, the response will include a list of entity count changes; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("week/percentage-change")]
    [ProducesResponseType(typeof(BaseResponse<IList<EntityCountChangeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPercentageChangeFromLastWeek()
    {
        var result = await _mediator.Send(new GetPercentageChangeFromLastWeekQuery{});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<IList<EntityCountChangeDto>>>(status, result);
    } 

    /// <summary>
    /// Get the top five commuters with the highest ride request counts.
    /// </summary>
    /// <remarks>Retrieves a list of the top five commuters with the highest ride request counts.
    /// This endpoint is accessible to admins only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Top five commuters with highest ride request counts retrieved successfully",
    ///    "value": [
    ///        {
    ///            "commuter": {
    ///                "fullName": "CommuterName1",
    ///                "phoneNumber": "1234567890",
    ///                "age": 30,
    ///                "statusByLogin": "Active",
    ///                "profilePicture": "http://example.com/profile1.jpg"
    ///            },
    ///            "rideRequestCount": 10
    ///        },
    ///        {
    ///            "commuter": {
    ///                "fullName": "CommuterName2",
    ///                "phoneNumber": "9876543210",
    ///                "age": 25,
    ///                "statusByLogin": "Inactive",
    ///                "profilePicture": "http://example.com/profile2.jpg"
    ///            },
    ///            "rideRequestCount": 8
    ///        }
    ///        // Add more commuters if needed
    ///    ]
    /// }
    /// </remarks>
    /// <response code="200">Indicates that the top five commuters were successfully retrieved</response>
    /// <returns>
    /// A response containing the retrieved top five commuters with their ride request counts.
    /// If successful, the response will include a list of commuters; otherwise, an error message will be provided.
    /// </returns>
    // Commuters Statistics
    [HttpGet("commuters/top-five")]
    [ProducesResponseType(typeof( List<CommuterWithRideRequestCntDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopFiveCommuter()
    {
        var result = await _mediator.Send(new GetTop5CommuterQuery { });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Get the count of active and idle commuters.
    /// </summary>
    /// <remarks>Retrieves the count of active and idle commuters.
    /// This endpoint is accessible to admins only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Active and idle commuter counts retrieved successfully",
    ///    "value": {
    ///        "active": 50,
    ///        "idle": 20
    ///    }
    /// }
    /// </remarks>
    /// <response code="200">Indicates that the commuter status counts were successfully retrieved</response>
    /// <returns>
    /// A response containing the count of active and idle commuters.
    /// If successful, the response will include the count of active and idle commuters; otherwise, an error message will be provided.
    /// </returns>
	[HttpGet("commuters/status-count")]
    [ProducesResponseType(typeof(BaseResponse<StatusDto>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetCommuterStatusCount()
	{
		var result = await _mediator.Send(new GetCommuterStatusQuery());

		var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
		return getResponse(status, result);
	}

    /// <summary>
    /// Get commuter count time series data.
    /// </summary>
    /// <remarks>Retrieves time series data for commuter counts based on the specified time series options.
    /// Time series options include "weekly", "monthly", or "yearly". These options allow you to choose the granularity of the time series data.
    /// For instance, if you select the "weekly" option, the time series data will provide commuter counts for each week within the specified year and month.
    /// Similarly, the "monthly" option will provide commuter counts for each individual month, and the "yearly" option will provide counts for each year.
    /// The time series data is structured in a dictionary format, where the keys represent the time periods (weeks, months, or years)
    /// and the values represent the corresponding commuter counts during those periods.
    /// This endpoint is accessible to administrators only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Commuter count time series data retrieved successfully",
    ///    "value": {
    ///        "01": 50, // Week 01 of a given month or January of a given year had 50 active commuters
    ///        "02": 60, // Week 02 of a given month or January of a given year had 60 active commuters
    ///        ...
    ///    }
    /// }
    /// </remarks>
    /// <param name="option">The time series option ("weekly", "monthly", or "yearly")</param>
    /// <param name="year">The year for which to retrieve the time series data</param>
    /// <param name="month">The month for which to retrieve the time series data</param>
    /// <response code="200">Indicates that the commuter count time series data was successfully retrieved</response>
    /// <response code="406">Indicates that invalid timeseries options are given either year and/or month </response>
    /// <returns>
    /// A response containing time series data for commuter counts.
    /// If successful, the response will include the time series data; otherwise, an error message will be provided.
    /// </returns>
	[HttpGet("commuters/time-series")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<int, int>>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetCommuterTimeseries([FromQuery] string option, [FromQuery] int? year, [FromQuery] int? month)
	{
        var result = await _mediator.Send(new GetCommutersCountStatisticsQuery { Year = year, Month = month });
        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
	} 

    /// <summary>
    /// Get driver count time series data.
    /// </summary>
    /// <remarks>Retrieves time series data for driver counts based on the specified time series options.
    /// Time series options include "weekly", "monthly", or "yearly". These options allow you to choose the granularity of the time series data.
    /// For instance, if you select the "weekly" option, the time series data will provide driver counts for each week within the specified year and month.
    /// Similarly, the "monthly" option will provide driver counts for each individual month, and the "yearly" option will provide counts for each year.
    /// The time series data is structured in a dictionary format, where the keys represent the time periods (weeks, months, or years)
    /// and the values represent the corresponding driver counts during those periods.
    /// This endpoint is accessible to administrators only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Driver count time series data retrieved successfully",
    ///    "value": {
    ///        "01": 50, // Week 01 of a given month or January of a given year had 50 active driver
    ///        "02": 60, // Week 02 of a given month or January of a given year had 60 active drivers
    ///        ...
    ///    }
    /// }
    /// </remarks>
    /// <param name="option">The time series option ("weekly", "monthly", or "yearly")</param>
    /// <param name="year">The year for which to retrieve the time series data</param>
    /// <param name="month">The month for which to retrieve the time series data</param>
    /// <response code="200">Indicates that the drivers count time series data was successfully retrieved</response>
    /// <response code="406">Indicates that invalid timeseries options are given either year and/or month </response>
    /// <returns>
    /// A response containing time series data for drivers counts.
    /// If successful, the response will include the time series data; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("drivers/time-series")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<int, int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDriversTimeseries([FromQuery]string timeFrame, [FromQuery]int? year, [FromQuery]int? month)
    {
        var result = await _mediator.Send(new GetDriversStatisticsRequest {Year = year, Month = month });
        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Get the count of active and idle drivers.
    /// </summary>
    /// <remarks>Retrieves the count of active and idle drivers.
    /// This endpoint is accessible to admins only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Active and idle drivers counts retrieved successfully",
    ///    "value": {
    ///        "active": 50,
    ///        "idle": 20
    ///    }
    /// }
    /// </remarks>
    /// <response code="200">Indicates that the drivers status counts were successfully retrieved</response>
    /// <returns>
    /// A response containing the count of active and idle commuters.
    /// If successful, the response will include the count of active and idle drivers; otherwise, an error message will be provided.
    /// </returns>drivers
    [HttpGet("drivers/status-count")]
    [ProducesResponseType(typeof(BaseResponse<StatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDriversStatusCount()
    {
        var result = await _mediator.Send(new GetCountByStatusRequest { });
        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Retrieve Top Performing Drivers
    /// </summary>
    /// <remarks>Retrieves a list of the top-performing drivers based on their statistics, including total ride offers and earnings.
    /// The list is sorted in descending order, showcasing the drivers with the highest performance first.
    /// Each driver's statistics include their unique identifier (DriverID), the total number of ride offers they've provided (TotalOffers),
    /// and the total earnings they've generated (Earnings).
    /// This endpoint is accessible to administrators only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Top performing drivers retrieved successfully",
    ///    "value": [
    ///        {
    ///            "DriverID": "5d42633e-7ae4-4bfb-8be9-953c17f7a1cc",
    ///            "TotalOffers": 102,
    ///            "Earnings": 1250.75
    ///        },
    ///        {
    ///            "DriverID": "4e17301f-2f2d-43e1-97f1-08c0d30a7ba7",
    ///            "TotalOffers": 97,
    ///            "Earnings": 1100.00
    ///        },
    ///        // ... (remaining top-performing drivers)
    ///    ]
    /// }
    /// </remarks>
    /// <response code="200">Indicates that the top-performing drivers were successfully retrieved</response>
    /// <response code="404">Indicates that no top-performing drivers were found</response>
    /// <returns>
    /// A response containing a list of top-performing drivers and their statistics.
    /// If successful, the response will include the list of drivers; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("drivers/top-five")]
    [ProducesResponseType(typeof(BaseResponse<List<DriverStatsDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopFiveDrivers()
    {
        var result = await _mediator.Send(new GetTopDriversWithStatsRequest { });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }
    

    /// <summary>
    /// Get rideoffer count time series data.
    /// </summary>
    /// <remarks>Retrieves time series data for rideoffer counts based on the specified time series options.
    /// Time series options include "weekly", "monthly", or "yearly". These options allow you to choose the granularity of the time series data.
    /// For instance, if you select the "weekly" option, the time series data will provide rideoffer counts for each week within the specified year and month.
    /// Similarly, the "monthly" option will provide rideoffer counts for each individual month, and the "yearly" option will provide counts for each year.
    /// The time series data is structured in a dictionary format, where the keys represent the time periods (weeks, months, or years)
    /// and the values represent the corresponding rideoffer counts during those periods.
    /// This endpoint is accessible to administrators only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Rideoffer count time series data retrieved successfully",
    ///    "value": {
    ///        "01": 50, // Week 01 of a given month or January of a given year had 50 rideoffers
    ///        "02": 60, // Week 02 of a given month or January of a given year had 60 rideoffers
    ///        ...
    ///    }
    /// }
    /// </remarks>
    /// <param name="option">The time series option ("weekly", "monthly", or "yearly")</param>
    /// <param name="year">The year for which to retrieve the time series data</param>
    /// <param name="month">The month for which to retrieve the time series data</param>
    /// <response code="200">Indicates that the rideoffer count time series data was successfully retrieved</response>
    /// <response code="406">Indicates that invalid timeseries options are given either year and/or month </response>
    /// <response code="404">Indicates that no rideoffer count time series data was found</response>
    /// <returns>
    /// A response containing time series data for rideoffer counts.
    /// If successful, the response will include the time series data; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("rideoffers/time-series")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<int, int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRideOffersTimeseries([FromQuery] string timeframe, [FromQuery]int? Year, [FromQuery] int? Month)
    {
        var result = await _mediator.Send(new GetRideOfferStatsQuery { Year=Year, Month=Month });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<int, int>>>(status, result);
    }

    /// <summary>
    /// Retrieve Ride Offer Time Series for Each Status
    /// </summary>
    /// <remarks>Retrieves time series data for ride offers categorized by their status.
    /// The data is presented in a nested dictionary format, where the outer dictionary's keys represent different ride offer status.
    /// The inner dictionary contains a breakdown of ride offers count per time period, based on the provided timeframe (weekly, monthly, or yearly).
    /// This endpoint is accessible to administrators only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Ride offer time series for each status retrieved successfully",
    ///    "value": {
    ///        "failed": {
    ///            "01": 50, // Week 01 of a given month or January of a given year had 50 failed rideoffers
    ///            "02": 60, // Week 02 of a given month or January of a given year had 60 failed rideoffers
    ///            ...
    ///        }
    ///        "completed": {
    ///            "01": 50, // Week 01 of a given month or January of a given year had 50 completed rideoffers
    ///            "02": 60, // Week 02 of a given month or January of a given year had 60 completed rideoffers
    ///            ...
    ///        }
    ///    }
    /// }
    /// </remarks>
    /// <param name="timeframe">The timeframe for the time series data: "weekly", "monthly", or "yearly"</param>
    /// <param name="Year">The year for which to retrieve data</param>
    /// <param name="Month">The month for which to retrieve data</param>
    /// <response code="200">Indicates that the ride offer time series data was successfully retrieved</response>
    /// <response code="406">Indicates that invalid timeseries options are given either year and/or month </response>
    /// <returns>
    /// A response containing a dictionary with nested dictionaries representing time series data for each ride offer status.
    /// If successful, the response will include the time series data; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("rideoffers/time-series-for-each-status")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<String, Dictionary<int, int>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRideOffersTimeseriesForEachStatus([FromQuery] string timeframe, [FromQuery]int? Year, [FromQuery] int? Month)
    {
        var result = await _mediator.Send(new GetRideOfferStatsWithStatusQuery{ Year= Year, Month= Month });

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<string, Dictionary<int, int>>>>(status, result);
    } 


    /// <summary>
    /// Retrieve Ride Offer Status Counts
    /// </summary>
    /// <remarks>Retrieves the count of ride offers categorized by their status.
    /// The keys in the dictionary represent different ride offer statuses, including "waiting", "onroute", "completed", and "cancelled".
    /// This endpoint is accessible to administrators only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Ride offer status counts retrieved successfully",
    ///    "value": {
    ///        "waiting": 15,      // Count of ride offers in "waiting" status
    ///        "onroute": 20,      // Count of ride offers in "onroute" status
    ///        "completed": 100,   // Count of ride offers in "completed" status
    ///        "cancelled": 5      // Count of ride offers in "cancelled" status
    ///    }
    /// }
    /// </remarks>
    /// <response code="200">Indicates that the ride offer status counts were successfully retrieved</response>
    /// <returns>
    /// A response containing a dictionary with keys representing different ride offer statuses and their respective counts.
    /// If successful, the response will include the status counts; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("rideoffers/status-count")]
    [ProducesResponseType(typeof(BaseResponse<Dictionary<string, int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRideOffersStatusCount()
    {
        var result = await _mediator.Send(new GetCountForEachStatusQuery{});

        var status = result.Success ? HttpStatusCode.OK: HttpStatusCode.NotFound;
        return getResponse<BaseResponse<Dictionary<string, int>>>(status, result);
    } 

    /// <summary>
    /// Retrieve top 10 Vehicle Models
    /// </summary>
    /// <remarks>Retrieves a list of the top ten vehicle models ranked with their corresponding counts of rideoffers.
    /// Each item in the list includes the vehicle model and the number of ride offers associated with that model.
    /// This endpoint is accessible to administrators only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Top ten ride offer vehicle models retrieved successfully",
    ///    "value": [
    ///        {
    ///            "model": "Toyota Camry",
    ///            "count": 150
    ///        },
    ///        {
    ///            "model": "Honda Accord",
    ///            "count": 120
    ///        },
    ///        // ... (up to ten entries)
    ///    ]
    /// }
    /// </remarks>
    /// <response code="200">Indicates that the top ten ride offer vehicle models were successfully retrieved</response>
    /// <returns>
    /// A response containing a list of the top ten ride offer vehicle models and their corresponding counts.
    /// If successful, the response will include the vehicle models and counts; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("rideoffers/top-ten-model-count")]
    [ProducesResponseType(typeof(BaseResponse<IReadOnlyList<ModelAndCountDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNumberOfVihcle()
    {
        var result = await _mediator.Send(new GetNoTopModelRideOfferQuery { });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Get ride request count time series data.
    /// </summary>
    /// <remarks>Retrieves time series data for ride request counts based on the specified time series options.
    /// Time series options include "weekly", "monthly", or "yearly". These options allow you to choose the granularity of the time series data.
    /// For instance, if you select the "weekly" option, the time series data will provide ride request counts for each week within the specified year and month.
    /// Similarly, the "monthly" option will provide ride request counts for each individual month, and the "yearly" option will provide counts for each year.
    /// The time series data is structured in a dictionary format, where the keys represent the time periods (weeks, months, or years)
    /// and the values represent the corresponding ride request counts during those periods.
    /// This endpoint is accessible to administrators only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "ride request count time series data retrieved successfully",
    ///    "value": {
    ///        "01": 50, // Week 01 of a given month or January of a given year had 50 active ride requests
    ///        "02": 60, // Week 02 of a given month or January of a given year had 60 active ride requests
    ///        ...
    ///    }
    /// }
    /// </remarks>
    /// <param name="option">The time series option ("weekly", "monthly", or "yearly")</param>
    /// <param name="year">The year for which to retrieve the time series data</param>
    /// <param name="month">The month for which to retrieve the time series data</param>
    /// <response code="200">Indicates that the ride request count time series data was successfully retrieved</response>
    /// <response code="406">Indicates that invalid timeseries options are given either year and/or month </response>
    /// <returns>
    /// A response containing time series data for ride request counts.
    /// If successful, the response will include the time series data; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("riderequests/time-series")]
    public async Task<IActionResult> GetRideRequestsTimeseries([FromQuery] string timeframe, [FromQuery]int? Year, [FromQuery] int? Month)
    {
        var result = await _mediator.Send(new GetRideRequestByTimeQuery{Year= Year, Month= Month});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }


    /// <summary>
    /// Retrieve ride request Time Series for Each Status
    /// </summary>
    /// <remarks>Retrieves time series data for ride requests categorized by their status.
    /// The data is presented in a nested dictionary format, where the outer dictionary's keys represent different ride request status.
    /// The inner dictionary contains a breakdown of ride requests count per time period, based on the provided timeframe (weekly, monthly, or yearly).
    /// This endpoint is accessible to administrators only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "ride request time series for each status retrieved successfully",
    ///    "value": {
    ///        "failed": {
    ///            "01": 50, // Week 01 of a given month or January of a given year had 50 failed ride requests
    ///            "02": 60, // Week 02 of a given month or January of a given year had 60 failed ride requests
    ///            ...
    ///        }
    ///        "completed": {
    ///            "01": 50, // Week 01 of a given month or January of a given year had 50 completed ride requests
    ///            "02": 60, // Week 02 of a given month or January of a given year had 60 completed ride requests
    ///            ...
    ///        }
    ///    }
    /// }
    /// </remarks>
    /// <param name="timeframe">The timeframe for the time series data: "weekly", "monthly", or "yearly"</param>
    /// <param name="Year">The year for which to retrieve data</param>
    /// <param name="Month">The month for which to retrieve data</param>
    /// <response code="200">Indicates that the ride request time series data was successfully retrieved</response>
    /// <response code="406">Indicates that invalid timeseries options are given either year and/or month </response>
    /// <returns>
    /// A response containing a dictionary with nested dictionaries representing time series data for each ride request status.
    /// If successful, the response will include the time series data; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("riderequests/time-series-for-each-status")]
    public async Task<IActionResult> GetRideRequestsTimeseriesForEachStatus([FromQuery] string timeframe, [FromQuery]int? Year, [FromQuery] int? Month)
    {
        var result = await _mediator.Send(new GetRideRequestStatusStatsticsQuery{ Year= Year, Month= Month, option= timeframe});

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Retrieve Ride request Status Counts
    /// </summary>
    /// <remarks>Retrieves the count of ride requests categorized by their status.
    /// The keys in the dictionary represent different ride request statuses, including "waiting", "onroute", "completed", and "cancelled".
    /// This endpoint is accessible to administrators only.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "Ride request status counts retrieved successfully",
    ///    "value": {
    ///        "waiting": 15,      // Count of ride requests in "waiting" status
    ///        "onroute": 20,      // Count of ride requests in "onroute" status
    ///        "completed": 100,   // Count of ride requests in "completed" status
    ///        "cancelled": 5      // Count of ride requests in "cancelled" status
    ///    }
    /// }
    /// </remarks>
    /// <response code="200">Indicates that the ride request status counts were successfully retrieved</response>
    /// <returns>
    /// A response containing a dictionary with keys representing different ride request statuses and their respective counts.
    /// If successful, the response will include the status counts; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("riderequests/status-count")]
    public async Task<IActionResult> GetRideRequestsStatusCount()
    {
        var result = await _mediator.Send(new GetRideRequestAllStatusStatsticsQuery{ });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

    /// <summary>
    /// Get vehicles count time series data.
    /// </summary>
    /// <remarks>Retrieves time series data for vehicles counts based on the specified time series options.
    /// Time series options include "weekly", "monthly", or "yearly". These options allow you to choose the granularity of the time series data.
    /// For instance, if you select the "weekly" option, the time series data will provide vehicles counts for each week within the specified year and month.
    /// Similarly, the "monthly" option will provide vehicles counts for each individual month, and the "yearly" option will provide counts for each year.This endpoint is accessible to administrators only.
    /// The time series data is structured in a dictionary format, where the keys represent the time periods (weeks, months, or years)
    /// and the values represent the corresponding vehicles counts during those periods.
    /// 
    /// Sample Response:
    /// {
    ///    "success": true,
    ///    "message": "vehicles count time series data retrieved successfully",
    ///    "value": {
    ///        "01": 50, // Week 01 of a given month or January of a given year had 50 active vehicles
    ///        "02": 60, // Week 02 of a given month or January of a given year had 60 active vehicles
    ///        ...
    ///    }
    /// }
    /// </remarks>
    /// <param name="option">The time series option ("weekly", "monthly", or "yearly")</param>
    /// <param name="year">The year for which to retrieve the time series data</param>
    /// <param name="month">The month for which to retrieve the time series data</param>
    /// <response code="200">Indicates that the vehicles count time series data was successfully retrieved</response>
    /// <response code="406">Indicates that invalid timeseries options are given either year and/or month </response>
    /// <returns>
    /// A response containing time series data for vehicles counts.
    /// If successful, the response will include the time series data; otherwise, an error message will be provided.
    /// </returns>
    [HttpGet("vehicles/time-series")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetVehiclesTimeseries([FromQuery] string option, [FromQuery] int? year, [FromQuery] int? month)
    {
        var result = await _mediator.Send(new GetNumberOfVehicleQuery { Year = year, Month = month });

        var status = result.Success ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return getResponse(status, result);
    }

}
