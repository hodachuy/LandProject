namespace LandProject.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private LandDbContext dbContext;

        public LandDbContext Init()
        {
            return dbContext ?? (dbContext = new LandDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}