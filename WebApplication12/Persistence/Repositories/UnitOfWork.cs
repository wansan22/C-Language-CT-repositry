using Application.Interfaces.Repositories;

namespace Persistence.Repositories
{
    internal class UnitOfWork(ApplicationContext context) : IUnitOfWork
    {
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
