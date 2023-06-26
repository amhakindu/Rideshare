using AutoMapper;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
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

namespace Rideshare.UnitTests.FeedbackTest
{
    public class GetFeedbackDetailQueryHandlerTest
    {
        private IMapper _mapper { get; set; }
        private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
        private GetFeedbackDetailQueryHandler _handler { get; set; }

        public GetFeedbackDetailQueryHandlerTest()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

            var mapboxService = MockServices.GetMapboxService();

            _mapper = new MapperConfiguration(c => { c.AddProfile(new MappingProfile(mapboxService.Object, _mockUnitOfWork.Object)); })
            .CreateMapper();

            _handler = new GetFeedbackDetailQueryHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task GetFeedbackDetailValid()
        {
            var result = await _handler.Handle(new GetFeedbackDetailQuery() { Id = 1 }, CancellationToken.None);
            result.Success.ShouldBeTrue();
        }

        [Fact]
        public async Task GetFeedbackDetailInvalid()
        {
            NotFoundException ex = await Should.ThrowAsync<NotFoundException>(async () =>
            {
                var result = await _handler.Handle(new GetFeedbackDetailQuery() { Id = 10 }, CancellationToken.None);
            });
        }
    }
}
