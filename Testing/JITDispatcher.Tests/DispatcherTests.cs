using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using JITDispatcher.Commands;
using JITDispatcher.Dispatchers;
using JITDispatcher.Events;
using JITDispatcher.Queries;

namespace JITDispatcher.Tests
{
    public class DispatcherTests
    {
        private readonly Mock<ICommandDispatcher> _commandDispatcherMock;
        private readonly Mock<IQueryDispatcher> _queryDispatcherMock;
        private readonly Mock<IEventDispatcher> _eventDispatcherMock;
        private readonly IDispatcher _dispatcher;

        public DispatcherTests()
        {
            _commandDispatcherMock = new Mock<ICommandDispatcher>();
            _queryDispatcherMock = new Mock<IQueryDispatcher>();
            _eventDispatcherMock = new Mock<IEventDispatcher>();
            _dispatcher = new Dispatcher(_commandDispatcherMock.Object, _queryDispatcherMock.Object, _eventDispatcherMock.Object);
        }

        [Fact]
        public async Task PublishAsync_ShouldCallEventDispatcher()
        {
            var @event = new Mock<IEvent>().Object;
            await _dispatcher.PublishAsync(@event, CancellationToken.None);
            _eventDispatcherMock.Verify(ed => ed.PublishAsync(@event, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task QueryAsync_ShouldCallQueryDispatcher()
        {
            var query = new Mock<IQuery<string>>().Object;
            await _dispatcher.QueryAsync<IQuery<string>, string>(query, CancellationToken.None);
            _queryDispatcherMock.Verify(qd => qd.QueryAsync<IQuery<string>, string>(query, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task SendAsync_ShouldCallCommandDispatcher()
        {
            var command = new Mock<ICommand>().Object;
            await _dispatcher.SendAsync(command, CancellationToken.None);
            _commandDispatcherMock.Verify(cd => cd.SendAsync(command, CancellationToken.None), Times.Once);
        }
    }
}
