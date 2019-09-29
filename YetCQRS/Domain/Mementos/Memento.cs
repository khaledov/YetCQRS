using System;

namespace YetCQRS.Domain.Mementos
{
    [Serializable]
    public abstract class Memento
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
