namespace Pokemons.DataLayer.Database.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    Task BeginTransaction();
    Task RollbackTransaction();
    Task CommitTransaction();
}