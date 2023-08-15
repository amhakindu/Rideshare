using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Drivers.Commands;
using Rideshare.Application.Features.Drivers.Handlers;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Rideshare.UnitTests.Drivers
{
    public class UpdateDriverCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly UpdateDriverCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IResourceManager _mockResourceManger;
        private readonly IFormFile _mockIMG;
        private readonly IFormFile _mockPDF;
        


        public UpdateDriverCommandHandlerTests()
        {
            
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapboxService = MockServices.GetMapboxService();

            _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork.Object)); })
            .CreateMapper();

            _handler = new UpdateDriverCommandHandler( _mockUnitOfWork.Object, _mapper);
            _mockResourceManger = MockResourceManager.GetResourceManager().Object;
            _mockPDF = MockPDF.GetMockPDF();
            _mockIMG = MockIMG.GetMockImage();

        }


        [Fact]
        public async void UpdateDriverValid(){

            UpdateDriverDto updateDriverDto = new UpdateDriverDto {
                Id = 1,
                Rate = new List<double> (){1, 3},
                Experience = 4,
                Address = "new Address",
                LicenseNumber = "newLicenseNum",
                License = _mockIMG
            };
            
            var command = new UpdateDriverCommand{UpdateDriverDto = updateDriverDto, UserId="user1"};

            var result = await _handler.Handle(command, CancellationToken.None);
            
            var driver = await _mockUnitOfWork.Object.DriverRepository.Get(updateDriverDto.Id);


            driver.Rate.ShouldBe(updateDriverDto.Rate);
            driver.Experience.ShouldBe(updateDriverDto.Experience);
            driver.Address.ShouldBe(updateDriverDto.Address);
            driver.License.ShouldBeOfType<string>();
            driver.LicenseNumber.ShouldBe(updateDriverDto.LicenseNumber);   

        }


        [Fact]
        public async void UpdateDriverInvalidAddress()
        {
            UpdateDriverDto updateDriverDto = new UpdateDriverDto {
                Id = 1,
                Rate = new List<double> {2, 5},
                Experience = 4,
                Address = string.Empty,
                LicenseNumber = "34dfdf3",
                License = _mockIMG,

            };
            var command = new UpdateDriverCommand { UpdateDriverDto = updateDriverDto };
            var originalDriver = await _mockUnitOfWork.Object.DriverRepository.Get(updateDriverDto.Id);
            await Should.ThrowAsync<ValidationException>(async () =>
    {
        var result = await _handler.Handle(command, CancellationToken.None);
    });

            var driver = await _mockUnitOfWork.Object.DriverRepository.Get(updateDriverDto.Id);

            driver.Experience.ShouldBe(originalDriver.Experience);
            driver.Address.ShouldBe(originalDriver.Address);
            driver.License.ShouldBeOfType<string>();

            driver.LicenseNumber.ShouldBe(originalDriver.LicenseNumber);

            


        }        
        
        
        [Fact]
        public async void UpdateDriverInvalidExperience()
        {
            UpdateDriverDto updateDriverDto = new UpdateDriverDto {
                Id = 1,
                Rate = new List<double> {2, 5, 0},
                Experience = -3,
                Address = "addis",
                LicenseNumber = "34dfdf3",
                License = _mockIMG,

            };
            var command = new UpdateDriverCommand { UpdateDriverDto = updateDriverDto };
            var originalDriver = await _mockUnitOfWork.Object.DriverRepository.Get(updateDriverDto.Id);
            await Should.ThrowAsync<ValidationException>(async () =>
    {
        var result = await _handler.Handle(command, CancellationToken.None);
    });

            var driver = await _mockUnitOfWork.Object.DriverRepository.Get(updateDriverDto.Id);

            driver.Experience.ShouldBe(originalDriver.Experience);
            driver.Address.ShouldBe(originalDriver.Address);
            driver.License.ShouldBeOfType<string>();

            driver.LicenseNumber.ShouldBe(originalDriver.LicenseNumber);

            


        }        
        

        
        [Fact]
        public async void UpdateDriverInvalidLicense()
        {
            UpdateDriverDto updateDriverDto = new UpdateDriverDto {
                Id = 1,
                Rate = new List<double> {2, 5, 0},
                Experience = 4,
                Address = "addis",
                LicenseNumber = "34dfdf3",
                License = _mockPDF,

            };
            var command = new UpdateDriverCommand { UpdateDriverDto = updateDriverDto };
            var originalDriver = await _mockUnitOfWork.Object.DriverRepository.Get(updateDriverDto.Id);
            await Should.ThrowAsync<ValidationException>(async () =>
    {
        var result = await _handler.Handle(command, CancellationToken.None);
    });

            var driver = await _mockUnitOfWork.Object.DriverRepository.Get(updateDriverDto.Id);

            driver.Experience.ShouldBe(originalDriver.Experience);
            driver.Address.ShouldBe(originalDriver.Address);
            driver.License.ShouldBeOfType<string>();

            driver.LicenseNumber.ShouldBe(originalDriver.LicenseNumber);

            


        }        
        
        
        [Fact]
        public async void UpdateDriverInvalidLicenseNumber()
        {
            UpdateDriverDto updateDriverDto = new UpdateDriverDto {
                Id = 1,
                Rate = new List<double> {2, 5, 0},
                Experience = 4,
                Address = "addis",
                LicenseNumber = string.Empty,
                License = _mockIMG,

            };
            var command = new UpdateDriverCommand { UpdateDriverDto = updateDriverDto };
            var originalDriver = await _mockUnitOfWork.Object.DriverRepository.Get(updateDriverDto.Id);
            await Should.ThrowAsync<ValidationException>(async () =>
    {
        var result = await _handler.Handle(command, CancellationToken.None);
    });

            var driver = await _mockUnitOfWork.Object.DriverRepository.Get(updateDriverDto.Id);

            driver.Experience.ShouldBe(originalDriver.Experience);
            driver.Address.ShouldBe(originalDriver.Address);
            driver.License.ShouldBeOfType<string>();
            driver.LicenseNumber.ShouldBe(originalDriver.LicenseNumber);

            


        }


    }
}