using Queries.Core.Domain;

namespace Queries.Core.Repositories
{
    // LVET: IRepository is generic, but here we specify project class related methods
    public interface IAuthorRepository : IRepository<Author>
    {
        Author GetAuthorWithCourses(int id);
    }
}