using System;
using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Drivers.Handlers;
using Rideshare.Application.Features.Drivers.Queries;
using Rideshare.Application.Profiles;
using Rideshare.Domain.Entities;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.Drivers
{
    public class GetDriverRequestHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly GetDriverRequestHandler _handler;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;



        public GetDriverRequestHandlerTests()
        {
            
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapboxService = MockServices.GetMapboxService();

            _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork.Object)); })
            .CreateMapper();

            _handler = new GetDriverRequestHandler(_mapper, _mockUnitOfWork.Object);
        }

        [Fact]
        public async void  GetDriverRequestValid(){

            var request = new GetDriverRequest{Id = 1};

            var result = await _handler.Handle(request, CancellationToken.None );

            result.Value.ShouldNotBe(null);
            result.Value.ShouldBeOfType<DriverDetailDto>();
            





    

        }


        [Fact]
        public async void GetDriverRequestInvalid(){

            var request = new GetDriverRequest { Id = 0 };

             await Should.ThrowAsync<NotFoundException>(async () =>
    {
        var result = await _handler.Handle(request, CancellationToken.None);
    });


            

        }
        
    }
}