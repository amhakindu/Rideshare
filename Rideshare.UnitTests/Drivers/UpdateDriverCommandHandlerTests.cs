using System;
using AutoMapper;
using Moq;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Contracts.Persistence;
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



        public UpdateDriverCommandHandlerTests()
        {
            
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
            _mapper = new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); }).CreateMapper();

            _handler = new UpdateDriverCommandHandler( _mockUnitOfWork.Object, _mapper);

        }

        [Fact]
        public async void UpdateDriverValid(){

            UpdateDriverDto updateDriverDto = new UpdateDriverDto {
                Id = 1,
                Rate = new List<double> (){1, 3},
                Experience = 4,
                Address = "new Address",
                LicenseNumber = "newLicenseNum",
                License = "newLicense"

            };
            
            var command = new UpdateDriverCommand{UpdateDriverDto = updateDriverDto};

            

            var result = await _handler.Handle(command, CancellationToken.None);

            var driver = await _mockUnitOfWork.Object.DriverRepository.Get(updateDriverDto.Id);

            driver.Rate.ShouldBe(updateDriverDto.Rate);
            driver.Experience.ShouldBe(updateDriverDto.Experience);
            driver.Address.ShouldBe(updateDriverDto.Address);
            driver.License.ShouldBe(updateDriverDto.License);
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
                License = "license",

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
            driver.License.ShouldBe(originalDriver.License);
            driver.LicenseNumber.ShouldBe(originalDriver.LicenseNumber);

            


        }        
        
        
        [Fact]
        public async void UpdateDriverInvalidExperience()
        {
            UpdateDriverDto updateDriverDto = new UpdateDriverDto {
                Id = 1,
                Rate = new List<int> {2, 5},
                Experience = -3,
                Address = "addis",
                LicenseNumber = "34dfdf3",
                License = "license",

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
            driver.License.ShouldBe(originalDriver.License);
            driver.LicenseNumber.ShouldBe(originalDriver.LicenseNumber);

            


        }        
        

        
        [Fact]
        public async void UpdateDriverInvalidLicense()
        {
            UpdateDriverDto updateDriverDto = new UpdateDriverDto {
                Id = 1,
                Rate = new List<int> {2, 5},
                Experience = 4,
                Address = "addis",
                LicenseNumber = "34dfdf3",
                License = string.Empty,

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
            driver.License.ShouldBe(originalDriver.License);
            driver.LicenseNumber.ShouldBe(originalDriver.LicenseNumber);

            


        }        
        
        
        [Fact]
        public async void UpdateDriverInvalidLicenseNumber()
        {
            UpdateDriverDto updateDriverDto = new UpdateDriverDto {
                Id = 1,
                Rate = new List<int> {2, 5},
                Experience = 4,
                Address = "addis",
                LicenseNumber = string.Empty,
                License = "license",

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
            driver.License.ShouldBe(originalDriver.License);
            driver.LicenseNumber.ShouldBe(originalDriver.LicenseNumber);

            


        }


    }
}