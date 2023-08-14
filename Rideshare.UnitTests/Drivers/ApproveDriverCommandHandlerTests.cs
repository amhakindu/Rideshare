using AutoMapper;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Xunit;
using Rideshare.Application.Features.Drivers.Handlers;
using Rideshare.Application.Features.Drivers.Commands;
using Shouldly;
using MediatR;
using Rideshare.Application.Exceptions;

namespace Rideshare.UnitTests.Drivers
{
    public class ApproveDriverCommandHandlerTests
    {


        private readonly IUnitOfWork _mockUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ApproveDriverCommandHandler _handler;


        public ApproveDriverCommandHandlerTests()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork().Object;
            var mapboxService = MockServices.GetMapboxService();

            _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork)); })
            .CreateMapper();

            _handler = new ApproveDriverCommandHandler(_mockUnitOfWork, _mapper);



        }


        [Fact]

        public async void ApproveDriverCommandValid(){
            
            var approveDriverDto = new ApproveDriverDto {Id = 1 , Verified = true};

            var command = new ApproveDriverCommand {ApproveDriverDto = approveDriverDto};

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.ShouldBe(true);
            result.Value.ShouldBeOfType<Unit>();
        }


        [Fact]
        public async void ApproveDriverCommandInvalid()
        {

            var command = new ApproveDriverCommand {ApproveDriverDto = new ApproveDriverDto {Id = 0, Verified = true}};

            
            
            await Should.ThrowAsync<ValidationException>(async () =>
    {
        var result = await _handler.Handle(command, CancellationToken.None);
    });

        }
        
    }
}