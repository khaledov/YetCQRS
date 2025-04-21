using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using JITDispatcher.Commands;
using JITDispatcher.Dispatchers;

namespace JITDispatcher.Tests
{
    public class CommandDispatcherTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly CommandDispatcher _commandDispatcher;

        public CommandDispatcherTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _commandDispatcher = new CommandDispatcher(_serviceProviderMock.Object);
        }

        [Fact]
        public async Task SendAsync_ShouldThrowArgumentNullException_WhenCommandIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _commandDispatcher.SendAsync<ICommand>(null, CancellationToken.None));
        }

        [Fact]
        public async Task SendAsync_ShouldThrowInvalidOperationException_WhenHandlerNotFound()
        {
            var command = new Mock<ICommand>().Object;

            await Assert.ThrowsAsync<InvalidOperationException>(() => _commandDispatcher.SendAsync(command, CancellationToken.None));
        }

        [Fact]
        public async Task SendAsync_ShouldExecuteHandler_WhenCommandIsValid()
        {
            var command = new Mock<ICommand>().Object;
            var validator  =new Mock<ICommandValidator<ICommand>>();    
            validator.Setup(v => v.Validate(command)).Returns(new ValidationResult(null,true));
            var handlerMock = new Mock<ICommandHandler<ICommand>>();
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(ICommandHandler<ICommand>)))
                                .Returns(handlerMock.Object);

            await _commandDispatcher.SendAsync(command, CancellationToken.None);

            handlerMock.Verify(h => h.Execute(command, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
