using AutoMapper;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Drivers.Commands;
using Rideshare.Application.Features.Drivers.Handlers;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using System;
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
            _mapper = new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); }).CreateMapper();

            _handler = new DeleteDriverCommandHandler(_mapper, _mockUnitOfWork.Object);
        }

        [Fact]
        public async void DeleteDriverValid()
        {

            var command = new DeleteDriverCommand { Id = 1 };

            var result = await _handler.Handle(command, CancellationToken.None);
            

            (await _mockUnitOfWork.Object.DriverRepository.GetAll(1, 10)).Count.ShouldBe(1);


        }


  [Fact]
public async Task DeleteDriverInValid()
{
    var command = new DeleteDriverCommand { Id = 0 };

    await Should.ThrowAsync<NotFoundException>(async () =>
    {
        var result = await _handler.Handle(command, CancellationToken.None);
    });

    (await _mockUnitOfWork.Object.DriverRepository.GetAll(1, 10)).Count.ShouldBe(2);
}


    }
}