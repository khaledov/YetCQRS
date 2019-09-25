using FluentValidation;
using MediatR;
using Optional;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YetCQRS.Events;

namespace YetCQRS.Commands
{
    public abstract class BaseCommandHandler<TCommand> :
         ICommandHandler<TCommand> where TCommand : ICommand
    {
        protected IEventBus EventBus { get; }
        protected IValidator<TCommand> Validator { get; }
       

        public BaseCommandHandler(
            IValidator<TCommand> validator,
            IEventBus eventBus
            )
        {
            Validator = validator ??
                 throw new InvalidOperationException("It seems that you tried to instantiate a command handler without a validator.");
            EventBus = eventBus;
          
        }

        public abstract Task<Option<Unit, Error>> Handle(TCommand request, CancellationToken cancellationToken);



        protected Task<Unit> PublishEvents(Guid streamId, params Event[] events) =>
           EventBus.Publish(streamId, events);
        protected Option<TCommand, Error> ValidateCommand(TCommand command)
        {
            var validationResult = Validator.Validate(command);

            return validationResult
                .SomeWhen(
                r => r.IsValid,
                Error.Validation(validationResult.Errors.Select(e => e.ErrorMessage)))
               .Map(_ => command);
        }
    }
}
