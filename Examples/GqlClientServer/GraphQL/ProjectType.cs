using GqlClientServer.Database;
using GqlClientServer.Models;
using HotChocolate.Types;
using System.Linq;

namespace GqlClientServer.GraphQL
{
    public class ProjectType : ObjectType<Project>
    {
    }

    public class TimeLogType : ObjectType<TimeLog>
    {
    }

    public class Query
    {
        private readonly TimeGraphContext dbContext;

        public Query(TimeGraphContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Project> Projects => dbContext.Projects;
        public IQueryable<TimeLog> TimeLogs => dbContext.TimeLogs;
    }
}
