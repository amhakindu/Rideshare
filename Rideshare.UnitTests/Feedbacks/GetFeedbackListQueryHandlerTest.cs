using AutoMapper;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Feedbacks.Handlers;
using Rideshare.Application.Features.Feedbacks.Queries;
using Rideshare.Application.Profiles;
using Rideshare.UnitTests.Mocks;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rideshare.UnitTests.Feedbacks
{
    public class GetFeedbackListQueryHandlerTest
    {
        private IMapper _mapper { get; set; }
        private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
        private GetFeedbackListQueryHandler _handler { get; set; }

        public GetFeedbackListQueryHandlerTest()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

            var mapboxService = MockServices.GetMapboxService();

            _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork.Object)); })
            .CreateMapper();


            _handler = new GetFeedbackListQueryHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task GetSeatListValid()
        {
            var result = await _handler.Handle(new GetFeedbackListQuery() { }, CancellationToken.None);
            result.Success.ShouldBeTrue();
        }
    }
}
