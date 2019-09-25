﻿using MediatR;
using Optional;

namespace YetCQRS.Commands
{
    public interface ICommandHandler<in TCommand> :
        IRequestHandler<TCommand, Option<Unit, Error>>
        where TCommand : ICommand
    {
    }

    public interface ICommandHandler<in TCommand, TResult> :
        IRequestHandler<TCommand, Option<TResult, Error>>
        where TCommand : ICommand<TResult>
    {

    }
}
