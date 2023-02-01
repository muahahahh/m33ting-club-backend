using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Utilities;
using MediatR;

namespace M33tingClub.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly M33tingClubDbContext _m33TingClubDbContext;
    private readonly IMediator _mediator;

    public UnitOfWork(
        M33tingClubDbContext m33TingClubDbContext, 
        IMediator mediator)
    {
        _m33TingClubDbContext = m33TingClubDbContext;
        _mediator = mediator;
    }

    public async Task<int> CommitAsync()
    {
        await ProcessDomainEvents();
        return await _m33TingClubDbContext.SaveChangesAsync();  
    }

    private async Task ProcessDomainEvents()
    {
        var domainEvents = _m33TingClubDbContext.ChangeTracker
            .Entries<Entity>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();
        
        _m33TingClubDbContext.ChangeTracker
            .Entries<Entity>()
            .ToList()
            .ForEach(x => x.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}