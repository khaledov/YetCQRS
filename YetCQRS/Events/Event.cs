using MediatR;
using System;

namespace YetCQRS.Events
{
    public class Event : INotification
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
