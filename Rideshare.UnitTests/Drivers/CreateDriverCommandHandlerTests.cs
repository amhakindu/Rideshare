using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Drivers.Commands;
using Rideshare.Application.Features.Drivers.Handlers;
using Rideshare.Application.Profiles;
using Rideshare.Application.UnitTests.Mocks;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using System;
using Xunit;

namespace Rideshare.UnitTests.Drivers
{
    public class CreateDriverCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly CreateDriverCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IResourceManager _mockResourceManager;
        private readonly IFormFile _mockIMG;
        private readonly IFormFile _mockPdf;



        public CreateDriverCommandHandlerTests()
        {

            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
            _mapper = new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); }).CreateMapper();
            _mockResourceManager = MockResourceManager.GetResourceManager().Object;
            _mockUserRepository = new MockUserRepository();
            _mockIMG = MockIMG.GetMockImage();
            _mockPdf = MockPDF.GetMockPDF();

            _handler = new CreateDriverCommandHandler(_mapper, _mockUnitOfWork.Object,_mockUserRepository.Object, _mockResourceManager );
        }


        [Fact]

        public async Task CreateDriverValidTest()
        {

            CreateDriverDto createDriverDto = new CreateDriverDto
            {
                UserId = "user1",
                Experience = 1,
                Address = "Shiro meda",
                LicenseNumber = "343dld34",
                License = _mockIMG,



            };

            var command = new CreateDriverCommand { CreateDriverDto = createDriverDto };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Value.ShouldBeOfType<int>();
            (await _mockUnitOfWork.Object.DriverRepository.GetAll(1, 10)).Count.ShouldBe(3);




        }

        [Fact]
        public async Task CreateDriverInvalidExperienceTest()
        {

            CreateDriverDto createDriverDto = new CreateDriverDto
            {
                UserId = "user2",
                Experience = -1,
                Address = "Shiro meda",
                LicenseNumber = "343dld34",
                License = _mockIMG,
            };

            var command = new CreateDriverCommand { CreateDriverDto = createDriverDto };

            await Should.ThrowAsync<ValidationException>(async () =>
    {
        var result = await _handler.Handle(command, CancellationToken.None);
    });

            (await _mockUnitOfWork.Object.DriverRepository.GetAll(1, 10)).Count.ShouldBe(2);







        }

        [Fact]
        public async Task CreateDriverInvalidAddressTest()
        {

            CreateDriverDto createDriverDto = new CreateDriverDto
            {
                UserId = "user1",
                Experience = 10,
                Address = string.Empty,
                LicenseNumber = "343dld34",
                License = _mockIMG,
            };

            var command = new CreateDriverCommand { CreateDriverDto = createDriverDto };

            await Should.ThrowAsync<ValidationException>(async () =>
            {
                var result = await _handler.Handle(command, CancellationToken.None);
            });

            (await _mockUnitOfWork.Object.DriverRepository.GetAll(1, 10)).Count.ShouldBe(2);






        }
        [Fact]
        public async Task CreateDriverInvalidLicenseNumberTest()
        {

            CreateDriverDto createDriverDto = new CreateDriverDto
            {
                UserId = "user1",
                Experience = 1,
                Address = "Shiro meda",
                LicenseNumber = string.Empty,
                License = _mockIMG,
            };

            var command = new CreateDriverCommand { CreateDriverDto = createDriverDto };

            await Should.ThrowAsync<ValidationException>(async () =>
            {
                var result = await _handler.Handle(command, CancellationToken.None);
            });

            (await _mockUnitOfWork.Object.DriverRepository.GetAll(1, 10)).Count.ShouldBe(2);



        }



        [Fact]
        public async Task CreateDriverInvalidLicenseTest()
        {

            CreateDriverDto createDriverDto = new CreateDriverDto
            {
                UserId = "user1",
                Experience = 1,
                Address = "Shiro meda",
                LicenseNumber = "3sdkfkd3",
                License = _mockPdf,
            };

            var command = new CreateDriverCommand { CreateDriverDto = createDriverDto };

            await Should.ThrowAsync<ValidationException>(async () =>
            {
                var result = await _handler.Handle(command, CancellationToken.None);
            });

            (await _mockUnitOfWork.Object.DriverRepository.GetAll(1, 10)).Count.ShouldBe(2);



        }

                [Fact]
        public async Task CreateDriverInvalidUserTest()
        {

            CreateDriverDto createDriverDto = new CreateDriverDto
            {
                UserId = "user5",
                Experience = 1,
                Address = "Shiro meda",
                LicenseNumber = "3sdkfkd3",
                License = _mockIMG,
            };

            var command = new CreateDriverCommand { CreateDriverDto = createDriverDto };

            await Should.ThrowAsync<NotFoundException>(async () =>
            {
                var result = await _handler.Handle(command, CancellationToken.None);
            });

            (await _mockUnitOfWork.Object.DriverRepository.GetAll(1, 10)).Count.ShouldBe(2);



        }

    }
}