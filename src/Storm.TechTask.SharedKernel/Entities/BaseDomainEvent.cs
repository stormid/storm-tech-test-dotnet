using MediatR;

namespace Storm.TechTask.SharedKernel.Entities
{
    [Serializable]
    public abstract class BaseDomainEvent : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}
