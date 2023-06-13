using AutoMapper;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Profiles;
using MediatR;
using Moq;
using Shouldly;
using Rideshare.Application.Features.Rates.Handlers;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.UnitTests.Mocks;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Rates.Commands;
using Xunit;

namespace CineFlex.Application.UnitTest.RateTest.Command
{
    public class DeleteRateCommandHandlerTest
    {

        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockRepo;
        private int _id { get; set; }
        private readonly DeleteRateCommandHandler _handler;
        private readonly CreateRateDto _rateDto;
        public DeleteRateCommandHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _rateDto = new CreateRateDto
            {
				Id = 1,
				Rate = 2.4,
				RaterId = 1,
				DriverId = 3,
				Description = "true",
            };
            _id = 1;

            _handler = new DeleteRateCommandHandler(_mapper, _mockRepo.Object);

        }


        [Fact]
        public async Task DeleteRate()
        {

            var result = await _handler.Handle(new DeleteRateCommand() { RateId = _id }, CancellationToken.None);
            result.ShouldBeOfType<BaseResponse<Unit>>();
            result.Success.ShouldBeTrue();

            var rates = await _mockRepo.Object.RateRepository.GetAll();
            rates.Count().ShouldBe(1);
        }

        [Fact]
        public async Task Delete_Rate_Doesnt_Exist()
        {

            _id  = 0;
            var result = await _handler.Handle(new DeleteRateCommand() { RateId = _id }, CancellationToken.None);
            result.ShouldBe(null);
        
            var rates = await _mockRepo.Object.RateRepository.GetAll();
            rates.Count.ShouldBe(2);

        }
    }
}


