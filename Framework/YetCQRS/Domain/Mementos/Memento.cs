using System;

namespace YetCQRS.Domain.Mementos
{
    [Serializable]
    public abstract class Memento:Entity<Guid>
    {
       public int Version { get; set; }
    }
}
