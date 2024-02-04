using HigherOrLower.Infrastructure.Database;

namespace HigherOrLower.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly IHigherOrLowerDbContext _datacontext;

        public RepositoryBase(IHigherOrLowerDbContext datacontext)
        {
            _datacontext = datacontext;
        }

        protected void InsertAndSubmit<T>(T data)
        {
            _datacontext.InsertAndSubmit(data);
        }

        protected void SubmitChanges()
        {
            _datacontext.SubmitChanges();
        }
    }
}
