using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ULearn.Be.Data.Context;

public static class ContextExtensions
{
    public static void ApplyRemoveOneToManyCascade(this ModelBuilder builder)
    {
        builder.EntityLoop(et => et.GetForeignKeys()
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
            .ToList()
            .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict));
    }

    private static void EntityLoop(this ModelBuilder builder, Action<IMutableEntityType> action)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            action(entityType);
        }
    }
}