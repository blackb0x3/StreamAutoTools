namespace StreamInstruments.DataObjects;

public abstract class EntityBase
{
    public long Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime LastUpdatedOn { get; set; }
}