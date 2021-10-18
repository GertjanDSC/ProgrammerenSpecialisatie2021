using Queries.Core.Repositories;

namespace Queries.Core
{

    public interface IUnitOfWork : IUnitOfWorkBase
    {
        ICourseRepository Courses { get; }
        IAuthorRepository Authors { get; }
    }
}