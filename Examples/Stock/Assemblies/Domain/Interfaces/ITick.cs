using System;

// https://github.com/zkavtaskin/Domain-Driven-Design-Example

namespace Stock.Domain.Interfaces
{
    public interface ITick
    {
        DateTime DateTime { get; }
    }
}
