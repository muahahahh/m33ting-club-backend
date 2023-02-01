namespace M33tingClub.Application.Utilities;

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}