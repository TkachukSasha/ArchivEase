namespace SharedKernel.Abstractions;

public abstract class Entity<TEntityId>
    where TEntityId : TypeId
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Entity()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    protected Entity(TEntityId id)
    {
        Id = id;
        CreatedOnUtc = DateTimeOffset.UtcNow;
        RecordState = RecordState.Active;
    }

    public TEntityId Id { get; }

    public DateTimeOffset? CreatedOnUtc { get; }

    public RecordState RecordState { get; } = RecordState.Active;
}