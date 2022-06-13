using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using StreamInstruments.DataObjects;

namespace StreamInstruments.DataAccess.Services;

public class PrimaryKeyGenerator<T> : ValueGenerator<Task<long>> where T : EntityBase
{
    public override async Task<long> Next(EntityEntry entry)
    {
        long idToTry = 0;
        var entityExistsWithId = true;

        while (entityExistsWithId)
        {
            idToTry = new Random().NextInt64(long.MaxValue);
            var entity = await entry.Context.Set<T>().FindAsync(idToTry);
            entityExistsWithId = entity != null;
        }

        return idToTry;
    }

    public override bool GeneratesTemporaryValues => false;
}