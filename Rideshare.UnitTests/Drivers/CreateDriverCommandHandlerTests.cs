//using AutoMapper;
//using Moq;
//using Rideshare.Application.Common.Dtos.Drivers;
//using Rideshare.Application.Contracts.Persistence;
//using Rideshare.Application.Exceptions;
//using Rideshare.Application.Features.Drivers.Commands;
//using Rideshare.Application.Features.Drivers.Handlers;
//using Rideshare.Application.Profiles;
//using Rideshare.UnitTests.Mocks;
//using Shouldly;
//using System;
//using Xunit;

//namespace Rideshare.UnitTests.Drivers
//{
//    public class CreateDriverCommandHandlerTests
//    {
//        private readonly IMapper _mapper;
//        private readonly CreateDriverCommandHandler _handler;
//        private readonly Mock<IUnitOfWork> _mockUnitOfWork;



//        public CreateDriverCommandHandlerTests()
//        {

//            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
//            _mapper = new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); }).CreateMapper();

//            _handler = new CreateDriverCommandHandler(_mapper, _mockUnitOfWork.Object);
//        }


//        [Fact]

//        public async Task CreateDriverValidTest()
//        {

//            CreateDriverDto createDriverDto = new CreateDriverDto
//            {

//                Experience = 1,
//                Address = "Shiro meda",
//                LicenseNumber = "343dld34",
//                License = "amhakindu",


//            };

//            var command = new CreateDriverCommand { CreateDriverDto = createDriverDto };

//            var result = await _handler.Handle(command, CancellationToken.None);

//            result.Value.ShouldBeOfType<int>();
//            (await _mockUnitOfWork.Object.DriverRepository.GetAll()).Count.ShouldBe(3);




//        }

//        [Fact]
//        public async Task CreateDriverInvalidExperienceTest()
//        {

//            CreateDriverDto createDriverDto = new CreateDriverDto
//            {
//                Experience = -1,
//                Address = "Shiro meda",
//                LicenseNumber = "343dld34",
//                License = "amhakindu",
//            };

//            var command = new CreateDriverCommand { CreateDriverDto = createDriverDto };

//            await Should.ThrowAsync<ValidationException>(async () =>
//    {
//        var result = await _handler.Handle(command, CancellationToken.None);
//    });

//            (await _mockUnitOfWork.Object.DriverRepository.GetAll()).Count.ShouldBe(2);







//        }

//        [Fact]
//        public async Task CreateDriverInvalidAddressTest()
//        {

//            CreateDriverDto createDriverDto = new CreateDriverDto
//            {
//                Experience = 10,
//                Address = string.Empty,
//                LicenseNumber = "343dld34",
//                License = "amhakindu",
//            };

//            var command = new CreateDriverCommand { CreateDriverDto = createDriverDto };

//            await Should.ThrowAsync<ValidationException>(async () =>
//            {
//                var result = await _handler.Handle(command, CancellationToken.None);
//            });

//            (await _mockUnitOfWork.Object.DriverRepository.GetAll()).Count.ShouldBe(2);







//        }
//        [Fact]
//        public async Task CreateDriverInvalidLicenseNumberTest()
//        {

//            CreateDriverDto createDriverDto = new CreateDriverDto
//            {
//                Experience = 1,
//                Address = "Shiro meda",
//                LicenseNumber = string.Empty,
//                License = "amhakindu",
//            };

//            var command = new CreateDriverCommand { CreateDriverDto = createDriverDto };

//            await Should.ThrowAsync<ValidationException>(async () =>
//            {
//                var result = await _handler.Handle(command, CancellationToken.None);
//            });

//            (await _mockUnitOfWork.Object.DriverRepository.GetAll()).Count.ShouldBe(2);



//        }



//        [Fact]
//        public async Task CreateDriverInvalidLicenseTest()
//        {

//            CreateDriverDto createDriverDto = new CreateDriverDto
//            {
//                Experience = 1,
//                Address = "Shiro meda",
//                LicenseNumber = "3sdkfkd3",
//                License = string.Empty,
//            };

//            var command = new CreateDriverCommand { CreateDriverDto = createDriverDto };

//            await Should.ThrowAsync<ValidationException>(async () =>
//            {
//                var result = await _handler.Handle(command, CancellationToken.None);
//            });

//            (await _mockUnitOfWork.Object.DriverRepository.GetAll()).Count.ShouldBe(2);



//        }

//    }
//}