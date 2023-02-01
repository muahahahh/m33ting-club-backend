using System.ComponentModel.DataAnnotations.Schema;
using M33tingClub.Domain.Utilities.Exceptions;

namespace M33tingClub.Domain.Utilities;

public abstract class Entity
{
    [NotMapped]
    private readonly List<DomainEventBase> _domainEvents = new();
    
    [NotMapped]
    public IReadOnlyList<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void AddDomainEvent(DomainEventBase domainEvent) 
        => _domainEvents.Add(domainEvent);

    protected void CheckRule(IRule rule)
    {
        if (rule.IsBroken())
            throw new RuleValidationException(rule);
    }
}