using System;

namespace JITDispatcher.Domain.Mementos
{
    [Serializable]
    public abstract class Memento:Entity<Guid>
    {
       public int Version { get; set; }
    }
}
