using AutoMapper;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Profiles;
using MediatR;
using Moq;
using Shouldly;
using Rideshare.Application.Features.Rates.Commands;
using Rideshare.UnitTests.Mocks;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Features.Rates.Handlers;
using Rideshare.Application.Responses;

namespace Rideshare.UnitTests.RateTest.Commands
{
    public class UpdateRateCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockRepo;
        private readonly UpdateRateDto _rateDto;
        private readonly UpdateRateCommandHandler _handler;
        public UpdateRateCommandHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _rateDto = new UpdateRateDto
            {
                Id = 1,
                Rate = 4.5,
                Description = "Some description"      };

            _handler = new UpdateRateCommandHandler(_mapper, _mockRepo.Object);

        }


        [Fact]
        public async Task Update_With_Invalid_RateDescription()
        {

            _rateDto.Description = "";
            var result = await _handler.Handle(new UpdateRateCommand() { RateDto = _rateDto }, CancellationToken.None);
            result.ShouldBeOfType<BaseResponse<Unit>>();
            result.Success.ShouldBeFalse();

            result.Errors.ShouldNotBeEmpty();
            var rates = await _mockRepo.Object.RateRepository.GetAll();
            rates.Count.ShouldBe(2);

        }

    }
}

