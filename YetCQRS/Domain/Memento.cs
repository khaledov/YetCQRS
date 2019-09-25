using System;

namespace YetCQRS.Domain
{
    public abstract class Memento
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
