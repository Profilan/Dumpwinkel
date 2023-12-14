using System;

namespace Profilan.SharedKernel
{
    public interface IDomainEvent
    {
        DateTime DateTimeEventOccurred { get; }
    }
}
