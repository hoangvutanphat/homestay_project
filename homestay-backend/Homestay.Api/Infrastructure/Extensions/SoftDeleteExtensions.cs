using Homestay.Api.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Homestay.Api.Infrastructure.Extensions;

public static class SoftDeleteExtensions
{
    public static void ConfigureSoftDelete(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var property = System.Linq.Expressions.Expression.Property(parameter, nameof(ISoftDeletable.DeletedAt));
                var nullConstant = System.Linq.Expressions.Expression.Constant(null, typeof(DateTime?));
                var compareExpression = System.Linq.Expressions.Expression.Equal(property, nullConstant);
                var lambda = System.Linq.Expressions.Expression.Lambda(compareExpression, parameter);

                entityType.SetQueryFilter(lambda);
                
                var deletedAtProperty = entityType.FindProperty(nameof(ISoftDeletable.DeletedAt));
                if (deletedAtProperty != null)
                {
                    var index = entityType.AddIndex(deletedAtProperty);
                    index.SetDatabaseName($"IX_{entityType.GetTableName()}_DeletedAt");
                }
            }
        }
    }
    
    public static IQueryable<T> IncludeDeleted<T>(this IQueryable<T> query) 
        where T : class, ISoftDeletable
    {
        return query.IgnoreQueryFilters();
    }
    
    public static IQueryable<T> OnlyDeleted<T>(this IQueryable<T> query) 
        where T : class, ISoftDeletable
    {
        return query.IgnoreQueryFilters().Where(e => e.DeletedAt != null);
    }
}
