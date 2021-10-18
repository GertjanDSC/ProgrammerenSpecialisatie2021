using System;

namespace Queries.Core
{
    public interface IUnitOfWorkBase: IDisposable
    {
        int Complete();
    }
}