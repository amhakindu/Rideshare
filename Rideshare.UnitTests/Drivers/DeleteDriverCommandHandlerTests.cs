using Moq;
using System;
using Shouldly;
using AutoMapper;
using Rideshare.UnitTests.Mocks;
using Rideshare.Application.Profiles;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.Commands;
using Rideshare.Application.Features.Drivers.Handlers;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace Rideshare.UnitTests.Drivers
{
    public class DeleteDriverCommandHandlerTests
    {

        private readonly IMapper _mapper;
        private readonly DeleteDriverCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;



        public DeleteDriverCommandHandlerTests()
        {

            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapboxService = MockServices.GetMapboxService();

            _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork.Object)); })
            .CreateMapper();

            _handler = new DeleteDriverCommandHandler(_mapper, _mockUnitOfWork.Object);
        }

        [Fact]
        public async void DeleteDriverValid()
        {

            var command = new DeleteDriverCommand { Id = 1 , UserId="user1"};

            var result = await _handler.Handle(command, CancellationToken.None);
            

            (await _mockUnitOfWork.Object.DriverRepository.GetAll(1, 10)).Count.ShouldBe(2);


        }


  [Fact]
public async Task DeleteDriverInValid()
{
    var command = new DeleteDriverCommand { Id = 0 };

    await Should.ThrowAsync<NotFoundException>(async () =>
    {
        var result = await _handler.Handle(command, CancellationToken.None);
    });

    (await _mockUnitOfWork.Object.DriverRepository.GetAll(1, 10)).Count.ShouldBe(3);
}


    }
}