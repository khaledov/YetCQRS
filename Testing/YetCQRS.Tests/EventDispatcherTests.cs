using Microsoft.Extensions.DependencyInjection;
using Moq;
using YetCQRS.Dispatchers;
using YetCQRS.Events;

namespace YetCQRS.Tests
{
    public class EventDispatcherTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly EventDispatcher _eventDispatcher;

        public EventDispatcherTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _eventDispatcher = new EventDispatcher(_serviceProviderMock.Object);
        }

        [Fact]
        public async Task PublishAsync_ShouldThrowArgumentNullException_WhenEventIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _eventDispatcher.PublishAsync<IEvent>(null, CancellationToken.None));
        }

        [Fact]
        public async Task PublishAsync_ShouldExecuteHandler_WhenEventIsValid()
        {
            var @event = new Mock<IEvent>().Object;
            var handlerMock = new Mock<IEventHandler<IEvent>>();
            _serviceProviderMock.Setup(sp => sp.GetServices<IEventHandler<IEvent>>())
                                .Returns(new List<IEventHandler<IEvent>>() { handlerMock.Object});

            await _eventDispatcher.PublishAsync(@event, CancellationToken.None);

            handlerMock.Verify(h => h.Handle(@event, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
