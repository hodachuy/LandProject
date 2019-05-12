using System;

namespace LandProject.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        LandDbContext Init();
    }
}