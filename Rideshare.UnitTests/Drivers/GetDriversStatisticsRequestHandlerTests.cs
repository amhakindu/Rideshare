using System;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Drivers.Handlers;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.Drivers
{
    public class GetDriversStatisticsRequestHandlerTests
    {

        private readonly GetDriversStatisticsRequestHandler _handler;
        private readonly IUnitOfWork _mockUnitOfWork;


        public GetDriversStatisticsRequestHandlerTests()
        {
            
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork().Object;
            _handler = new GetDriversStatisticsRequestHandler(_mockUnitOfWork);


        }


        [Fact]
        public async void GetDriversStatisticsWeeklyValidTest()
        {
            var request = new GetDriversStatisticsRequest{ Year =  2023, Month = 6 };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Value.ShouldBeOfType<Dictionary<int, int>>();
            result.Value.Count.ShouldBe(5);

        }

        [Fact]
        public async void GetDriversStatisticsWeeklyInvalid()
        {
            var request = new GetDriversStatisticsRequest{ Year =  DateTime.Now.Year + 1, Month = 6 };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Value.ShouldBeOfType<Dictionary<int, int>>();
            result.Value.Count.ShouldBe(5);
            
        }

        [Fact]
        public async void GetDriverStatisticsWeeklyInvalidMonth()
        {
            await Should.ThrowAsync<ValidationException> ( async () => new GetDriversStatisticsRequest{ Year =  DateTime.Now.Year , Month = 13 }); }
        
        [Fact]
        public async void GetDriverStaticsWeeklyInvalidYearOne(){
            await Should.ThrowAsync<ValidationException> ( async () => new GetDriversStatisticsRequest{ Year = 1999 , Month = 6 }); 
            
        }

        


        [Fact]
        public async void GetDriverStatisticsMonthlyValid()
        {
           var request = new GetDriversStatisticsRequest{ Year =  DateTime.Now.Year };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Value.ShouldBeOfType<Dictionary<int, int>>();
            result.Value.Count.ShouldBe(12);
        }


        [Fact]
        public async void GetDriverStatisticsMonthlyInvalid()
        {
            var request = new GetDriversStatisticsRequest{ Year =  DateTime.Now.Year + 1 };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Value.ShouldBeOfType<Dictionary<int, int>>();
            result.Value.Count.ShouldBe(12);
        }

        [Fact]
        public async void GetDriverStatisticsYearlyValid()
        {
            var request = new GetDriversStatisticsRequest{ };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Value.ShouldBeOfType<Dictionary<int, int>>();
            
        }

        [Fact]
        public void GetDriverStatisticsYearlyInvalid()
        {
            // Given
        
            // When
        
            // Then
        }
        
    }
}