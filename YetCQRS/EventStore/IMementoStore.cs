using YetCQRS.Domain.Mementos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace YetCQRS.EventStore
{
    public interface IMementoStore
    {
        Task<Memento> Get(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        Task Save(Memento snapshot, CancellationToken cancellationToken = default(CancellationToken));
    }
}
