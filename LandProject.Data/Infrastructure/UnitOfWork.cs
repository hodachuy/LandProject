namespace LandProject.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private LandDbContext _dbContext;
        private IDbFactory _dbFactory;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this._dbFactory = dbFactory;
        }

        public LandDbContext DbContext
        {
            get { return _dbContext ?? (_dbContext = _dbFactory.Init()); }
        }
        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
