namespace M33tingClub.Domain.Utilities;

public abstract class DomainEventBase : IDomainEvent
{
    public Guid Id { get; }
    
    public DateTimeOffset OccurredOn { get; }

    protected DomainEventBase()
    {
        Id = Guid.NewGuid();
        OccurredOn = Clock.Now;
    }
}