using MediatR;

namespace M33tingClub.Domain.Utilities;

public interface IDomainEvent : INotification
{
    public Guid Id { get; }
    
    public DateTimeOffset OccurredOn { get; }
}