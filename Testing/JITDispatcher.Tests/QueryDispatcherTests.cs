using Moq;
using JITDispatcher.Dispatchers;
using JITDispatcher.Queries;

namespace JITDispatcher.Tests
{
    public class QueryDispatcherTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly QueryDispatcher _queryDispatcher;

        public QueryDispatcherTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _queryDispatcher = new QueryDispatcher(_serviceProviderMock.Object);
        }

        [Fact]
        public async Task QueryAsync_ShouldThrowArgumentNullException_WhenQueryIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _queryDispatcher.QueryAsync<IQuery<string>, string>(null, CancellationToken.None));
        }

        [Fact]
        public async Task QueryAsync_ShouldThrowInvalidOperationException_WhenHandlerNotFound()
        {
            var query = new Mock<IQuery<string>>().Object;

            await Assert.ThrowsAsync<InvalidOperationException>(() => _queryDispatcher.QueryAsync<IQuery<string>, string>(query, CancellationToken.None));
        }

        [Fact]
        public async Task QueryAsync_ShouldExecuteHandler_WhenQueryIsValid()
        {
            var query = new Mock<IQuery<string>>().Object;
            var handlerMock = new Mock<IQueryHandler<IQuery<string>, string>>();
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IQueryHandler<IQuery<string>, string>)))
                                .Returns(handlerMock.Object);

            await _queryDispatcher.QueryAsync<IQuery<string>, string>(query, CancellationToken.None);

            handlerMock.Verify(h => h.Execute(query, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
