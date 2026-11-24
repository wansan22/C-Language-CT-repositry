namespace Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
