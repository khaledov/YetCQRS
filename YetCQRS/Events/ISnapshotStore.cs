using YetCQRS.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace YetCQRS.Events
{
    public interface ISnapshotStore
    {
        Task<Memento> Get(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        Task Save(Memento snapshot, CancellationToken cancellationToken = default(CancellationToken));
    }
}
